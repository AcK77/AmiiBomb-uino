using ArduinoUploader.Hardware;

namespace ArduinoUploader.BootloaderProgrammers
{
    internal abstract class ArduinoBootloaderProgrammer : SerialPortBootloaderProgrammer
    {
        protected int MaxSyncRetries { get { return 20; } }

        protected abstract void Reset();

        protected ArduinoBootloaderProgrammer(SerialPortConfig serialPortConfig, IMCU mcu)
            : base(serialPortConfig, mcu)
        {
        }

        public override void Open()
        {
            base.Open();
            Reset();
        }

        public override void Close()
        {
            Reset();
            base.Close();
        }
    }
}
