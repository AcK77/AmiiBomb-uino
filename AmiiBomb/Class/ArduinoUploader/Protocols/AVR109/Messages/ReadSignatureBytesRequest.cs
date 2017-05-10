namespace ArduinoUploader.Protocols.AVR109.Messages
{
    internal class ReadSignatureBytesRequest : Request
    {
        internal ReadSignatureBytesRequest()
        {
            Bytes = new[]
            {
                Constants.CMD_READ_SIGNATURE_BYTES
            };
        }
    }
}
