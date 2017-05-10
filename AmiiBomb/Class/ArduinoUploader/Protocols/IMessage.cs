namespace ArduinoUploader.Protocols
{
    internal interface IMessage
    {
        byte[] Bytes { get; set; }
    }
}
