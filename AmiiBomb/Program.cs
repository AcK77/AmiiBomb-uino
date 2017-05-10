using System;
using System.IO;
using System.Windows.Forms;

namespace AmiiBomb
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            try
            {
                Check_AmiiBomb_Files();
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Main_Form());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Application.Exit();
            }
        }

        private static void Check_AmiiBomb_Files()
        {
            if (!Directory.Exists("cache")) throw new Exception("\"cache\" folder is missing!");
            if (!Directory.Exists("lang")) throw new Exception("\"lang\" folder is missing!");
            if (!Directory.Exists("lib")) throw new Exception("\"lib\" folder is missing!");

            if (!Directory.Exists("lib\\ArduinoHex")) throw new Exception("\"lib\\ArduinoHex\" folder is missing!");

            if (!File.Exists(@"lib\ArduinoHex\AmiiBombuino.Mega.hex")) throw new Exception("\"lib\\ArduinoHex\\AmiiBombuino.Mega.hex\" file is missing!");
            if (!File.Exists(@"lib\ArduinoHex\AmiiBombuino.Micro.hex")) throw new Exception("\"lib\\ArduinoHex\\AmiiBombuino.Micro.hex\" file is missing!");
            if (!File.Exists(@"lib\ArduinoHex\AmiiBombuino.Nano.hex")) throw new Exception("\"lib\\ArduinoHex\\AmiiBombuino.Nano.hex\" file is missing!");
            if (!File.Exists(@"lib\ArduinoHex\AmiiBombuino.Uno.hex")) throw new Exception("\"lib\\ArduinoHex\\AmiiBombuino.Uno.hex\" file is missing!");

            if (!File.Exists(@"lib\AmiiBomb.conf")) throw new Exception("\"lib\\AmiiBomb.conf\" file is missing!");
            if (!File.Exists(@"lib\AngleSharp.dll")) throw new Exception("\"lib\\AngleSharp.dll\" file is missing!");
            if (!File.Exists(@"lib\Arduino-RC522.png")) throw new Exception("\"lib\\Arduino-RC522.png\" file is missing!");
            if (!File.Exists(@"lib\BouncyCastle.Crypto.dll")) throw new Exception("\"lib\\BouncyCastle.Crypto.dll\" file is missing!");
            if (!File.Exists(@"lib\IntelHexFormatReader.dll")) throw new Exception("\"lib\\IntelHexFormatReader.dll\" file is missing!");
            if (!File.Exists(@"lib\Newtonsoft.Json.dll")) throw new Exception("\"lib\\Newtonsoft.Json.dll\" file is missing!");
            if (!File.Exists(@"lib\NLog.dll")) throw new Exception("\"lib\\NLog.dll\" file missing!");
            if (!File.Exists(@"lib\RJCP.SerialPortStream.dll")) throw new Exception("\"lib\\RJCP.SerialPortStream.dll\" file is missing!");
        }
    }
}
