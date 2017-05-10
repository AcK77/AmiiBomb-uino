namespace ArduinoUploader.Protocols.AVR109.Messages
{
    internal class SelectDeviceTypeRequest : Request
    {
        internal SelectDeviceTypeRequest(byte deviceCode)
        {
            Bytes = new[]
            {
                Constants.CMD_SELECT_DEVICE_TYPE,
                deviceCode
            };
        }
    }
}
