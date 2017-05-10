namespace ArduinoUploader.Protocols.STK500v1.Messages
{
    internal class GetSyncResponse : Response
    {
        internal bool IsInSync
        {
            get { return Bytes.Length > 0 && Bytes[0] == Constants.RESP_STK_INSYNC; }
        }
    }
}
