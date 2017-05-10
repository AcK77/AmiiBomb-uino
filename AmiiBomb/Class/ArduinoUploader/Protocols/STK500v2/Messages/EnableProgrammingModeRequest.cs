using ArduinoUploader.Hardware;

namespace ArduinoUploader.Protocols.STK500v2.Messages
{
    internal class EnableProgrammingModeRequest : Request
    {
        internal EnableProgrammingModeRequest(IMCU mcu)
        {
            var cmdBytes = mcu.CommandBytes[Command.PGM_ENABLE];
            Bytes = new[]
            {
                Constants.CMD_ENTER_PROGRMODE_ISP,
                mcu.Timeout,
                mcu.StabDelay,
                mcu.CmdExeDelay,
                mcu.SynchLoops,
                mcu.ByteDelay,
                mcu.PollValue,
                mcu.PollIndex,
                cmdBytes[0],
                cmdBytes[1],
                cmdBytes[2],
                cmdBytes[3]
            };
        }
    }
}
