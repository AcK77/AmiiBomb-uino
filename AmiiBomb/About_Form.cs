using System.Diagnostics;
using System.Windows.Forms;

namespace AmiiBomb
{
    public partial class About_Form : Form
    {
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
        }
    }
}
