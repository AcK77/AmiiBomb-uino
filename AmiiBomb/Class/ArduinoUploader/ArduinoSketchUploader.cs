using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ArduinoUploader.BootloaderProgrammers;
using ArduinoUploader.Hardware;
using IntelHexFormatReader;
using IntelHexFormatReader.Model;
using NLog;
using RJCP.IO.Ports;

namespace ArduinoUploader
{
    public class ArduinoSketchUploader
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly ArduinoSketchUploaderOptions options;

        public ArduinoSketchUploader(ArduinoSketchUploaderOptions options)
        {
            logger.Info("Starting AmiiBombuino Flasher...");
            this.options = options;
        }

        public void UploadSketch()
        {
            var hexFileName = options.FileName;
            logger.Info("Starting upload process for file '{0}'.", hexFileName);
            var hexFileContents = File.ReadAllLines(hexFileName);
            UploadSketch(hexFileContents);
        }

        public void UploadSketch(IEnumerable<string> hexFileContents)
        {
            var serialPortName = options.PortName;

            var ports = SerialPortStream.GetPortNames();

            if (!ports.Any() || ports.Distinct().SingleOrDefault(
                x => x.Equals(serialPortName, StringComparison.OrdinalIgnoreCase)) == null)
            {
                UploaderLogger.LogErrorAndQuit(
                    string.Format("Specified COM port name '{0}' is not valid.", serialPortName));
            }

            logger.Trace("Creating serial port '{0}'...", serialPortName);
            SerialPortBootloaderProgrammer programmer = null;

            IMCU mcu = null;
            SerialPortConfig serialPortConfig;

            switch (options.ArduinoModel)
            {
                case ArduinoModel.Mega2560:
                {
                    mcu = new ATMega2560();
                    serialPortConfig = new SerialPortConfig(serialPortName, 115200);
                    programmer = new WiringBootloaderProgrammer(serialPortConfig, mcu);
                    break;
                }
                case ArduinoModel.Micro:
                {
                    mcu = new ATMega32U4();
                    serialPortConfig = new SerialPortConfig(serialPortName, 57600);
                    programmer = new ButterflyBootloaderProgrammer(serialPortConfig, mcu);
                    break;
                }
                case ArduinoModel.NanoR3:
                {
                    mcu = new ATMega328P();
                    serialPortConfig = new SerialPortConfig(serialPortName, 57600);
                    programmer = new OptibootBootloaderProgrammer(serialPortConfig, mcu);
                    break;
                }
                case ArduinoModel.UnoR3:
                {
                    mcu = new ATMega328P();
                    serialPortConfig = new SerialPortConfig(serialPortName, 115200);
                    programmer = new OptibootBootloaderProgrammer(serialPortConfig, mcu);
                    break;
                }
                default:
                {
                    UploaderLogger.LogErrorAndQuit(
                        string.Format("Unsupported model: {0}!", options.ArduinoModel));
                    break;
                }
            }

            try
            {
                programmer.Open();

                logger.Info("Establishing sync...");
                programmer.EstablishSync();
                logger.Info("Sync established.");

                logger.Info("Checking device signature...");
                programmer.CheckDeviceSignature();
                logger.Info("Device signature checked.");

                logger.Info("Initializing device...");
                programmer.InitializeDevice();
                logger.Info("Device initialized.");

                logger.Info("Enabling programming mode on the device...");
                programmer.EnableProgrammingMode();
                logger.Info("Programming mode enabled.");

                logger.Info("Programming device...");
                programmer.ProgramDevice(ReadHexFile(hexFileContents, mcu.Flash.Size));
                logger.Info("Device programmed.");

                logger.Info("Leaving programming mode...");
                programmer.LeaveProgrammingMode();
                logger.Info("Left programming mode!");
            }
            finally
            {
                programmer.Close();
            }
            logger.Info("All done, shutting down!");
        }

        #region Private Methods

        private static MemoryBlock ReadHexFile(IEnumerable<string> hexFileContents, int memorySize)
        {
            try
            {
                var reader = new HexFileReader(hexFileContents, memorySize);
                return reader.Parse();
            }
            catch (Exception ex)
            {
                UploaderLogger.LogErrorAndQuit(ex.Message);
            }
            return null;
        }

        #endregion
    }
}
