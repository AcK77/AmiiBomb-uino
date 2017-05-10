namespace ArduinoUploader.Protocols.AVR109.Messages
{
    internal class ReturnProgrammerTypeRequest : Request
    {
        internal ReturnProgrammerTypeRequest()
        {
            Bytes = new[]
            {
                Constants.CMD_RETURN_PROGRAMMER_TYPE
            };
        }
    }
}
