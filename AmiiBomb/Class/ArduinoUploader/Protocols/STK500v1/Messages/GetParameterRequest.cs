namespace ArduinoUploader.Protocols.STK500v1.Messages
{
    internal class GetParameterRequest : Request
    {
        internal GetParameterRequest(byte param)
        {
            Bytes = new[]
            {
                Constants.CMD_STK_GET_PARAMETER,
                param,
                Constants.SYNC_CRC_EOP
            };
        }
    }
}
