using System;
using System.Linq;
using System.Windows.Forms;
using ArduinoUploader;
using ArduinoUploader.Hardware;
using NLog.Config;
using NLog;
using System.Threading;

namespace AmiiBomb
{
    public partial class Arduino_Form : Form
    {
        Thread Flash_Thread;
        private I18n i18n = I18n.Instance;
        public Arduino_Form()
        {
            InitializeComponent();
            this.Opacity = 0;
        }

        private void Arduino_Form_Load(object sender, EventArgs e)
        {
            Translate_Class.Translate(this);

            comboBox1.DataSource = COMPortInfo.GetCOMPortsInfo();
            comboBox1.DisplayMember = "Description";
            comboBox1.ValueMember = "Name";

            if (comboBox1.Items.Count > 0)
            {
                comboBox1.SelectedIndex = 0;
                comboBox2.SelectedIndex = 3;
                this.Opacity = 100;
            }
            else
            {
                MessageBox.Show(i18n.__("AmiiBombuino_No_Com_Port"), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var config = new LoggingConfiguration();
            var consoleTarget = new TextBoxTarget();
            consoleTarget.TextBox_Console = textBox1;
            consoleTarget.Layout = "${message}";
            config.AddTarget("console", consoleTarget);
            config.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, consoleTarget));
            LogManager.Configuration = config;

            string Port = comboBox1.SelectedValue.ToString();
            string ArduinoFile = comboBox2.SelectedItem.ToString().Split(' ').First();
            int ArduinoID = comboBox2.SelectedIndex;

            Flash_Thread = new Thread(() => Flash(Port, ArduinoFile, ArduinoID));
            Flash_Thread.Start();

        }

        private void Flash(string Port, string ArduinoFile, int ArduinoID)
        {
            bool AllDone = false;

            try
            {
                var options = new ArduinoSketchUploaderOptions
                {
                    PortName = Port,
                    FileName = Application.StartupPath + @"\lib\ArduinoHex\AmiiBombuino." + ArduinoFile + ".hex",
                    ArduinoModel = (ArduinoModel)ArduinoID
                };

                var uploader = new ArduinoSketchUploader(options);
                uploader.UploadSketch();
                AllDone = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (AllDone)
                {
                    MessageBox.Show(i18n.__("AmiiBombuino_All_Done"), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Invoke(new Action(() => this.Close()));
                }
            }
        }

        private void Arduino_Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (Flash_Thread != null) Flash_Thread.Abort();
        }
    }
}
