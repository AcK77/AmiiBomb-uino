namespace ArduinoUploader.Protocols.AVR109.Messages
{
    internal class CheckBlockSupportResponse : Response
    {
        internal bool HasBlockSupport { get { return Bytes[0] == (byte) 'Y'; } }
        internal int BufferSize { get { return (Bytes[1] << 8) + Bytes[2]; } }
    }
}
