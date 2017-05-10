namespace ArduinoUploader.Protocols.AVR109.Messages
{
    internal class ReturnSupportedDeviceCodesRequest : Request
    {
        internal ReturnSupportedDeviceCodesRequest()
        {
            Bytes = new[]
            {
                Constants.CMD_RETURN_SUPPORTED_DEVICE_CODES
            };
        }
    }
}
