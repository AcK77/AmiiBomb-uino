namespace ArduinoUploader.Protocols.STK500v2.Messages
{
    internal class LeaveProgrammingModeRequest : Request
    {
        internal LeaveProgrammingModeRequest()
        {
            Bytes = new[]
            {
                Constants.CMD_LEAVE_PROGMODE_ISP,
                (byte) 0x01,
                (byte) 0x01
            };
        }
    }
}
