namespace ArduinoUploader.Protocols.STK500v2
{
    internal static class Constants
    {
        internal const byte CMD_SIGN_ON                         = 0x01;
        internal const byte CMD_GET_PARAMETER                   = 0x03;
        internal const byte CMD_LOAD_ADDRESS                    = 0x06;
        internal const byte CMD_ENTER_PROGRMODE_ISP             = 0x10;
        internal const byte CMD_LEAVE_PROGMODE_ISP              = 0x11;
        internal const byte CMD_PROGRAM_FLASH_ISP               = 0x13;
        internal const byte CMD_READ_FLASH_ISP                  = 0x14;
        internal const byte CMD_PROGRAM_EEPROM_ISP              = 0x15;
        internal const byte CMD_READ_EEPROM_ISP                 = 0x16;

        internal const byte STATUS_CMD_OK                       = 0x00;

        internal const byte MESSAGE_START                       = 0x1b;
        internal const byte TOKEN                               = 0x0e;

        internal const byte PARAM_HW_VER                        = 0x90;
        internal const byte PARAM_SW_MAJOR                      = 0x91;
        internal const byte PARAM_SW_MINOR                      = 0x92;
    }
}
