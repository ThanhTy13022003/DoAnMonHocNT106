namespace DoAnMonHocNT106
{
    partial class Introducing
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblGameIntro;
        private System.Windows.Forms.Label lblTeam;
        private System.Windows.Forms.TextBox txtGameInfo;
        private System.Windows.Forms.TextBox txtTeamInfo;
        private System.Windows.Forms.Button btnClose;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Introducing));
            this.lblGameIntro = new System.Windows.Forms.Label();
            this.lblTeam = new System.Windows.Forms.Label();
            this.txtGameInfo = new System.Windows.Forms.TextBox();
            this.txtTeamInfo = new System.Windows.Forms.TextBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblGameIntro
            // 
            this.lblGameIntro.AutoSize = true;
            this.lblGameIntro.Location = new System.Drawing.Point(20, 20);
            this.lblGameIntro.Name = "lblGameIntro";
            this.lblGameIntro.Size = new System.Drawing.Size(111, 16);
            this.lblGameIntro.TabIndex = 0;
            this.lblGameIntro.Text = "Giới thiệu trò chơi:";
            // 
            // lblTeam
            // 
            this.lblTeam.AutoSize = true;
            this.lblTeam.Location = new System.Drawing.Point(20, 160);
            this.lblTeam.Name = "lblTeam";
            this.lblTeam.Size = new System.Drawing.Size(112, 16);
            this.lblTeam.TabIndex = 2;
            this.lblTeam.Text = "Đội ngũ phát triển:";
            // 
            // txtGameInfo
            // 
            this.txtGameInfo.Location = new System.Drawing.Point(20, 45);
            this.txtGameInfo.Multiline = true;
            this.txtGameInfo.Name = "txtGameInfo";
            this.txtGameInfo.ReadOnly = true;
            this.txtGameInfo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtGameInfo.Size = new System.Drawing.Size(760, 100);
            this.txtGameInfo.TabIndex = 1;
            this.txtGameInfo.Text = resources.GetString("txtGameInfo.Text");
            // 
            // txtTeamInfo
            // 
            this.txtTeamInfo.Location = new System.Drawing.Point(20, 185);
            this.txtTeamInfo.Multiline = true;
            this.txtTeamInfo.Name = "txtTeamInfo";
            this.txtTeamInfo.ReadOnly = true;
            this.txtTeamInfo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtTeamInfo.Size = new System.Drawing.Size(760, 100);
            this.txtTeamInfo.TabIndex = 3;
            this.txtTeamInfo.Text = resources.GetString("txtTeamInfo.Text");
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(700, 300);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "Đóng";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // Introducing
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 353);
            this.Controls.Add(this.lblGameIntro);
            this.Controls.Add(this.txtGameInfo);
            this.Controls.Add(this.lblTeam);
            this.Controls.Add(this.txtTeamInfo);
            this.Controls.Add(this.btnClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Introducing";
            this.Text = "Giới thiệu về Caro";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
    }
}
