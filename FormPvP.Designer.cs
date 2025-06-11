namespace DoAnMonHocNT106
{
    partial class FormPvP
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel panelBoard;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Button btnRestart;
        private System.Windows.Forms.Label lblYou;
        private System.Windows.Forms.Label lblOpponent;
        private System.Windows.Forms.Label lblCountdown;
        private System.Windows.Forms.ListView lstChat;
        private System.Windows.Forms.TextBox txtChat;
        private System.Windows.Forms.Button btnSendChat;


        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            // Chat List
            this.lstChat = new System.Windows.Forms.ListView();
            this.lstChat.HideSelection = false;
            this.lstChat.Location = new System.Drawing.Point(540, 260);
            this.lstChat.Name = "lstChat";
            this.lstChat.Size = new System.Drawing.Size(140, 300);
            this.lstChat.TabIndex = 6;
            this.lstChat.View = System.Windows.Forms.View.List;

            // Message TextBox
            this.txtChat = new System.Windows.Forms.TextBox();
            this.txtChat.Location = new System.Drawing.Point(540, 570);
            this.txtChat.Name = "txtChat";
            this.txtChat.Size = new System.Drawing.Size(100, 22);
            this.txtChat.TabIndex = 7;

            // Send Button
            this.btnSendChat = new System.Windows.Forms.Button();
            this.btnSendChat.Location = new System.Drawing.Point(640, 568);
            this.btnSendChat.Name = "btnSendChat";
            this.btnSendChat.Size = new System.Drawing.Size(40, 25);
            this.btnSendChat.TabIndex = 8;
            this.btnSendChat.Text = "Gửi";
            this.btnSendChat.UseVisualStyleBackColor = true;
            this.btnSendChat.Click += new System.EventHandler(this.btnSendChat_Click);

            // Đừng quên thêm này vào phần controls:
            this.Controls.Add(this.lstChat);
            this.Controls.Add(this.txtChat);
            this.Controls.Add(this.btnSendChat);

            this.panelBoard = new System.Windows.Forms.Panel();
            this.btnBack = new System.Windows.Forms.Button();
            this.btnRestart = new System.Windows.Forms.Button();
            this.lblYou = new System.Windows.Forms.Label();
            this.lblOpponent = new System.Windows.Forms.Label();
            this.lblCountdown = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // panelBoard
            // 
            this.panelBoard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.panelBoard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelBoard.Location = new System.Drawing.Point(20, 20);
            this.panelBoard.Name = "panelBoard";
            this.panelBoard.Size = new System.Drawing.Size(500, 600);
            this.panelBoard.TabIndex = 0;
            // 
            // btnBack
            // 
            this.btnBack.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.btnBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBack.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnBack.ForeColor = System.Drawing.Color.Red;
            this.btnBack.Location = new System.Drawing.Point(540, 30);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(120, 40);
            this.btnBack.TabIndex = 1;
            this.btnBack.Text = "❌ Thoát";
            this.btnBack.UseVisualStyleBackColor = false;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // btnRestart
            // 
            this.btnRestart.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(120)))), ((int)(((byte)(80)))));
            this.btnRestart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRestart.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnRestart.ForeColor = System.Drawing.Color.White;
            this.btnRestart.Location = new System.Drawing.Point(540, 80);
            this.btnRestart.Name = "btnRestart";
            this.btnRestart.Size = new System.Drawing.Size(120, 40);
            this.btnRestart.TabIndex = 2;
            this.btnRestart.Text = "🔁 Chơi lại";
            this.btnRestart.UseVisualStyleBackColor = false;
            this.btnRestart.Click += new System.EventHandler(this.btnRestart_Click);
            // 
            // lblYou
            // 
            this.lblYou.AutoSize = true;
            this.lblYou.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblYou.ForeColor = System.Drawing.Color.Cyan;
            this.lblYou.Location = new System.Drawing.Point(540, 150);
            this.lblYou.Name = "lblYou";
            this.lblYou.Size = new System.Drawing.Size(51, 19);
            this.lblYou.TabIndex = 3;
            this.lblYou.Text = "Bạn: X";
            // 
            // lblOpponent
            // 
            this.lblOpponent.AutoSize = true;
            this.lblOpponent.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblOpponent.ForeColor = System.Drawing.Color.Orange;
            this.lblOpponent.Location = new System.Drawing.Point(540, 180);
            this.lblOpponent.Name = "lblOpponent";
            this.lblOpponent.Size = new System.Drawing.Size(76, 19);
            this.lblOpponent.TabIndex = 4;
            this.lblOpponent.Text = "Đối thủ: O";
            // 
            // lblCountdown
            // 
            this.lblCountdown.AutoSize = true;
            this.lblCountdown.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblCountdown.ForeColor = System.Drawing.Color.Lime;
            this.lblCountdown.Location = new System.Drawing.Point(540, 220);
            this.lblCountdown.Name = "lblCountdown";
            this.lblCountdown.Size = new System.Drawing.Size(101, 19);
            this.lblCountdown.TabIndex = 5;
            this.lblCountdown.Text = "Thời gian: 20s";
            // 
            // FormPvP
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(15)))), ((int)(((byte)(15)))));
            this.BackgroundImage = global::DoAnMonHocNT106.Properties.Resources.pngtree_retro_futuristic_gaming_desk_scene_image_16555312;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(700, 650);
            this.Controls.Add(this.lblCountdown);
            this.Controls.Add(this.lblOpponent);
            this.Controls.Add(this.lblYou);
            this.Controls.Add(this.btnRestart);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.panelBoard);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "FormPvP";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "🎮 Cờ Caro PvP - NT106";
            this.Load += new System.EventHandler(this.FormPvP_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
