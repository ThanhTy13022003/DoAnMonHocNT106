using System;
using System.Windows.Forms;

namespace DoAnMonHocNT106
{
    public partial class Chatbox : Form
    {
        private string currentMode = "Global";
        private string chatGlobal = "";
        private string chatIngame = "";

        public Chatbox()
        {
            InitializeComponent();
            UpdateTitle();
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            string message = textBoxInput.Text.Trim();
            if (!string.IsNullOrEmpty(message))
            {
                string formatted = $"[You]: {message}\n";

                if (currentMode == "Global")
                {
                    chatGlobal += formatted;
                    richTextBoxDisplay.Text = chatGlobal;
                }
                else
                {
                    chatIngame += formatted;
                    richTextBoxDisplay.Text = chatIngame;
                }

                textBoxInput.Clear();
            }
        }

        private void chatGlobalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            currentMode = "Global";
            richTextBoxDisplay.Text = chatGlobal;
            UpdateTitle();
        }

        private void chatIngameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            currentMode = "Ingame";
            richTextBoxDisplay.Text = chatIngame;
            UpdateTitle();
        }

        private void UpdateTitle()
        {
            this.Text = $"Chatbox - {currentMode} Mode";
        }
    }
}
