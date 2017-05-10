using System.Linq;
using System.Threading;
using ArduinoUploader.Hardware;
using ArduinoUploader.Hardware.Memory;
using IntelHexFormatReader.Model;
using NLog;

namespace ArduinoUploader.BootloaderProgrammers
{
    internal abstract class BootloaderProgrammer : IBootloaderProgrammer
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        protected IMCU MCU { get; private set; }

        protected BootloaderProgrammer(IMCU mcu)
        {
            MCU = mcu;
        }

        public abstract void Open();
        public abstract void Close();
        public abstract void EstablishSync();
        public abstract void CheckDeviceSignature();
        public abstract void InitializeDevice();
        public abstract void EnableProgrammingMode();
        public abstract void LeaveProgrammingMode();
        public abstract void LoadAddress(IMemory memory, int offset);
        public abstract void ExecuteWritePage(IMemory memory, int offset, byte[] bytes);
        public abstract byte[] ExecuteReadPage(IMemory memory);

        public virtual void ProgramDevice(MemoryBlock memoryBlock)
        {
            var sizeToWrite = memoryBlock.HighestModifiedOffset + 1;
            var flashMem = MCU.Flash;
            var pageSize = flashMem.PageSize;
            logger.Info("Preparing to write {0} bytes...", sizeToWrite);
            logger.Info("Flash page size: {0}.", pageSize);

            int offset;
            for (offset = 0; offset < sizeToWrite; offset += pageSize)
            {
                var needsWrite = false;
                for (var i = offset; i < offset + pageSize; i++)
                {
                    if (!memoryBlock.Cells[i].Modified) continue;
                    needsWrite = true;
                    break;
                }
                if (needsWrite)
                {
                    logger.Debug("Executing paged write @ address {0} (page size {1})...", offset, pageSize);
                    var bytesToCopy = memoryBlock.Cells.Skip(offset).Take(pageSize).Select(x => x.Value).ToArray();

                    logger.Trace("Checking if bytes at offset {0} need to be overwritten...", offset);
                    LoadAddress(flashMem, offset);
                    var bytesAlreadyPresent = ExecuteReadPage(flashMem);
                    if (bytesAlreadyPresent.SequenceEqual(bytesToCopy))
                    {
                        logger.Trace("Bytes to be written are identical to bytes already present - skipping actual write!");
                        continue;
                    }
                    logger.Trace("Writing page at offset {0}.", offset);
                    LoadAddress(flashMem, offset);
                    ExecuteWritePage(flashMem, offset, bytesToCopy);

                    logger.Trace("Page written, now verifying...");
                    Thread.Sleep(10);
                    LoadAddress(flashMem, offset);
                    var verify = ExecuteReadPage(flashMem);
                    var succeeded = verify.SequenceEqual(bytesToCopy);
                    if (!succeeded)
                        UploaderLogger.LogErrorAndQuit(
                            "Difference encountered during verification, write failed!");
                }
                else
                {
                    logger.Trace("Skip writing page...");
                }
            }
            logger.Info("{0} bytes written to flash memory!", sizeToWrite);
        }
    }
}
