using System;
using System.Diagnostics;
using System.IO.Ports;

namespace AmiiBomb
{
    public class Arduino_Class
    {
        public SerialPort Serial;
        bool Reading_Ended = false;
        string Result = "";
        private I18n i18n = I18n.Instance;

        public Arduino_Class(string COM_Port, string BaudRate)
        {
            //http://zachsaw.blogspot.fr/2010/07/serialport-ioexception-workaround-in-c.html
            

            Serial = new SerialPort(COM_Port, int.Parse(BaudRate), Parity.None, 8, StopBits.One);

            if (!Serial.IsOpen)
            {
                Serial.DataReceived += new SerialDataReceivedEventHandler(Arduino_DataReceived);

                try
                {
                    Serial.Open();
                }
                catch (Exception) {}
            }
            else throw new Exception(i18n.__("NFC_No_Com_Port"));
        }

        public string SendCommand(object Message)
        {
            Reading_Ended = false;

            if (Serial.IsOpen)
            {
                if (Message is string) Serial.Write((string)Message + "\n");
                else if (Message is byte[]) Serial.Write((byte[])Message, 0, ((byte[])Message).Length);
            }

            var sw = Stopwatch.StartNew();
            while (!Reading_Ended)
            {
                if (sw.ElapsedMilliseconds > 3000)
                {
                    sw.Stop();
                    Result = "TIMEOUT";
                    Reading_Ended = true;
                }
            }

            return Result;
        }

        void Arduino_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Result = Serial.ReadTo("\x03").Substring(1);
            
            Console.WriteLine(i18n.__("NFC_Serial_Response") + Result);

            Reading_Ended = true;
        }

        public void Close()
        {
            Serial.Close();
        }
    }
}
