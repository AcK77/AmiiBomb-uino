using System;
using System.Collections.Generic;
using System.Threading;
using ArduinoUploader.Hardware;
using ArduinoUploader.Hardware.Memory;
using ArduinoUploader.Protocols;
using ArduinoUploader.Protocols.STK500v2;
using ArduinoUploader.Protocols.STK500v2.Messages;
using NLog;
using EnableProgrammingModeRequest = ArduinoUploader.Protocols.STK500v2.Messages.EnableProgrammingModeRequest;
using GetParameterRequest = ArduinoUploader.Protocols.STK500v2.Messages.GetParameterRequest;
using GetSyncRequest = ArduinoUploader.Protocols.STK500v2.Messages.GetSyncRequest;
using GetSyncResponse = ArduinoUploader.Protocols.STK500v2.Messages.GetSyncResponse;
using LoadAddressRequest = ArduinoUploader.Protocols.STK500v2.Messages.LoadAddressRequest;

namespace ArduinoUploader.BootloaderProgrammers
{
    internal class WiringBootloaderProgrammer : ArduinoBootloaderProgrammer
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private const string EXPECTED_DEVICE_SIGNATURE = "AVRISP_2";
        private const string STK500v2_CORRUPT_WRAPPER = "STK500V2 wrapper corrupted ({0})!";

        private readonly IDictionary<MemoryType, byte> readCommands = new Dictionary<MemoryType, byte>()
        {
            { MemoryType.FLASH, Constants.CMD_READ_FLASH_ISP },
            { MemoryType.EEPROM, Constants.CMD_READ_EEPROM_ISP }
        };

        private readonly IDictionary<MemoryType, byte> writeCommands = new Dictionary<MemoryType, byte>()
        {
            { MemoryType.FLASH, Constants.CMD_PROGRAM_FLASH_ISP },
            { MemoryType.EEPROM, Constants.CMD_PROGRAM_EEPROM_ISP }
        };

        private string deviceSignature;
        private static byte sequenceNumber;
        protected static byte LastCommandSequenceNumber;
        protected static byte SequenceNumber
        {
            get
            {
                if (sequenceNumber == 255) sequenceNumber = 0;
                return ++sequenceNumber;
            }
        }

        public WiringBootloaderProgrammer(SerialPortConfig serialPortConfig, IMCU mcu)
            : base(serialPortConfig, mcu)
        {
        }

        public override void Open()
        {
            base.Open();
            // The Uno (and Nano R3) will have auto-reset because DTR is true when opening the serial connection, 
            // so we just wait a small amount of time for it to come back.
            Thread.Sleep(50);
        }

        protected override void Reset()
        {
            ToggleDtrRts(50, 50, true);
        }

        protected override void Send(IRequest request)
        {
            var requestBodyLength = request.Bytes.Length;
            var totalMessageLength = requestBodyLength + 6;
            var wrappedBytes = new byte[totalMessageLength];
            wrappedBytes[0] = Constants.MESSAGE_START;
            wrappedBytes[1] = LastCommandSequenceNumber = SequenceNumber;
            wrappedBytes[2] = (byte)(requestBodyLength >> 8);
            wrappedBytes[3] = (byte) (requestBodyLength & 0xFF);
            wrappedBytes[4] = Constants.TOKEN;
            Buffer.BlockCopy(request.Bytes, 0, wrappedBytes, 5, requestBodyLength);

            byte checksum = 0;
            for (var i = 0; i < totalMessageLength - 1; i++) checksum ^= wrappedBytes[i];
            wrappedBytes[totalMessageLength -1] = checksum;

            request.Bytes = wrappedBytes;
            base.Send(request);
        }

        protected TResponse Receive<TResponse>() where TResponse: Response
        {
            var response = (TResponse) Activator.CreateInstance(typeof(TResponse));

            var wrappedResponseBytes = new byte[300];
            var messageStart = ReceiveNext();
            if (messageStart != Constants.MESSAGE_START)
            {
                logger.Warn(
                    STK500v2_CORRUPT_WRAPPER,
                    "No Start Message detected!");
                return null;                
            }
            wrappedResponseBytes[0] = (byte) messageStart;
            logger.Trace("Received MESSAGE_START.");

            var seqNumber = ReceiveNext();
            if (seqNumber != LastCommandSequenceNumber)
            {
                logger.Warn(
                    STK500v2_CORRUPT_WRAPPER,
                    "Wrong sequence number!");
                return null;                      
            }
            wrappedResponseBytes[1] = sequenceNumber;
            logger.Trace("Received sequence number.");

            var messageSizeHighByte = ReceiveNext();
            if (messageSizeHighByte == -1)
            {
                logger.Warn(
                    STK500v2_CORRUPT_WRAPPER,
                    "Timeout ocurred!");
                return null;                       
            }
            wrappedResponseBytes[2] = (byte) messageSizeHighByte;

            var messageSizeLowByte = ReceiveNext();
            if (messageSizeLowByte == -1)
            {
                logger.Warn(
                    STK500v2_CORRUPT_WRAPPER,
                    "Timeout ocurred!");
                return null;
            }
            wrappedResponseBytes[3] = (byte) messageSizeLowByte;

            var messageSize = (messageSizeHighByte << 8) + messageSizeLowByte;
            logger.Trace("Received message size: {0}.", messageSize);

            var token = ReceiveNext();
            if (token != Constants.TOKEN)
            {
                logger.Warn(
                   STK500v2_CORRUPT_WRAPPER,
                   "Token not received!");
                return null;               
            }
            wrappedResponseBytes[4] = (byte) token;

            logger.Trace("Received TOKEN.");

            var payload = ReceiveNext(messageSize);
            if (payload == null)
            {
                logger.Warn(
                   STK500v2_CORRUPT_WRAPPER,
                   "Inner message not received!");
                return null;                               
            }

            Buffer.BlockCopy(payload, 0, wrappedResponseBytes, 5, messageSize);

            var responseCheckSum = ReceiveNext();
            if (responseCheckSum == -1)
            {
                logger.Warn(
                   STK500v2_CORRUPT_WRAPPER,
                   "Checksum not received!");
                return null;
            }
            wrappedResponseBytes[5 + messageSize] = (byte) responseCheckSum;

            byte checksum = 0;
            for (var i = 0; i < 5 + messageSize; i++) checksum ^= wrappedResponseBytes[i];

            if (responseCheckSum != checksum)
            {
                logger.Warn(
                    STK500v2_CORRUPT_WRAPPER,
                    "Checksum incorrect!"
                    );
                return null;
            }

            var message = new byte[messageSize];
            Buffer.BlockCopy(wrappedResponseBytes, 5, message, 0, messageSize);
            response.Bytes = message;
            return response;
        }

