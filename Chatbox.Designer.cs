namespace DoAnMonHocNT106
{
    partial class Chatbox
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.chatModeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.chatGlobalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.chatIngameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.richTextBoxDisplay = new System.Windows.Forms.RichTextBox();
            this.textBoxInput = new System.Windows.Forms.TextBox();
            this.buttonSend = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.chatModeToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 28);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // chatModeToolStripMenuItem
            // 
            this.chatModeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.chatGlobalToolStripMenuItem,
            this.chatIngameToolStripMenuItem});
            this.chatModeToolStripMenuItem.Name = "chatModeToolStripMenuItem";
            this.chatModeToolStripMenuItem.Size = new System.Drawing.Size(96, 24);
            this.chatModeToolStripMenuItem.Text = "Chat Mode";
            // 
            // chatGlobalToolStripMenuItem
            // 
            this.chatGlobalToolStripMenuItem.Name = "chatGlobalToolStripMenuItem";
            this.chatGlobalToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.chatGlobalToolStripMenuItem.Text = "Chat Global";
            this.chatGlobalToolStripMenuItem.Click += new System.EventHandler(this.chatGlobalToolStripMenuItem_Click);
            // 
            // chatIngameToolStripMenuItem
            // 
            this.chatIngameToolStripMenuItem.Name = "chatIngameToolStripMenuItem";
            this.chatIngameToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.chatIngameToolStripMenuItem.Text = "Chat Ingame";
            this.chatIngameToolStripMenuItem.Click += new System.EventHandler(this.chatIngameToolStripMenuItem_Click);
            // 
            // richTextBoxDisplay
            // 
            this.richTextBoxDisplay.Location = new System.Drawing.Point(12, 40);
            this.richTextBoxDisplay.Name = "richTextBoxDisplay";
            this.richTextBoxDisplay.ReadOnly = true;
            this.richTextBoxDisplay.Size = new System.Drawing.Size(776, 330);
            this.richTextBoxDisplay.TabIndex = 1;
            this.richTextBoxDisplay.Text = "";
            // 
            // textBoxInput
            // 
            this.textBoxInput.Location = new System.Drawing.Point(12, 385);
            this.textBoxInput.Name = "textBoxInput";
            this.textBoxInput.Size = new System.Drawing.Size(680, 22);
            this.textBoxInput.TabIndex = 2;
            // 
            // buttonSend
            // 
            this.buttonSend.Location = new System.Drawing.Point(700, 383);
            this.buttonSend.Name = "buttonSend";
            this.buttonSend.Size = new System.Drawing.Size(88, 26);
            this.buttonSend.TabIndex = 3;
            this.buttonSend.Text = "Send";
            this.buttonSend.UseVisualStyleBackColor = true;
            this.buttonSend.Click += new System.EventHandler(this.buttonSend_Click);
            // 
            // Chatbox
            // 
            this.ClientSize = new System.Drawing.Size(800, 424);
            this.Controls.Add(this.buttonSend);
            this.Controls.Add(this.textBoxInput);
            this.Controls.Add(this.richTextBoxDisplay);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Chatbox";
            this.Text = "Chatbox";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem chatModeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem chatGlobalToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem chatIngameToolStripMenuItem;
        private System.Windows.Forms.RichTextBox richTextBoxDisplay;
        private System.Windows.Forms.TextBox textBoxInput;
        private System.Windows.Forms.Button buttonSend;
    }
}
