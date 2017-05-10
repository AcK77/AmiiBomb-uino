namespace ArduinoUploader.Protocols.AVR109.Messages
{
    internal class ReturnProgrammerTypeResponse : Response
    {
        internal char ProgrammerType { get { return (char) Bytes[0]; } }
    }
}
