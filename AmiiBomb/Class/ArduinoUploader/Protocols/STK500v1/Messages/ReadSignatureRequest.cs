namespace ArduinoUploader.Protocols.STK500v1.Messages
{
    internal class ReadSignatureRequest : Request
    {
        internal ReadSignatureRequest()
        {
            Bytes = new[]
            {
                Constants.CMD_STK_READ_SIGNATURE,
                Constants.SYNC_CRC_EOP
            };
        }
    }
}
