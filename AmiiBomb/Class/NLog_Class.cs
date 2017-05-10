using NLog;
using NLog.Config;
using NLog.Targets;
using System;
using System.Windows.Forms;

namespace AmiiBomb
{
        [Target("TextBox")]
        public sealed class TextBoxTarget : TargetWithLayout
        {
            public TextBoxTarget()
            {
                this.Host = "localhost";
            }

            [RequiredParameter]
            public string Host { get; set; }

            public TextBox TextBox_Console { get; set; }

            protected override void Write(LogEventInfo logEvent)
            {
                string logMessage = this.Layout.Render(logEvent);

                SendTheMessageToRemoteHost(this.Host, logMessage);
            }

            private void SendTheMessageToRemoteHost(string host, string message)
            {
                TextBox_Console.Invoke(new Action(() => TextBox_Console.AppendText("> " + message + Environment.NewLine)));
            }
        }

}
