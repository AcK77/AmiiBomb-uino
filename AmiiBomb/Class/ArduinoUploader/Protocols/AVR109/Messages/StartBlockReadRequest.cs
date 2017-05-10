using ArduinoUploader.Hardware.Memory;

namespace ArduinoUploader.Protocols.AVR109.Messages
{
    internal class StartBlockReadRequest : Request
    {
        internal StartBlockReadRequest(MemoryType memType, int blockSize)
        {
            Bytes = new[]
            {
                Constants.CMD_START_BLOCK_READ,
                (byte)(blockSize >> 8),
                (byte)(blockSize & 0xff),
                (byte)(memType == MemoryType.FLASH ? 'F' : 'E')
            };
        }
    }
}
