namespace ArduinoUploader.Hardware.Memory
{
    internal class FlashMemory : Memory
    {
        public override MemoryType Type
        {
            get { return MemoryType.FLASH; }
        }
    }
}
