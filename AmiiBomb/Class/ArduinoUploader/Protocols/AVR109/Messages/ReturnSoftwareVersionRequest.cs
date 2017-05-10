namespace ArduinoUploader.Protocols.AVR109.Messages
{
    internal class ReturnSoftwareVersionRequest : Request
    {
        internal ReturnSoftwareVersionRequest()
        {
            Bytes = new[]
            {
                Constants.CMD_RETURN_SOFTWARE_VERSION
            };
        }
    }
}
