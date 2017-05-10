namespace ArduinoUploader.Protocols.STK500v2.Messages
{
    internal class ExecuteReadPageResponse : Response
    {
        internal byte AnswerID { get { return Bytes[0]; } }
        internal byte Status { get { return Bytes[1]; } }
    }
}
