using System;
using System.Threading;
using ArduinoUploader.Hardware;
using ArduinoUploader.Protocols;
using NLog;
using RJCP.IO.Ports;

namespace ArduinoUploader.BootloaderProgrammers
{
    internal abstract class SerialPortBootloaderProgrammer : BootloaderProgrammer
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        protected SerialPortConfig serialPortConfig;
        protected SerialPortStream SerialPort { get; set; }

        protected SerialPortBootloaderProgrammer(SerialPortConfig serialPortConfig, IMCU mcu)
            : base(mcu)
        {
            this.serialPortConfig = serialPortConfig;
        }

        public override void Open()
        {
            var portName = serialPortConfig.PortName;
            var baudRate = serialPortConfig.BaudRate;
            logger.Info("Opening serial port {0} - baudrate {1}", serialPortConfig.PortName, serialPortConfig.BaudRate);

            SerialPort = new SerialPortStream(portName, baudRate)
            {
                ReadTimeout = serialPortConfig.ReadTimeOut,
                WriteTimeout = serialPortConfig.WriteTimeOut,
                DtrEnable = true // This means the Arduino will reset the moment we open the serial connection.
            };
            try
            {
                SerialPort.Open();
            }
            catch (ObjectDisposedException ex)
            {
                UploaderLogger.LogErrorAndQuit(
                    string.Format("Unable to open serial port {0} - {1}.", portName, ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                UploaderLogger.LogErrorAndQuit(
                    string.Format("Unable to open serial port {0} - {1}.", portName, ex.Message));
            }
            logger.Trace("Opened serial port {0} with baud rate {1}!", portName, baudRate);
        }

        public override void Close()
        {
            logger.Info("Closing serial port...");
            SerialPort.DtrEnable = false;
            SerialPort.RtsEnable = false;
            try
            {
                SerialPort.Close();
            }
            catch (Exception)
            {
                // Ignore
            }
        }

        protected void ToggleDtrRts(int wait1, int wait2, bool invert = false)
        {
            logger.Trace("Toggling DTR/RTS...");

            SerialPort.DtrEnable = invert;
            SerialPort.RtsEnable = invert;

            Thread.Sleep(wait1);

            SerialPort.DtrEnable = !invert;
            SerialPort.RtsEnable = !invert;

            Thread.Sleep(wait2);    
        }

        protected virtual void Send(IRequest request)
        {
            var bytes = request.Bytes;
            var length = bytes.Length;
            logger.Trace(
                "Sending {0} bytes: {1}{2}", 
                length, Environment.NewLine, BitConverter.ToString(bytes));
            SerialPort.Write(bytes, 0, length);
        }

        protected TResponse Receive<TResponse>(int length = 1) where TResponse : Response, new()
        {
            var bytes = ReceiveNext(length);
            if (bytes == null) return null;
            return new TResponse { Bytes = bytes };
        }

        protected int ReceiveNext()
        {
            var bytes = new byte[1];
            try
            {
                SerialPort.Read(bytes, 0, 1);
                logger.Trace(
                    "Receiving byte: {0}",
                    BitConverter.ToString(bytes));
                return bytes[0];
            }
            catch (TimeoutException)
            {
                return -1;
            }
        }

        protected byte[] ReceiveNext(int length)
        {
            var bytes = new byte[length];
            var retrieved = 0;
            try
            {
                while (retrieved < length)
                    retrieved += SerialPort.Read(bytes, retrieved, length - retrieved);

                logger.Trace("Receiving bytes: {0}", BitConverter.ToString(bytes));
                return bytes;
            }
            catch (TimeoutException)
            {
                return null;
            }            
        }
    }
}
