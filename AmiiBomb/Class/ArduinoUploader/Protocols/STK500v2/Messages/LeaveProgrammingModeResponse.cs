namespace ArduinoUploader.Protocols.STK500v2.Messages
{
    internal class LeaveProgrammingModeResponse : Response
    {
        internal bool Success
        {
            get
            {

                return Bytes.Length == 2
                       && Bytes[0] == Constants.CMD_LEAVE_PROGMODE_ISP
                       && Bytes[1] == Constants.STATUS_CMD_OK;
            }
        }
    }
}
