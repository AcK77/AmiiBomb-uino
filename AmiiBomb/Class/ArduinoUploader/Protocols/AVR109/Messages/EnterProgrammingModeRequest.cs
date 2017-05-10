namespace ArduinoUploader.Protocols.AVR109.Messages
{
    internal class EnterProgrammingModeRequest : Request
    {
        internal EnterProgrammingModeRequest()
        {
            Bytes = new[]
            {
                Constants.CMD_ENTER_PROGRAMMING_MODE
            };
        }
    }
}
