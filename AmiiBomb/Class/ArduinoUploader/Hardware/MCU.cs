using System.Collections.Generic;
using System.Linq;
using ArduinoUploader.Hardware.Memory;

namespace ArduinoUploader.Hardware
{
    internal abstract class MCU : IMCU
    {
        // TODO: move properties (both on interface and implementation to correct corresponding places)
        // At the moment this is just one giant mixin class.
        public abstract byte DeviceCode { get; }
        public abstract byte DeviceRevision { get; }
        public abstract byte LockBytes { get; }
        public abstract byte FuseBytes { get; }

        public abstract byte Timeout { get; }
        public abstract byte StabDelay { get; }
        public abstract byte CmdExeDelay { get; }
        public abstract byte SynchLoops { get; }
        public abstract byte ByteDelay { get; }
        public abstract byte PollValue { get; }
        public abstract byte PollIndex { get; }

        public virtual byte ProgType { get { return 0; } }
        public virtual byte ParallelMode { get { return 0; } }
        public virtual byte Polling { get { return 1; } }
        public virtual byte SelfTimed { get { return 1; } }

        public abstract IDictionary<Command, byte[]> CommandBytes { get; }

        public IMemory Flash
        {
            get { return Memory.SingleOrDefault(x => x.Type == MemoryType.FLASH); }
        }

        public IMemory EEPROM
        {
            get { return Memory.SingleOrDefault(x => x.Type == MemoryType.EEPROM); }
        }

        public abstract IList<IMemory> Memory { get; }
    }
}
