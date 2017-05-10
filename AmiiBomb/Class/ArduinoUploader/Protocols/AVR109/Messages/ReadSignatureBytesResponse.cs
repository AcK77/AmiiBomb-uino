namespace ArduinoUploader.Protocols.AVR109.Messages
{
    internal class ReadSignatureBytesResponse : Response
    {
        internal byte[] Signature
        {
            get { return new[] { Bytes[2], Bytes[1], Bytes[0] }; }
        }
    }
}
