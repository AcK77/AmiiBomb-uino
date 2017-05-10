namespace ArduinoUploader.Protocols.AVR109.Messages
{
    internal class ExitBootLoaderRequest : Request
    {
        internal ExitBootLoaderRequest()
        {
            Bytes = new[]
            {
                Constants.CMD_EXIT_BOOTLOADER
            };
        }
    }
}
