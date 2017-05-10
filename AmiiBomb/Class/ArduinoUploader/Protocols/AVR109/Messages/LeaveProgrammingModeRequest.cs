namespace ArduinoUploader.Protocols.AVR109.Messages
{
    internal class LeaveProgrammingModeRequest : Request
    {
        internal LeaveProgrammingModeRequest()
        {
            Bytes = new[]
            {
                Constants.CMD_LEAVE_PROGRAMMING_MODE
            };
        }
    }
}
