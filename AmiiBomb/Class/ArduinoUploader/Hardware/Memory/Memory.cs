namespace ArduinoUploader.Hardware.Memory
{
    internal abstract class Memory : IMemory
    {
        public abstract MemoryType Type { get; }

        public int Size { get; set; }
        public int PageSize { get; set; }
        public byte PollVal1 { get; set; }
        public byte PollVal2 { get; set; }
        public byte Delay { get; set; }
        public byte[] CmdBytesRead { get; set; }
        public byte[] CmdBytesWrite { get; set; }
    }
}
