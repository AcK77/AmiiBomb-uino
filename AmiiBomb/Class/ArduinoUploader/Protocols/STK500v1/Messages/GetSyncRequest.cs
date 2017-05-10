namespace ArduinoUploader.Protocols.STK500v1.Messages
{
    internal class GetSyncRequest : Request
    {
        internal GetSyncRequest()
        {
            Bytes = new[]
            {
                Constants.CMD_STK_GET_SYNC,
                Constants.SYNC_CRC_EOP
            };
        }
    }
}
