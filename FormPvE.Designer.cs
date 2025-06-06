namespace DoAnMonHocNT106
{
    partial class FormPvE
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel panelBoard;
        private System.Windows.Forms.Button btnRestart;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Label lblTimer;
        private System.Windows.Forms.Label lblPlayerName;
        private System.Windows.Forms.Label lblTurn;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.panelBoard = new System.Windows.Forms.Panel();
            this.btnRestart = new System.Windows.Forms.Button();
            this.btnBack = new System.Windows.Forms.Button();
            this.lblTimer = new System.Windows.Forms.Label();
            this.lblPlayerName = new System.Windows.Forms.Label();
            this.lblTurn = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // panelBoard
            // 
            this.panelBoard.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panelBoard.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelBoard.Location = new System.Drawing.Point(20, 20);
            this.panelBoard.Name = "panelBoard";
            this.panelBoard.Size = new System.Drawing.Size(600, 600);
            this.panelBoard.TabIndex = 0;
            // 
            // btnRestart
            // 
            this.btnRestart.BackColor = System.Drawing.Color.SteelBlue;
            this.btnRestart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRestart.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnRestart.ForeColor = System.Drawing.Color.White;
            this.btnRestart.Location = new System.Drawing.Point(650, 180);
            this.btnRestart.Name = "btnRestart";
            this.btnRestart.Size = new System.Drawing.Size(160, 45);
            this.btnRestart.TabIndex = 1;
            this.btnRestart.Text = "🔁 Chơi lại";
            this.btnRestart.UseVisualStyleBackColor = false;
            this.btnRestart.Click += new System.EventHandler(this.btnRestart_Click);
            // 
            // btnBack
            // 
            this.btnBack.BackColor = System.Drawing.Color.IndianRed;
            this.btnBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBack.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnBack.ForeColor = System.Drawing.Color.White;
            this.btnBack.Location = new System.Drawing.Point(650, 240);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(160, 45);
            this.btnBack.TabIndex = 2;
            this.btnBack.Text = "⬅ Thoát";
            this.btnBack.UseVisualStyleBackColor = false;
            this.btnBack.Click += new System.EventHandler(this.button1_Click);
            // 
            // lblTimer
            // 
            this.lblTimer.AutoSize = true;
            this.lblTimer.BackColor = System.Drawing.Color.Silver;
            this.lblTimer.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTimer.ForeColor = System.Drawing.Color.Black;
            this.lblTimer.Location = new System.Drawing.Point(650, 80);
            this.lblTimer.Name = "lblTimer";
            this.lblTimer.Size = new System.Drawing.Size(160, 25);
            this.lblTimer.TabIndex = 3;
            this.lblTimer.Text = "⏱ Thời gian: 10s";
            // 
            // lblPlayerName
            // 
            this.lblPlayerName.AutoSize = true;
            this.lblPlayerName.BackColor = System.Drawing.Color.Silver;
            this.lblPlayerName.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblPlayerName.ForeColor = System.Drawing.Color.Black;
            this.lblPlayerName.Location = new System.Drawing.Point(650, 30);
            this.lblPlayerName.Name = "lblPlayerName";
            this.lblPlayerName.Size = new System.Drawing.Size(159, 25);
            this.lblPlayerName.TabIndex = 4;
            this.lblPlayerName.Text = "👤 Người chơi: ...";
            // 
            // lblTurn
            // 
            this.lblTurn.AutoSize = true;
            this.lblTurn.BackColor = System.Drawing.Color.Silver;
            this.lblTurn.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.lblTurn.ForeColor = System.Drawing.Color.DarkGreen;
            this.lblTurn.Location = new System.Drawing.Point(650, 130);
            this.lblTurn.Name = "lblTurn";
            this.lblTurn.Size = new System.Drawing.Size(155, 25);
            this.lblTurn.TabIndex = 5;
            this.lblTurn.Text = "Lượt: Người chơi";
            // 
            // FormPvE
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.BackgroundImage = global::DoAnMonHocNT106.Properties.Resources.pngtree_retro_futuristic_gaming_desk_scene_image_16555312;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(840, 650);
            this.Controls.Add(this.lblTurn);
            this.Controls.Add(this.lblPlayerName);
            this.Controls.Add(this.lblTimer);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.btnRestart);
            this.Controls.Add(this.panelBoard);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FormPvE";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Caro - PvE (Chơi với Máy)";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
