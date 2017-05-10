using System.Collections.Generic;
using ArduinoUploader.Hardware.Memory;

namespace ArduinoUploader.Hardware
{
    internal class ATMega2560 : MCU
    {
        public override byte DeviceCode { get { return 0xb2; } }
        public override byte DeviceRevision { get { return 0; } }
        public override byte LockBytes { get { return 1; } }
        public override byte FuseBytes { get { return 3; } }

        public override byte Timeout { get { return 200; } }
        public override byte StabDelay { get { return 100; } }
        public override byte CmdExeDelay { get { return 25; } }
        public override byte SynchLoops { get { return 32; } }
        public override byte ByteDelay { get { return 0; } }
        public override byte PollIndex { get { return 3; } }
        public override byte PollValue { get { return 0x53; } }

        public override IDictionary<Command, byte[]> CommandBytes
        {
            get
            {
                return new Dictionary<Command, byte[]>
                {
                    { Command.PGM_ENABLE, new byte[] { 0xac, 0x53, 0x00, 0x00 } }
                };
            }
        }

        public override IList<IMemory> Memory
        {
            get 
            { 
                return new List<IMemory>()
                {
                    new FlashMemory()
                    {
                        Size = 256 * 1024,
                        PageSize = 256,
                        PollVal1 = 0x00,
                        PollVal2 = 0x00,
                        Delay = 10,
                        CmdBytesRead = new byte[] { 0x20, 0x00, 0x00 },
                        CmdBytesWrite = new byte[] { 0x40, 0x4c, 0x00 }
                    },
                    new EEPROMMemory()
                    {
                        Size = 4 * 1024,
                        PollVal1 = 0x00,
                        PollVal2 = 0x00,
                        Delay = 10,
                        CmdBytesRead = new byte[] { 0xa0, 0x00, 0x00 },
                        CmdBytesWrite = new byte[] { 0xc1, 0xc2, 0x00 }
                    }
                }; 
            }
        }
    }
}
