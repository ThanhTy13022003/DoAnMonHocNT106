using System.Windows.Forms;

namespace GameCaro_PlayToBot
{
    partial class PlayToBot
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel panelBoard;
        private System.Windows.Forms.Button btnRestart;
        private System.Windows.Forms.Label lblTimer;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnUndo;
        private System.Windows.Forms.ComboBox cbTimeLimit;
        private System.Windows.Forms.ComboBox cbDifficulty;
        private System.Windows.Forms.Label lblTimeLimit;
        private System.Windows.Forms.Label lblDifficulty;
        private System.Windows.Forms.Label lblUsername;

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
            this.panelBoard = new System.Windows.Forms.Panel();
            this.btnRestart = new System.Windows.Forms.Button();
            this.lblTimer = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnUndo = new System.Windows.Forms.Button();
            this.cbTimeLimit = new System.Windows.Forms.ComboBox();
            this.cbDifficulty = new System.Windows.Forms.ComboBox();
            this.lblTimeLimit = new System.Windows.Forms.Label();
            this.lblDifficulty = new System.Windows.Forms.Label();
            this.lblUsername = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnPause = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // panelBoard
            // 
            this.panelBoard.Location = new System.Drawing.Point(12, 80);
            this.panelBoard.Name = "panelBoard";
            this.panelBoard.Size = new System.Drawing.Size(600, 600);
            this.panelBoard.TabIndex = 0;
            // 
            // btnRestart
            // 
            this.btnRestart.Location = new System.Drawing.Point(662, 336);
            this.btnRestart.Name = "btnRestart";
            this.btnRestart.Size = new System.Drawing.Size(100, 40);
            this.btnRestart.TabIndex = 1;
            this.btnRestart.Text = "New Game";
            this.btnRestart.UseVisualStyleBackColor = true;
            this.btnRestart.Click += new System.EventHandler(this.btnRestart_Click);
            // 
            // lblTimer
            // 
            this.lblTimer.AutoSize = true;
            this.lblTimer.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblTimer.Location = new System.Drawing.Point(693, 481);
            this.lblTimer.Name = "lblTimer";
            this.lblTimer.Size = new System.Drawing.Size(87, 23);
            this.lblTimer.TabIndex = 3;
            this.lblTimer.Text = "Time: 10s";
            this.lblTimer.Click += new System.EventHandler(this.lblTimer_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblStatus.Location = new System.Drawing.Point(30, 43);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(77, 23);
            this.lblStatus.TabIndex = 2;
            this.lblStatus.Text = "Turn: ---";
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(790, 336);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(100, 40);
            this.btnExit.TabIndex = 4;
            this.btnExit.Text = "Quit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnUndo
            // 
            this.btnUndo.Location = new System.Drawing.Point(790, 398);
            this.btnUndo.Name = "btnUndo";
            this.btnUndo.Size = new System.Drawing.Size(100, 40);
            this.btnUndo.TabIndex = 5;
            this.btnUndo.Text = "Undo";
            this.btnUndo.UseVisualStyleBackColor = true;
            this.btnUndo.Click += new System.EventHandler(this.btnUndo_Click);
            // 
            // cbTimeLimit
            // 
            this.cbTimeLimit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTimeLimit.Items.AddRange(new object[] {
        "5 seconds",
        "10 seconds",
        "15 seconds",
        "20 seconds"});
            this.cbTimeLimit.Location = new System.Drawing.Point(697, 565);
            this.cbTimeLimit.Name = "cbTimeLimit";
            this.cbTimeLimit.Size = new System.Drawing.Size(100, 24);
            this.cbTimeLimit.TabIndex = 6;
            this.cbTimeLimit.SelectedIndex = 1; // Set default selection to "10 seconds"
            this.cbTimeLimit.SelectedIndexChanged += new System.EventHandler(this.cbTimeLimit_SelectedIndexChanged);

            // 
            // cbDifficulty
            // 
            this.cbDifficulty.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDifficulty.Location = new System.Drawing.Point(697, 642);
            this.cbDifficulty.Name = "cbDifficulty";
            this.cbDifficulty.Size = new System.Drawing.Size(100, 24);
            this.cbDifficulty.TabIndex = 7;
            // 
            // lblTimeLimit
            // 
            this.lblTimeLimit.AutoSize = true;
            this.lblTimeLimit.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblTimeLimit.Location = new System.Drawing.Point(697, 535);
            this.lblTimeLimit.Name = "lblTimeLimit";
            this.lblTimeLimit.Size = new System.Drawing.Size(92, 23);
            this.lblTimeLimit.TabIndex = 8;
            this.lblTimeLimit.Text = "Wait Time";
            // 
            // lblDifficulty
            // 
            this.lblDifficulty.AutoSize = true;
            this.lblDifficulty.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblDifficulty.Location = new System.Drawing.Point(697, 612);
            this.lblDifficulty.Name = "lblDifficulty";
            this.lblDifficulty.Size = new System.Drawing.Size(77, 23);
            this.lblDifficulty.TabIndex = 9;
            this.lblDifficulty.Text = "Difficult";
            this.lblDifficulty.Click += new System.EventHandler(this.lblDifficulty_Click);
            // 
            // lblUsername
            // 
            this.lblUsername.AutoSize = true;
            this.lblUsername.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblUsername.Location = new System.Drawing.Point(30, 20);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(84, 23);
            this.lblUsername.TabIndex = 10;
            this.lblUsername.Text = "Hi Guest!";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::DoAn_EarlyAccess_PlayToBot.Properties.Resources.images;
            this.pictureBox1.Location = new System.Drawing.Point(620, 31);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(321, 299);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 11;
            this.pictureBox1.TabStop = false;
            // 
            // btnPause
            // 
            this.btnPause.Location = new System.Drawing.Point(662, 398);
            this.btnPause.Name = "btnPause";
            this.btnPause.Size = new System.Drawing.Size(100, 40);
            this.btnPause.TabIndex = 12;
            this.btnPause.Text = "Pause";
            this.btnPause.UseVisualStyleBackColor = true;
            this.btnPause.Click += new System.EventHandler(this.btnPause_Click);
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(953, 700);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.lblUsername);
            this.Controls.Add(this.lblDifficulty);
            this.Controls.Add(this.lblTimeLimit);
            this.Controls.Add(this.cbDifficulty);
            this.Controls.Add(this.cbTimeLimit);
            this.Controls.Add(this.btnUndo);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.lblTimer);
            this.Controls.Add(this.btnRestart);
            this.Controls.Add(this.panelBoard);
            this.Controls.Add(this.btnPause);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Caro Game - Human vs Bot AI";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnPause;

    }
}