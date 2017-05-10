using System;
using NLog;

namespace ArduinoUploader
{
    internal class UploaderLogger
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        internal static void LogErrorAndQuit(string errorMessage) 
        {
            logger.Error(errorMessage);
            throw new Exception(errorMessage);
        }
    }
}
