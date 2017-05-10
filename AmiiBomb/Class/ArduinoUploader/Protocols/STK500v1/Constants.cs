namespace ArduinoUploader.Protocols.STK500v1
{
    internal static class Constants
    {
        internal const byte CMD_STK_GET_SYNC                    = 0x30;
        internal const byte CMD_STK_GET_PARAMETER               = 0x41;
        internal const byte CMD_STK_SET_DEVICE                  = 0x42;
        internal const byte CMD_STK_ENTER_PROGMODE              = 0x50;
        internal const byte CMD_STK_LEAVE_PROGMODE              = 0x51;
        internal const byte CMD_STK_LOAD_ADDRESS                = 0x55;
        internal const byte CMD_STK_PROG_PAGE                   = 0x64;
        internal const byte CMD_STK_READ_PAGE                   = 0x74;
        internal const byte CMD_STK_READ_SIGNATURE              = 0x75;

        internal const byte SYNC_CRC_EOP                        = 0x20;

        internal const byte RESP_STK_OK                         = 0x10;
        internal const byte RESP_STK_Failed                     = 0x11;
        internal const byte RESP_STK_NODEVICE                   = 0x13;
        internal const byte RESP_STK_INSYNC                     = 0x14;
        internal const byte RESP_STK_NOSYNC                     = 0x15;

        internal const byte PARM_STK_SW_MAJOR                   = 0x81;
        internal const byte PARM_STK_SW_MINOR                   = 0x82;
    }
}