        public override void EstablishSync()
        {
            int i;
            for (i = 0; i < MaxSyncRetries; i++)
            {
                Send(new GetSyncRequest());
                var result = Receive<GetSyncResponse>();
                if (result == null) continue;
                if (!result.IsInSync) continue;
                deviceSignature = result.Signature;
                break;
            }

            if (i == MaxSyncRetries)
                UploaderLogger.LogErrorAndQuit(
                    string.Format(
                        "Unable to establish sync after {0} retries.", MaxSyncRetries));
        }

        public override void CheckDeviceSignature()
        {
            logger.Debug("Expecting to find '{0}'...", EXPECTED_DEVICE_SIGNATURE);

            if (!deviceSignature.Equals(EXPECTED_DEVICE_SIGNATURE))
                UploaderLogger.LogErrorAndQuit(
                    string.Format("Unexpected device signature - found '{0}'- expected '{1}'.",
                        deviceSignature, EXPECTED_DEVICE_SIGNATURE));
        }

        public override void InitializeDevice()
        {
            var hardwareVersion = GetParameterValue(Constants.PARAM_HW_VER);
            var softwareMajor = GetParameterValue(Constants.PARAM_SW_MAJOR);
            var softwareMinor = GetParameterValue(Constants.PARAM_SW_MINOR);
            logger.Info("Retrieved software version: {0}.",
                string.Format("{0} (hardware) - {1}.{2} (software)", 
                    hardwareVersion, softwareMajor, softwareMinor));
        }

        public override void EnableProgrammingMode()
        {
            Send(new EnableProgrammingModeRequest(MCU));
            var response = Receive<EnableProgrammingModeResponse>();
            if (response == null)
                UploaderLogger.LogErrorAndQuit(
                    "Unable to enable programming mode on the device!");
        }

        public override void LeaveProgrammingMode()
        {
            Send(new LeaveProgrammingModeRequest());
            var response = Receive<LeaveProgrammingModeResponse>();
            if (response == null)
                UploaderLogger.LogErrorAndQuit(
                    "Unable to leave programming mode on the device!");
        }

        public override void ExecuteWritePage(IMemory memory, int offset, byte[] bytes)
        {
            logger.Trace(
                "Sending execute write page request for offset {0} ({1} bytes)...", 
                offset, bytes.Length);

            var writeCmd = writeCommands[memory.Type];

            Send(new ExecuteProgramPageRequest(writeCmd, memory, bytes));
            var response = Receive<ExecuteProgramPageResponse>();
            if (response == null || response.AnswerID != writeCmd
                || response.Status != Constants.STATUS_CMD_OK)
            {
                UploaderLogger.LogErrorAndQuit(
                    string.Format(
                        "Executing write page request at offset {0} failed!", offset));
            }
        }

        public override byte[] ExecuteReadPage(IMemory memory)
        {
            var readCmd = readCommands[memory.Type];

            Send(new ExecuteReadPageRequest(readCmd, memory));
            var response = Receive<ExecuteReadPageResponse>();
            if (response == null || response.AnswerID != readCmd || response.Status != Constants.STATUS_CMD_OK)
                UploaderLogger.LogErrorAndQuit("Executing read page request failed!");

            var responseBytes = new byte[memory.PageSize];
            Buffer.BlockCopy(response.Bytes, 2, responseBytes, 0, responseBytes.Length);
            return responseBytes;
        }

        public override void LoadAddress(IMemory memory, int offset)
        {
            logger.Trace("Sending load address request: {0}.", offset);
            offset = offset >> 1;
            Send(new LoadAddressRequest(memory, offset));
            var response = Receive<LoadAddressResponse>();
            if (response == null || !response.Succeeded)
                UploaderLogger.LogErrorAndQuit(
                    "Unable to execute load address!");
        }

        private uint GetParameterValue(byte param)
        {
            logger.Trace("Retrieving parameter '{0}'...", param);
            Send(new GetParameterRequest(param));
            var response = Receive<GetParameterResponse>();
            if (response == null || !response.IsSuccess)
                UploaderLogger.LogErrorAndQuit(
                    string.Format("Retrieving parameter '{0}' failed!", param));
            return response.ParameterValue;
        }
    }
}
