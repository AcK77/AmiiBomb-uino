using System;
using ArduinoUploader.Hardware.Memory;

namespace ArduinoUploader.Protocols.AVR109.Messages
{
    internal class StartBlockLoadRequest : Request
    {
        internal StartBlockLoadRequest(MemoryType memType, int blockSize, byte[] bytes)
        {
            Bytes = new byte[blockSize + 4];
            Bytes[0] = Constants.CMD_START_BLOCK_LOAD;
            Bytes[1] = (byte) (blockSize >> 8);
            Bytes[2] = (byte) (blockSize & 0xff);
            Bytes[3] = (byte) (memType == MemoryType.FLASH ? 'F' : 'E');
            Buffer.BlockCopy(bytes, 0, Bytes, 4, blockSize);
        }
    }
}
