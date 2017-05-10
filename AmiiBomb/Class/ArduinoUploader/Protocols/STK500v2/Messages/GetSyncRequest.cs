namespace ArduinoUploader.Protocols.STK500v2.Messages
{
    internal class GetSyncRequest : Request
    {
        internal GetSyncRequest()
        {
            Bytes = new[]
            {
                Constants.CMD_SIGN_ON
            };
        }
    }
}
