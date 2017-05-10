using System.Collections.Generic;
using ArduinoUploader.Hardware.Memory;

namespace ArduinoUploader.Hardware
{
    internal interface IMCU
    {
        byte DeviceCode { get; }
        byte DeviceRevision { get; }
        byte ProgType { get; }
        byte ParallelMode { get; }
        byte Polling { get; }
        byte SelfTimed { get; }
        byte LockBytes { get; }
        byte FuseBytes { get; }

        byte Timeout { get; }
        byte StabDelay { get; }
        byte CmdExeDelay { get; }
        byte SynchLoops { get; }
        byte ByteDelay { get; }
        byte PollValue { get; }
        byte PollIndex { get; }

        IDictionary<Command,byte[]> CommandBytes { get; }

        IMemory Flash { get; }
        IMemory EEPROM { get; }

        IList<IMemory> Memory { get; }

    }
}
