namespace ArduinoUploader.Protocols.AVR109.Messages
{
    internal class CheckBlockSupportRequest : Request
    {
        internal CheckBlockSupportRequest()
        {
            Bytes = new[]
            {
                Constants.CMD_CHECK_BLOCK_SUPPORT
            };
        }
    }
}
