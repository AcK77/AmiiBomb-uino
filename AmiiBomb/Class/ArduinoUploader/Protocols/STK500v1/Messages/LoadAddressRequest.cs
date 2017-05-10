namespace ArduinoUploader.Protocols.STK500v1.Messages
{
    internal class LoadAddressRequest : Request
    {
        internal LoadAddressRequest(int address)
        {
            Bytes = new[]
            {
                Constants.CMD_STK_LOAD_ADDRESS,
                (byte)(address & 0xff),
                (byte)((address >> 8) & 0xff),
                Constants.SYNC_CRC_EOP
            };
        }
    }
}
