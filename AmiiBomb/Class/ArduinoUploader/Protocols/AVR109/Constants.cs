namespace ArduinoUploader.Protocols.AVR109
{
    internal static class Constants
    {
        internal const byte NULL                                = 0x00;
        internal const byte CARRIAGE_RETURN                     = 0x0d;

        internal const byte CMD_SET_ADDRESS                     = 0x41;
        internal const byte CMD_START_BLOCK_LOAD                = 0x42;
        internal const byte CMD_EXIT_BOOTLOADER                 = 0x45;
        internal const byte CMD_LEAVE_PROGRAMMING_MODE          = 0x4c;
        internal const byte CMD_ENTER_PROGRAMMING_MODE          = 0x50;
        internal const byte CMD_RETURN_SOFTWARE_IDENTIFIER      = 0x53;
        internal const byte CMD_SELECT_DEVICE_TYPE              = 0x54;
        internal const byte CMD_RETURN_SOFTWARE_VERSION         = 0x56;
        internal const byte CMD_CHECK_BLOCK_SUPPORT             = 0x62;
        internal const byte CMD_START_BLOCK_READ                = 0x67;
        internal const byte CMD_RETURN_PROGRAMMER_TYPE          = 0x70;
        internal const byte CMD_READ_SIGNATURE_BYTES            = 0x73;
        internal const byte CMD_RETURN_SUPPORTED_DEVICE_CODES   = 0x74;
    }
}
