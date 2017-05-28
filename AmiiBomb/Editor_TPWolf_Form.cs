using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace AmiiBomb
{
    public partial class Editor_TPWolf_Form : Form
    {
        public string Current_File_Bin = "";
        byte[] AppData;
        private I18n i18n = I18n.Instance;

        public Editor_TPWolf_Form()
        {
            InitializeComponent();
        }

        private void Editor_TPWolf_Form_Load(object sender, EventArgs e)
        {
            Translate_Class.Translate(this);

            AppData = Amiibo_Class.Dump_AppData(File.ReadAllBytes(Current_File_Bin));

            if (AppData[0x11] < 1 || AppData[0x11] > 40) AppData[0x11] = 1;
            if (AppData[0x12] < 1 || AppData[0x12] > 80) AppData[0x12] = 0;

            trackBar1.Value = AppData[0x11]; //Level
            trackBar2.Value = AppData[0x12]; //Heart
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            label3.Text = trackBar1.Value.ToString();

            if (trackBar1.Value == 5) label3.Text += " " + i18n.__("TP_Editor_Run1"); 
            if (trackBar1.Value == 20) label3.Text += " " + i18n.__("TP_Editor_Run2"); 
            if (trackBar1.Value == 40) label3.Text += " " + i18n.__("TP_Editor_RunAll");
        }

        private void trackBar2_ValueChanged(object sender, EventArgs e)
        {
            float Health = ((float)(((float)trackBar2.Value * 25) / 100));
            label4.Text = Health.ToString("R");

            panel1.Controls.Add(Generate_Health(trackBar2.Value));
            if(panel1.Controls.Count > 1) panel1.Controls.RemoveAt(0);
        }

        private Panel Generate_Health(int Health_Val)
        {
            Panel Health_Panel = new Panel();

            float fHealth = ((float)(((float)Health_Val * 25) / 100));

            if ((int)Math.Ceiling(fHealth) > 3) label5.Visible = false;
            else label5.Visible = true;

            for (int i = 1; i <= (int)Math.Ceiling(fHealth) - 1; i++)
            {
                int Location_Y = 0;
                if (i > 10) Location_Y = 18;

                int Location_X = 0;
                if (i <= 10) Location_X = (18 * i);
                else if (i >= 11) Location_X = 18 * (i - 10);

                var Picture = new PictureBox
                {
                    Size = new Size(16, 14),
                    Location = new Point(Location_X, Location_Y),
                    Image = Properties.Resources.Heart_Full
                };

                Health_Panel.Controls.Add(Picture);
            }

            if (Health_Val != 0)
            { 
                int i = (int)Math.Ceiling(fHealth);
                int Last_Location_Y = 0;
                if (i > 10) Last_Location_Y = 18;

                int Last_Location_X = 0;
                if (i <= 10) Last_Location_X = (18 * i);
                else if (i >= 11) Last_Location_X = 18 * (i - 10);

                var Last_Picture = new PictureBox
                {
                    Size = new Size(16, 14),
                    Location = new Point(Last_Location_X, Last_Location_Y)
                };

                switch ((fHealth - Math.Truncate(fHealth)).ToString())
                {
                    case "0":
                        Last_Picture.Image = Properties.Resources.Heart_Full;
                        break;

                    case "0,25":
                        Last_Picture.Image = Properties.Resources.Heart_0_25;
                        break;

                    case "0,5":
                        Last_Picture.Image = Properties.Resources.Heart_0_5;
                        break;

                    case "0,75":
                        Last_Picture.Image = Properties.Resources.Heart_0_75;
                        break;
                }

                Health_Panel.Controls.Add(Last_Picture);
            }

            return Health_Panel;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SaveFile(Current_File_Bin);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = i18n.__("TP_Editor_Filter", "|*.bin");
            saveFileDialog1.Title = i18n.__("TP_Editor_Title");
            saveFileDialog1.FileName = Path.GetFileNameWithoutExtension(Current_File_Bin);

            if (saveFileDialog1.ShowDialog(this) == DialogResult.OK && saveFileDialog1.FileName != "")
            {
                SaveFile(saveFileDialog1.FileName);
            }
        }

        private void SaveFile(string FilePath)
        {
            AppData[0x11] = (byte)trackBar1.Value; //Level
            AppData[0x12] = (byte)trackBar2.Value; //Heart

            byte[] AppData_Patched_File = Amiibo_Class.WriteAppData(File.ReadAllBytes(Current_File_Bin), AppData);
            File.WriteAllBytes(FilePath, AppData_Patched_File);
            MessageBox.Show(this, i18n.__("TP_Editor_SaveMessage", Path.GetFileNameWithoutExtension(FilePath)), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }
    }
}
