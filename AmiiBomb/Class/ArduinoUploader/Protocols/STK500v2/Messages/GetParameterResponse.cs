namespace ArduinoUploader.Protocols.STK500v2.Messages
{
    internal class GetParameterResponse : Response
    {
        internal bool IsSuccess
        {
            get
            {
                return Bytes.Length > 2 && Bytes[0] == Constants.CMD_GET_PARAMETER
                       && Bytes[1] == Constants.STATUS_CMD_OK;
            }
        }

        internal byte ParameterValue
        {
            get { return Bytes[2]; }
        }
    }
}
