namespace DoAnMonHocNT106
{
    partial class Form_Leaderboard
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.ListView lstLeaderboard;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblTitle;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_Leaderboard));
            this.lstLeaderboard = new System.Windows.Forms.ListView();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lstLeaderboard
            // 
            this.lstLeaderboard.FullRowSelect = true;
            this.lstLeaderboard.GridLines = true;
            this.lstLeaderboard.HideSelection = false;
            this.lstLeaderboard.Location = new System.Drawing.Point(32, 89);
            this.lstLeaderboard.Name = "lstLeaderboard";
            this.lstLeaderboard.Size = new System.Drawing.Size(736, 373);
            this.lstLeaderboard.TabIndex = 0;
            this.lstLeaderboard.UseCompatibleStateImageBehavior = false;
            this.lstLeaderboard.View = System.Windows.Forms.View.Details;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.IndianRed;
            this.btnClose.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(343, 483);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(114, 43);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Đóng";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.BackColor = System.Drawing.Color.Silver;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.Black;
            this.lblTitle.Location = new System.Drawing.Point(32, 46);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(184, 32);
            this.lblTitle.TabIndex = 2;
            this.lblTitle.Text = "Bảng xếp hạng";
            // 
            // Form_Leaderboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::DoAnMonHocNT106.Properties.Resources._4768220eda2bdf16308f85bc566d46f7;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(806, 550);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lstLeaderboard);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form_Leaderboard";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Bảng xếp hạng";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}