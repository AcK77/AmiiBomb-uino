namespace ArduinoUploader.Protocols.AVR109.Messages
{
    internal class ReturnSoftwareVersionResponse : Response
    {
        internal char MajorVersion { get { return (char) Bytes[0]; } }
        internal char MinorVersion { get { return (char) Bytes[1]; } }
    }
}
