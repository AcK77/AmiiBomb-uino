using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace AmiiBomb
{
    public partial class About_Form : Form
    {
        [DllImport("user32.dll")]
        static extern int SetScrollPos(IntPtr hWnd, int nBar, int nPos, bool bRedraw);
        [DllImport("user32.dll")]
        static extern int SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, ref RECT lParam);

        [StructLayout(LayoutKind.Sequential)]
        struct RECT
        {
            public int Left, Top, Right, Bottom;
        }

        Thread thTextBox_Animate, thTextBox_Animate2, thTextBox_Animate3;

        public About_Form()
        {
            InitializeComponent();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://github.com/Falco20019/libamiibo");
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://github.com/socram8888/amiitool");
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://www.amiibo.life/");
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://github.com/konstantin-kelemen/arduino-amiibo-tools");
        }

        private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://gist.github.com/bkifft/c6fa52dc39e29b85cb2787ac6dd633ed");
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private void About_Form_Shown(object sender, System.EventArgs e)
        {
            Translate_Class.Translate(this);
            thTextBox_Animate = new Thread(() => AnimateTxtBox(textBox2));
            thTextBox_Animate.Start();

            thTextBox_Animate2 = new Thread(() => AnimateTxtBox(textBox3));
            thTextBox_Animate2.Start();

            thTextBox_Animate3 = new Thread(() => AnimateTxtBox(textBox4));
            thTextBox_Animate3.Start();
        }

        private void AnimateTxtBox(TextBox txtBox)
        {
            int Down_Up = 1;
            int i = 0;

            while(true)
            {
                txtBox.Invoke((MethodInvoker)delegate
                {
                    SetScrollPos(txtBox.Handle, 0x1, i, true);
                    SendMessage(txtBox.Handle, 0x115, 4 + 0x10000 * i, 0);
                });

                if (Down_Up == 1)
                {
                    var rect = new RECT();
                    var count = 1;
                    txtBox.Invoke((MethodInvoker)delegate
                    {
                        SendMessage(txtBox.Handle, 0xB2, IntPtr.Zero, ref rect);
                        count = (rect.Bottom - rect.Top) / txtBox.Font.Height;
                    });

                    if (i < txtBox.Lines.Length - count)  i++;
                    else Down_Up = 0;
                }

                if (Down_Up == 0)
                {
                    if (i > 0) i--;
                    else Down_Up = 1;
                }

                Thread.Sleep(800);
            }
        }

        private void textBox2_Click(object sender, EventArgs e)
        {
            if (thTextBox_Animate.IsAlive) thTextBox_Animate.Abort();
        }

        private void textBox4_Click(object sender, EventArgs e)
        {
            if (thTextBox_Animate3.IsAlive) thTextBox_Animate3.Abort();
        }

        private void textBox3_Click(object sender, EventArgs e)
        {
            if (thTextBox_Animate2.IsAlive) thTextBox_Animate2.Abort();
        }

        private void About_Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (thTextBox_Animate.IsAlive) thTextBox_Animate.Abort();
            if (thTextBox_Animate2.IsAlive) thTextBox_Animate2.Abort();
            if (thTextBox_Animate3.IsAlive) thTextBox_Animate3.Abort();
        }
    }
}
