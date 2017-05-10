namespace ArduinoUploader.Protocols.AVR109.Messages
{
    internal class ReturnSoftwareIdentifierRequest : Request
    {
        internal ReturnSoftwareIdentifierRequest()
        {
            Bytes = new[]
            {
                Constants.CMD_RETURN_SOFTWARE_IDENTIFIER
            };
        }
    }
}
