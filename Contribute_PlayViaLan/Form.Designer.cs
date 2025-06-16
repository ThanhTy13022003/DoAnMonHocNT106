using System.Windows.Forms;

namespace DoAnMonHocNT106
{
    partial class Form
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

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form));
            this.timeLabel = new System.Windows.Forms.Label();
            this.setTimeComboBox = new System.Windows.Forms.ComboBox();
            this.gamePanel = new System.Windows.Forms.Panel();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.hostButton = new System.Windows.Forms.Button();
            this.joinButton = new System.Windows.Forms.Button();
            this.chatBox = new System.Windows.Forms.TextBox();
            this.sendButton = new System.Windows.Forms.Button();
            this.ruleLabel = new System.Windows.Forms.Label();
            this.findRandomButton = new System.Windows.Forms.Button();
            this.statusLabel = new System.Windows.Forms.TextBox();
            this.newGameButton = new System.Windows.Forms.Button();
            this.resetButton = new System.Windows.Forms.Button();
            this.undoButton = new System.Windows.Forms.Button();
            this.exitButton = new System.Windows.Forms.Button();
            this.youLabel = new System.Windows.Forms.Label();
            this.opponentLabel = new System.Windows.Forms.Label();
            this.chatHistory = new System.Windows.Forms.TextBox();
            this.yourNameLabel = new System.Windows.Forms.Label();
            this.opponentNameLabel = new System.Windows.Forms.Label();
            this.yourDetailsButton = new System.Windows.Forms.Button();
            this.opponentDetailsButton = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // timeLabel
            // 
            this.timeLabel.AutoSize = true;
            this.timeLabel.Location = new System.Drawing.Point(1253, 486);
            this.timeLabel.Name = "timeLabel";
            this.timeLabel.Size = new System.Drawing.Size(65, 16);
            this.timeLabel.TabIndex = 5;
            this.timeLabel.Text = "Time: 20s";
            this.timeLabel.Click += new System.EventHandler(this.timeLabel_Click);
            // 
            // setTimeComboBox
            // 
            this.setTimeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.setTimeComboBox.Items.AddRange(new object[] {
            "20s",
            "30s",
            "40s",
            "60s"});
            this.setTimeComboBox.Location = new System.Drawing.Point(1098, 486);
            this.setTimeComboBox.Name = "setTimeComboBox";
            this.setTimeComboBox.Size = new System.Drawing.Size(100, 24);
            this.setTimeComboBox.TabIndex = 4;
            this.setTimeComboBox.SelectedIndexChanged += new System.EventHandler(this.SetTimeComboBox_SelectedIndexChanged);
            // 
            // gamePanel
            // 
            this.gamePanel.Location = new System.Drawing.Point(33, 35);
            this.gamePanel.Margin = new System.Windows.Forms.Padding(4);
            this.gamePanel.Name = "gamePanel";
            this.gamePanel.Size = new System.Drawing.Size(796, 738);
            this.gamePanel.TabIndex = 0;
            this.gamePanel.Paint += new System.Windows.Forms.PaintEventHandler(this.gamePanel_Paint);
            // 
            // pictureBox3
            // 
            this.pictureBox3.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox3.Image")));
            this.pictureBox3.Location = new System.Drawing.Point(841, 622);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(216, 188);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox3.TabIndex = 23;
            this.pictureBox3.TabStop = false;
            // 
            // hostButton
            // 
            this.hostButton.Location = new System.Drawing.Point(1097, 330);
            this.hostButton.Margin = new System.Windows.Forms.Padding(4);
            this.hostButton.Name = "hostButton";
            this.hostButton.Size = new System.Drawing.Size(230, 40);
            this.hostButton.TabIndex = 1;
            this.hostButton.Text = "Host Game";
            this.hostButton.UseVisualStyleBackColor = true;
            this.hostButton.Click += new System.EventHandler(this.HostGame_Click);
            // 
            // joinButton
            // 
            this.joinButton.Location = new System.Drawing.Point(1097, 378);
            this.joinButton.Margin = new System.Windows.Forms.Padding(4);
            this.joinButton.Name = "joinButton";
            this.joinButton.Size = new System.Drawing.Size(230, 40);
            this.joinButton.TabIndex = 2;
            this.joinButton.Text = "Join Game";
            this.joinButton.UseVisualStyleBackColor = true;
            this.joinButton.Click += new System.EventHandler(this.JoinGame_Click);
            // 
            // chatBox
            // 
            this.chatBox.Location = new System.Drawing.Point(841, 440);
            this.chatBox.Margin = new System.Windows.Forms.Padding(4);
            this.chatBox.Name = "chatBox";
            this.chatBox.Size = new System.Drawing.Size(230, 22);
            this.chatBox.TabIndex = 7;
            this.chatBox.TextChanged += new System.EventHandler(this.chatBox_TextChanged);
            this.chatBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ChatBox_KeyDown);
            // 
            // sendButton
            // 
            this.sendButton.Location = new System.Drawing.Point(841, 470);
            this.sendButton.Margin = new System.Windows.Forms.Padding(4);
            this.sendButton.Name = "sendButton";
            this.sendButton.Size = new System.Drawing.Size(230, 40);
            this.sendButton.TabIndex = 8;
            this.sendButton.Text = "Send";
            this.sendButton.UseVisualStyleBackColor = true;
            this.sendButton.Click += new System.EventHandler(this.SendButton_Click);
            // 
            // ruleLabel
            // 
            this.ruleLabel.AutoSize = true;
            this.ruleLabel.Font = new System.Drawing.Font("Ravie", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ruleLabel.ForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.ruleLabel.Location = new System.Drawing.Point(1064, 656);
            this.ruleLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ruleLabel.Name = "ruleLabel";
            this.ruleLabel.Size = new System.Drawing.Size(333, 34);
            this.ruleLabel.TabIndex = 12;
            this.ruleLabel.Text = "5 in a row to win!";
            this.ruleLabel.Click += new System.EventHandler(this.ruleLabel_Click);
            // 
            // findRandomButton
            // 
            this.findRandomButton.Location = new System.Drawing.Point(1097, 426);
            this.findRandomButton.Margin = new System.Windows.Forms.Padding(4);
            this.findRandomButton.Name = "findRandomButton";
            this.findRandomButton.Size = new System.Drawing.Size(230, 40);
            this.findRandomButton.TabIndex = 3;
            this.findRandomButton.Text = "Find Random";
            this.findRandomButton.UseVisualStyleBackColor = true;
            this.findRandomButton.Click += new System.EventHandler(this.FindRandom_Click);
            // 
            // statusLabel
            // 
            this.statusLabel.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.statusLabel.Location = new System.Drawing.Point(1097, 521);
            this.statusLabel.Margin = new System.Windows.Forms.Padding(4);
            this.statusLabel.Multiline = true;
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.ReadOnly = true;
            this.statusLabel.Size = new System.Drawing.Size(230, 57);
            this.statusLabel.TabIndex = 11;
            this.statusLabel.TextChanged += new System.EventHandler(this.statusLabel_TextChanged);
            // 
            // newGameButton
            // 
            this.newGameButton.Location = new System.Drawing.Point(1097, 704);
            this.newGameButton.Margin = new System.Windows.Forms.Padding(4);
            this.newGameButton.Name = "newGameButton";
            this.newGameButton.Size = new System.Drawing.Size(110, 40);
            this.newGameButton.TabIndex = 13;
            this.newGameButton.Text = "New Game";
            this.newGameButton.UseVisualStyleBackColor = true;
            this.newGameButton.Click += new System.EventHandler(this.NewGameButton_Click);
            // 
            // resetButton
            // 
            this.resetButton.Location = new System.Drawing.Point(1217, 704);
            this.resetButton.Margin = new System.Windows.Forms.Padding(4);
            this.resetButton.Name = "resetButton";
            this.resetButton.Size = new System.Drawing.Size(110, 40);
            this.resetButton.TabIndex = 14;
            this.resetButton.Text = "Reset";
            this.resetButton.UseVisualStyleBackColor = true;
            this.resetButton.Click += new System.EventHandler(this.ResetButton_Click);
            // 
            // undoButton
            // 
            this.undoButton.Location = new System.Drawing.Point(1097, 752);
            this.undoButton.Margin = new System.Windows.Forms.Padding(4);
            this.undoButton.Name = "undoButton";
            this.undoButton.Size = new System.Drawing.Size(110, 40);
            this.undoButton.TabIndex = 15;
            this.undoButton.Text = "Undo";
            this.undoButton.UseVisualStyleBackColor = true;
            this.undoButton.Click += new System.EventHandler(this.UndoButton_Click);
            this.undoButton.Enabled = false; // Disable undo button initially
            // 
            // exitButton
            // 
            this.exitButton.Location = new System.Drawing.Point(1217, 752);
            this.exitButton.Margin = new System.Windows.Forms.Padding(4);
            this.exitButton.Name = "exitButton";
            this.exitButton.Size = new System.Drawing.Size(110, 40);
            this.exitButton.TabIndex = 16;
            this.exitButton.Text = "Exit";
            this.exitButton.UseVisualStyleBackColor = true;
            this.exitButton.Click += new System.EventHandler(this.ExitButton_Click);
            // 
            // youLabel
            // 
            this.youLabel.Location = new System.Drawing.Point(1097, 628);
            this.youLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.youLabel.Name = "youLabel";
            this.youLabel.Size = new System.Drawing.Size(230, 28);
            this.youLabel.TabIndex = 10;
            this.youLabel.Click += new System.EventHandler(this.youLabel_Click);
            // 
            // opponentLabel
            // 
            this.opponentLabel.Location = new System.Drawing.Point(1097, 591);
            this.opponentLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.opponentLabel.Name = "opponentLabel";
            this.opponentLabel.Size = new System.Drawing.Size(230, 25);
            this.opponentLabel.TabIndex = 9;
            this.opponentLabel.Click += new System.EventHandler(this.opponentLabel_Click);
            // 
            // chatHistory
            // 
            this.chatHistory.Location = new System.Drawing.Point(841, 316);
            this.chatHistory.Margin = new System.Windows.Forms.Padding(4);
            this.chatHistory.Multiline = true;
            this.chatHistory.Name = "chatHistory";
            this.chatHistory.ReadOnly = true;
            this.chatHistory.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.chatHistory.Size = new System.Drawing.Size(230, 120);
            this.chatHistory.TabIndex = 6;
            this.chatHistory.TextChanged += new System.EventHandler(this.chatHistory_TextChanged);
            // 
            // yourNameLabel
            // 
            this.yourNameLabel.AutoSize = true;
            this.yourNameLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.yourNameLabel.Location = new System.Drawing.Point(849, 590);
            this.yourNameLabel.Name = "yourNameLabel";
            this.yourNameLabel.Size = new System.Drawing.Size(0, 18);
            this.yourNameLabel.TabIndex = 17;
            // 
            // opponentNameLabel
            // 
            this.opponentNameLabel.AutoSize = true;
            this.opponentNameLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.opponentNameLabel.Location = new System.Drawing.Point(849, 536);
            this.opponentNameLabel.Name = "opponentNameLabel";
            this.opponentNameLabel.Size = new System.Drawing.Size(0, 18);
            this.opponentNameLabel.TabIndex = 18;
            // 
            // yourDetailsButton
            // 
            this.yourDetailsButton.Location = new System.Drawing.Point(996, 583);
            this.yourDetailsButton.Margin = new System.Windows.Forms.Padding(4);
            this.yourDetailsButton.Name = "yourDetailsButton";
            this.yourDetailsButton.Size = new System.Drawing.Size(75, 25);
            this.yourDetailsButton.TabIndex = 19;
            this.yourDetailsButton.Text = "Details";
            this.yourDetailsButton.UseVisualStyleBackColor = true;
            this.yourDetailsButton.Click += new System.EventHandler(this.YourDetailsButton_Click);
            // 
            // opponentDetailsButton
            // 
            this.opponentDetailsButton.Location = new System.Drawing.Point(996, 529);
            this.opponentDetailsButton.Margin = new System.Windows.Forms.Padding(4);
            this.opponentDetailsButton.Name = "opponentDetailsButton";
            this.opponentDetailsButton.Size = new System.Drawing.Size(75, 25);
            this.opponentDetailsButton.TabIndex = 20;
            this.opponentDetailsButton.Text = "Details";
            this.opponentDetailsButton.UseVisualStyleBackColor = true;
            this.opponentDetailsButton.Click += new System.EventHandler(this.OpponentDetailsButton_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(943, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(336, 297);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 21;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1380, 822);
            this.Controls.Add(this.pictureBox3);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.gamePanel);
            this.Controls.Add(this.hostButton);
            this.Controls.Add(this.joinButton);
            this.Controls.Add(this.findRandomButton);
            this.Controls.Add(this.opponentNameLabel);
            this.Controls.Add(this.opponentDetailsButton);
            this.Controls.Add(this.opponentLabel);
            this.Controls.Add(this.yourNameLabel);
            this.Controls.Add(this.yourDetailsButton);
            this.Controls.Add(this.youLabel);
            this.Controls.Add(this.timeLabel);
            this.Controls.Add(this.setTimeComboBox);
            this.Controls.Add(this.chatHistory);
            this.Controls.Add(this.chatBox);
            this.Controls.Add(this.sendButton);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.ruleLabel);
            this.Controls.Add(this.newGameButton);
            this.Controls.Add(this.resetButton);
            this.Controls.Add(this.undoButton);
            this.Controls.Add(this.exitButton);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Game Caro";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        // Thêm hàm xử lý sự kiện KeyDown
        private void ChatBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true; // Ngăn Enter tạo dòng mới trong TextBox
                SendButton_Click(sender, e); // Gọi hàm SendButton_Click để gửi tin nhắn
            }
        }

        private System.Windows.Forms.Panel gamePanel;
        private System.Windows.Forms.Button hostButton;
        private System.Windows.Forms.Button joinButton;
        private System.Windows.Forms.TextBox chatBox;
        private System.Windows.Forms.Button sendButton;
        private System.Windows.Forms.Label ruleLabel;
        private System.Windows.Forms.Button findRandomButton;
        private System.Windows.Forms.TextBox statusLabel;
        private System.Windows.Forms.Button newGameButton;
        private System.Windows.Forms.Button resetButton;
        private System.Windows.Forms.Button undoButton;
        private System.Windows.Forms.Button exitButton;
        private System.Windows.Forms.Label youLabel;
        private System.Windows.Forms.Label timeLabel;
        private System.Windows.Forms.ComboBox setTimeComboBox;
        private System.Windows.Forms.Label opponentLabel;
        private System.Windows.Forms.TextBox chatHistory;
        private System.Windows.Forms.Label yourNameLabel;
        private System.Windows.Forms.Label opponentNameLabel;
        private System.Windows.Forms.Button yourDetailsButton;
        private System.Windows.Forms.Button opponentDetailsButton;

        private void Form1_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e) { }

        private PictureBox pictureBox1;
        private PictureBox pictureBox3;
    }
}