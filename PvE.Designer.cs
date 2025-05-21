namespace DoAnMonHocNT106
{
    partial class PvE
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel panelBoard;
        private System.Windows.Forms.Button btnRestart;
        private System.Windows.Forms.Label lblTimer;
        private System.Windows.Forms.Label lblStatus;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.panelBoard = new System.Windows.Forms.Panel();
            this.btnRestart = new System.Windows.Forms.Button();
            this.lblTimer = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // panelBoard
            // 
            this.panelBoard.Location = new System.Drawing.Point(12, 50);
            this.panelBoard.Name = "panelBoard";
            this.panelBoard.Size = new System.Drawing.Size(600, 600);
            this.panelBoard.TabIndex = 0;
            // 
            // btnRestart
            // 
            this.btnRestart.Location = new System.Drawing.Point(620, 50);
            this.btnRestart.Name = "btnRestart";
            this.btnRestart.Size = new System.Drawing.Size(100, 40);
            this.btnRestart.TabIndex = 1;
            this.btnRestart.Text = "Chơi lại";
            this.btnRestart.UseVisualStyleBackColor = true;
            this.btnRestart.Click += new System.EventHandler(this.btnRestart_Click);
            // 
            // lblTimer
            // 
            this.lblTimer.AutoSize = true;
            this.lblTimer.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblTimer.Location = new System.Drawing.Point(620, 110);
            this.lblTimer.Name = "lblTimer";
            this.lblTimer.Size = new System.Drawing.Size(123, 23);
            this.lblTimer.TabIndex = 3;
            this.lblTimer.Text = "Thời gian: 10s";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblStatus.Location = new System.Drawing.Point(12, 15);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(79, 23);
            this.lblStatus.TabIndex = 2;
            this.lblStatus.Text = "Lượt: ---";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(620, 488);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 40);
            this.button1.TabIndex = 4;
            this.button1.Text = "Thoát";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // PvE
            // 
            this.ClientSize = new System.Drawing.Size(740, 660);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.lblTimer);
            this.Controls.Add(this.btnRestart);
            this.Controls.Add(this.panelBoard);
            this.Name = "PvE";
            this.Text = "Cờ Caro - Người vs Bot AI (10s)";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Button button1;
    }
}
