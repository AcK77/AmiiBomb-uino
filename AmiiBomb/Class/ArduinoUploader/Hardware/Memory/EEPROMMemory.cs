namespace ArduinoUploader.Hardware.Memory
{
    internal class EEPROMMemory : Memory
    {
        public override MemoryType Type
        {
            get { return MemoryType.EEPROM; }
        }
    }
}
