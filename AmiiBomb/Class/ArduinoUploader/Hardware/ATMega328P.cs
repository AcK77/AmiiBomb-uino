using System.Collections.Generic;
using ArduinoUploader.Hardware.Memory;

namespace ArduinoUploader.Hardware
{
    internal class ATMega328P : MCU
    {
        public override byte DeviceCode { get { return 0x86; } }
        public override byte DeviceRevision { get { return 0; } }
        public override byte ProgType { get { return 0; } }
        public override byte ParallelMode { get { return 1; } }
        public override byte Polling { get { return 1; } }
        public override byte SelfTimed { get { return 1; } }
        public override byte LockBytes { get { return 1; } }
        public override byte FuseBytes { get { return 3; } }

        public override byte Timeout { get { return 200; } }
        public override byte StabDelay { get { return 100; } }
        public override byte CmdExeDelay { get { return 25; } }
        public override byte SynchLoops { get { return 32;  } }
        public override byte ByteDelay { get { return 0; } }
        public override byte PollIndex { get { return 3; } }
        public override byte PollValue { get { return 0x53; } }

        public override IDictionary<Command, byte[]> CommandBytes
        {
            get { return new Dictionary<Command, byte[]>(); }
        }

        public override IList<IMemory> Memory
        {
            get
            {
                return new List<IMemory>()
                {
                    new FlashMemory()
                    {
                        Size = 32 * 1024,
                        PageSize = 128,
                        PollVal1 = 0xff,
                        PollVal2 = 0xff
                    },
                    new EEPROMMemory()
                    {
                        Size = 1024,
                        PollVal1 = 0xff,
                        PollVal2 = 0xff
                    }
                };
            }
        }
    }
}
