using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AmiiBomb
{
    public partial class PowerSaves_Form : Form
    {
        public PowerSaves_Form()
        {
            InitializeComponent();
            this.Opacity = 0;
        }

        HIDDevice USBHID_Device;
        string USBHID_DevicePath = "";
        public string Bin_Folder = "";
        Thread thRead_Tag;
        private I18n i18n = I18n.Instance;

        private void PowerSaves_Form_Load(object sender, EventArgs e)
        {
            Translate_Class.Translate(this);

            foreach (HIDDevice.interfaceDetails USB_Device in HIDDevice.getConnectedDevices())
            {
                if(USB_Device.product.Trim() == "NFC-Portal" && USB_Device.manufacturer.Trim() == "Datel")
                {
                    USBHID_DevicePath = USB_Device.devicePath;
                    label3.Text = USB_Device.product + " of " + USB_Device.manufacturer;
                    textBox1.Text = USB_Device.serialNumber;
                    break;
                }
            }

            if(USBHID_DevicePath == "")
            {
                MessageBox.Show(i18n.__("PowerSaves_No_PowerSaves"), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else this.Opacity = 100;
        }

        private void PowerSaves_Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (thRead_Tag != null && thRead_Tag.IsAlive) thRead_Tag.Abort();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;

            MessageBox.Show(i18n.__("PowerSaves_Put_Tag"), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);

            if (thRead_Tag != null && thRead_Tag.IsAlive) thRead_Tag.Abort();

            thRead_Tag = new Thread(() =>
            {
                try
                {
                    USBHID_Device = new HIDDevice(USBHID_DevicePath, false);

                    textBox2.Invoke(new Action(() => textBox2.Text = ""));

                    USBHID_Device.Write(new byte[] { 0x20, 0xFF });

                    USBHID_Device.Write(new byte[] { 0x11 });
                    USBHID_Device.Read();
                    USBHID_Device.Write(new byte[] { 0x10 });
                    USBHID_Device.Read();

                    textBox2.Invoke(new Action(() => textBox2.AppendText(i18n.__("PowerSaves_Wait_Tag"))));

                    byte[] UID_Data;

                    while (true)
                    {
                        USBHID_Device.Write(new byte[] { 0x12 });
                        UID_Data = USBHID_Device.Read();

                        if (UID_Data.Skip(5).Take(1).ToArray()[0] == 7) break;
                    }

                    textBox2.Invoke(new Action(() => textBox2.AppendText(" " + i18n.__("PowerSaves_Found_NTAG") + Environment.NewLine)));
                    textBox2.Invoke(new Action(() => textBox2.AppendText(i18n.__("PowerSaves_Short_UID") + " " + BitConverter.ToString(UID_Data.Skip(6).Take(7).ToArray()).Replace("-", "") + Environment.NewLine + Environment.NewLine)));

                    byte[] Amiibo_Dump = new byte[0];

                    for (int i = 0x00; i < 0x88; i = i + 0x04)
                    {
                    Read:
                        int Take = 16;
                        if (i == 0x84) Take = 12;

                        USBHID_Device.Write(new byte[] { 0x1C, (byte)i });
                        byte[] Readed_Bytes = USBHID_Device.Read().Skip(3).Take(Take).ToArray();

                        int Sum = 0;
                        foreach (byte Value in Readed_Bytes) { Sum += Value; }
                        if (Sum == 0)
                        {
                            textBox2.Invoke(new Action(() => textBox2.AppendText(i18n.__("PowerSaves_Pages_Error", i, i + 3) + Environment.NewLine)));
                            goto Read;
                        }

                        textBox2.Invoke(new Action(() => textBox2.AppendText(i18n.__("PowerSaves_Pages_Readed", i, i + 3) + Environment.NewLine)));
                        Array.Resize(ref Amiibo_Dump, Amiibo_Dump.Length + Take);
                        Array.Copy(Readed_Bytes, 0, Amiibo_Dump, Amiibo_Dump.Length - Take, Take);
                    }

                    textBox2.Invoke(new Action(() => textBox2.AppendText(i18n.__("PowerSaves_Reading_Finished") + Environment.NewLine)));

                    this.Invoke(new Action(() => {

                        byte[] Decrypted_Amiibo = Amiibo_Class.Decrypt(Amiibo_Dump, Main_Form.AmiiKeys);
                        string[] AmiiboLife_Info = AmiiboInfo_Class.Get_AmiiboInfo(Amiibo_Class.Get_NFC_ID(Decrypted_Amiibo));
                        byte[] UID = new byte[0x07];
                        Array.Copy(Amiibo_Dump, 0, UID, 0, UID.Length);

                        SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                        saveFileDialog1.Filter = i18n.__("PowerSaves_Save_Dump_Amiibo_Filter", "|*.bin");
                        saveFileDialog1.Title = i18n.__("PowerSaves_Save_Dump_Amiibo_Title");
                        saveFileDialog1.InitialDirectory = Bin_Folder;

                        saveFileDialog1.FileName = ((AmiiboLife_Info[0].Trim() != "") ? AmiiboLife_Info[0].Trim().Replace(" ", "_") : BitConverter.ToString(UID).Replace("-", "")) + ".bin";

                        if (saveFileDialog1.ShowDialog(this.Parent) == DialogResult.OK && saveFileDialog1.FileName != "")
                        {
                            File.WriteAllBytes(saveFileDialog1.FileName, Amiibo_Dump);
                            MessageBox.Show(this, i18n.__("PowerSaves_Save_Dump_Amiibo_Message"), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }));

                    USBHID_Device.Write(new byte[] { 0x20, 0x00 });
                    USBHID_Device.Close();

                    button1.Invoke(new Action(() => button1.Enabled = true));
                }
                catch (ThreadAbortException)
                {
                    USBHID_Device.Write(new byte[] { 0x20, 0x00 });
                    USBHID_Device.Close();
                }
            });

            thRead_Tag.Start();
        }
    }
}
