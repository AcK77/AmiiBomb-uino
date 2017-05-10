using System.Collections.Generic;
using System.Linq;
using ArduinoUploader.Hardware.Memory;

namespace ArduinoUploader.Protocols.STK500v2.Messages
{
    internal class ExecuteProgramPageRequest : Request
    {
        internal ExecuteProgramPageRequest(byte writeCmd, IMemory memory, IReadOnlyCollection<byte> data)
        {
            var len = data.Count;
            const byte mode = 0xc1;
            var headerBytes = new[]
            {
                writeCmd,
                (byte) (len >> 8),
                (byte) (len & 0xff),
                mode,
                memory.Delay,
                memory.CmdBytesWrite[0],
                memory.CmdBytesWrite[1],
                memory.CmdBytesWrite[2],
                memory.PollVal1,
                memory.PollVal2
            };
            Bytes = headerBytes.Concat(data).ToArray();
        }
    }
}
