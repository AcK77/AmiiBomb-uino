namespace ArduinoUploader.Protocols.STK500v2.Messages
{
    internal class LoadAddressResponse : Response
    {
        internal bool Succeeded
        {
            get
            {
                return Bytes.Length == 2
                       && Bytes[0] == Constants.CMD_LOAD_ADDRESS
                       && Bytes[1] == Constants.STATUS_CMD_OK;
            }
        }
    }
}
