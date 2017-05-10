using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AmiiBomb
{
    public partial class Flash_Form : Form
    {
        Arduino_Class Arduino;
        public string Current_File_Bin = "";
        public string Bin_Folder = "";
        public bool Action_Write, Com_Loaded = false;
        private I18n i18n = I18n.Instance;
        public Flash_Form()
        {
            InitializeComponent();
            this.Opacity = 0;
        }

        private void Flash_Form_Load(object sender, EventArgs e)
        {
            Translate_Class.Translate(this);

            comboBox2.SelectedIndex = 1;

            comboBox1.DataSource = COMPortInfo.GetCOMPortsInfo();
            comboBox1.DisplayMember = "Description";
            comboBox1.ValueMember = "Name";

            if (comboBox1.Items.Count > 0)
            {
                Com_Loaded = true;
                comboBox1.SelectedIndex = 0;
                comboBox1_SelectedIndexChanged(this, EventArgs.Empty);
                this.Opacity = 100;
            }
            else
            {
                MessageBox.Show(i18n.__("NFC_No_Com_Port"), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }

            if (Action_Write) button2.Text = i18n.__("NFC_Create_Amiibo_Tag");
            if (!Action_Write) checkBox1.Visible = false;
        }

        private bool NTAG_isHere()
        {
            string NTAG_Here = Arduino.SendCommand("/NTAG_HERE");
            if (NTAG_Here == "NO")
            {
                statusStrip1.Invoke(new Action(() => toolStripStatusLabel3.Text = i18n.__("NFC_No_NTAG_Present")));
                return false;
            }
            else
            {
                statusStrip1.Invoke(new Action(() => toolStripStatusLabel3.Text = i18n.__("NFC_NTAG_Present")));
                return true;
            }
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            comboBox1.Enabled = false;
            comboBox2.Enabled = false;
            checkBox1.Enabled = false;
            button2.Enabled = false;
            textBox1.Text = "";

            Arduino = new Arduino_Class(comboBox1.SelectedValue.ToString(), comboBox2.SelectedItem.ToString());
            if (Arduino.Serial.IsOpen)
            {
                toolStripStatusLabel1.Text = i18n.__("NFC_Connected");
                string DeviceNFC = i18n.__("NFC_NTAG215");
                if (!Action_Write) DeviceNFC = i18n.__("NFC_Amiibo_NTAG215");

                MessageBox.Show("Please, put your "+ DeviceNFC + " on the reader!" + Environment.NewLine + "(Or remove and put it again)", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);

                textBox1.Text = i18n.__("NFC_Wait_NTAG");
                await Task.Run(() => { while (!NTAG_isHere()){} });
                textBox1.AppendText(i18n.__("NFC_Found_NTAG") + Environment.NewLine + Environment.NewLine);

                if (Action_Write)
                {
                    string NTAG_Short_UID = Arduino.SendCommand("/GET_NTAG_UID");
                    textBox1.AppendText(i18n.__("NFC_Short_UID") + " " + NTAG_Short_UID + Environment.NewLine);

                    string NTAG_Long_UID = BitConverter.ToString(Amiibo_Class.Calculate_Long_UID(NTAG_Short_UID)).Replace("-", "");
                    textBox1.AppendText(i18n.__("NFC_Long_UID") + " " + NTAG_Long_UID + Environment.NewLine + Environment.NewLine);

                    byte[] Amiibo_Data = Amiibo_Class.Patch(File.ReadAllBytes(Current_File_Bin), NTAG_Long_UID);
                    textBox1.AppendText("\"" + Current_File_Bin + "\" " + i18n.__("NFC_Patched") + Environment.NewLine);

                    if(checkBox1.Checked) Arduino.SendCommand("/WRITE_AMIIBO 1");
                    else Arduino.SendCommand("/WRITE_AMIIBO 0");

                    string Result = Arduino.SendCommand(Amiibo_Data);
                    textBox1.AppendText("\"" + Current_File_Bin + "\" " + i18n.__("NFC_Send") + Environment.NewLine + Environment.NewLine);

                    if (Result.Split('/', ' ')[1] == "ERROR")
                        textBox1.AppendText(i18n.__("NFC_Error") + " " + Result.Substring(1) + Environment.NewLine);
                    else if (Result.Split('/', ' ')[1] == "END_WRITE")
                        textBox1.AppendText(i18n.__("NFC_Amiibo_Ready") + Environment.NewLine);
                    else
                        textBox1.AppendText(i18n.__("NFC_Unknown_Response") + " " + Result + Environment.NewLine);
                }
                else
                {
                    byte[] Amiibo_Dump = new byte[540];
                    int i;
                    for(i = 0; i < 135; i++)
                    {
                        string Result = Arduino.SendCommand("/READ_AMIIBO " + i);

                        string[] SplitResult = Result.Split('/', ' ');

                        if (SplitResult.Length > 1)
                        {
                            if (Result.Split('/', ' ')[1] == "ERROR")
                                textBox1.AppendText(i18n.__("NFC_Error") + " " + Result.Substring(1) + Environment.NewLine);
                            else
                                textBox1.AppendText(i18n.__("NFC_Unknown_Response") + " " + Result + Environment.NewLine);

                            i = 135;
                        }
                        else
                        {
                            textBox1.AppendText(i18n.__("NFC_Page_Readed", (i + 1)) + Environment.NewLine);
                            byte[] Buffer = Helper_Class.String_To_Byte_Array(Result);
                            Array.Copy(Buffer, 0, Amiibo_Dump, i * 4, Buffer.Length);
                        }
                    }

                    if (i == 135)
                    {
                        textBox1.AppendText(i18n.__("NFC_Reading_Finished") + Environment.NewLine);

                        SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                        saveFileDialog1.Filter = i18n.__("NFC_Save_Dump_Amiibo_Filter", "|*.bin");
                        saveFileDialog1.Title = i18n.__("NFC_Save_Dump_Amiibo_Title");
                        saveFileDialog1.InitialDirectory = Bin_Folder;

                        byte[] Decrypted_Amiibo = Amiibo_Class.Decrypt(Amiibo_Dump, Main_Form.AmiiKeys);
                        string[] AmiiboLife_Info = AmiiboLife_Class.Get_Amiibo_Info(Amiibo_Class.Get_NFC_ID(Decrypted_Amiibo));
                        byte[] UID = new byte[0x07];
                        Array.Copy(Amiibo_Dump, 0, UID, 0, UID.Length);

                        saveFileDialog1.FileName = ((AmiiboLife_Info[0].Trim() != "") ? AmiiboLife_Info[0].Trim().Replace(" ", "_") : BitConverter.ToString(UID).Replace("-", "")) + ".bin";

                        if (saveFileDialog1.ShowDialog(this.Parent) == DialogResult.OK && saveFileDialog1.FileName != "")
                        {
                            File.WriteAllBytes(saveFileDialog1.FileName, Amiibo_Dump);
                            MessageBox.Show(this, i18n.__("NFC_Save_Dump_Amiibo_Message"), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }

                Arduino.SendCommand("/NTAG_HALT");
                Arduino.Close();
            }
            else
            {
                toolStripStatusLabel1.Text = i18n.__("NFC_No_Connected");
                MessageBox.Show(i18n.__("NFC_No_Connection"), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            comboBox1.Enabled = true;
            //comboBox2.Enabled = true;
            checkBox1.Enabled = true;
            button2.Enabled = true;
        }

        private void Flash_Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (Arduino != null) Arduino.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Com_Loaded)
            { 
                Arduino = new Arduino_Class(comboBox1.SelectedValue.ToString(), comboBox2.SelectedItem.ToString());
                if (Arduino.SendCommand("/AMII") == "BOMB")
                {
                    toolStripStatusLabel4.Image = Properties.Resources.accept_button;
                }
                else
                {
                    toolStripStatusLabel4.Image = Properties.Resources.cancel;
                }

                Arduino.Close();
            }
        }
    }
}
