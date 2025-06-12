using System.Windows.Forms;
using System;
using System.Drawing;

namespace GameCaro_SettingInforPlayer
{
    partial class Form
    {
        private System.ComponentModel.IContainer components = null;

        // Khai báo các controls
        private System.Windows.Forms.PictureBox pictureBoxAvatar;
        private System.Windows.Forms.Button btnChangeAvatar;
        private System.Windows.Forms.Label lblPlayerName;
        private System.Windows.Forms.TextBox txtPlayerName;
        private System.Windows.Forms.Label lblWins;
        private System.Windows.Forms.TextBox txtWins;
        private System.Windows.Forms.Label lblLosses;
        private System.Windows.Forms.TextBox txtLosses;
        private System.Windows.Forms.Label lblDraws;
        private System.Windows.Forms.TextBox txtDraws;
        private System.Windows.Forms.Label lblWinRate;
        private System.Windows.Forms.TextBox txtWinRate;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.TextBox txtStatus;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Label lblBirthDate;
        private System.Windows.Forms.MaskedTextBox txtBirthDate; // Sử dụng MaskedTextBox
        private System.Windows.Forms.Label lblNationality;
        private System.Windows.Forms.ComboBox cmbNationality; // Sử dụng ComboBox thay vì TextBox
        private System.Windows.Forms.Button btnUpdateInfo;
        private System.Windows.Forms.Button btnChangePassword;

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
            this.btnChangeAvatar = new System.Windows.Forms.Button();
            this.lblPlayerName = new System.Windows.Forms.Label();
            this.txtPlayerName = new System.Windows.Forms.TextBox();
            this.lblWins = new System.Windows.Forms.Label();
            this.txtWins = new System.Windows.Forms.TextBox();
            this.lblLosses = new System.Windows.Forms.Label();
            this.txtLosses = new System.Windows.Forms.TextBox();
            this.lblDraws = new System.Windows.Forms.Label();
            this.txtDraws = new System.Windows.Forms.TextBox();
            this.lblWinRate = new System.Windows.Forms.Label();
            this.txtWinRate = new System.Windows.Forms.TextBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.txtStatus = new System.Windows.Forms.TextBox();
            this.lblEmail = new System.Windows.Forms.Label();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.lblBirthDate = new System.Windows.Forms.Label();
            this.txtBirthDate = new System.Windows.Forms.MaskedTextBox();
            this.lblNationality = new System.Windows.Forms.Label();
            this.cmbNationality = new System.Windows.Forms.ComboBox();
            this.btnUpdateInfo = new System.Windows.Forms.Button();
            this.btnChangePassword = new System.Windows.Forms.Button();
            this.pictureBoxAvatar = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxAvatar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnChangeAvatar
            // 
            this.btnChangeAvatar.Location = new System.Drawing.Point(47, 158);
            this.btnChangeAvatar.Name = "btnChangeAvatar";
            this.btnChangeAvatar.Size = new System.Drawing.Size(133, 31);
            this.btnChangeAvatar.TabIndex = 1;
            this.btnChangeAvatar.Text = "Change Avartar";
            // 
            // lblPlayerName
            // 
            this.lblPlayerName.Location = new System.Drawing.Point(220, 20);
            this.lblPlayerName.Name = "lblPlayerName";
            this.lblPlayerName.Size = new System.Drawing.Size(114, 24);
            this.lblPlayerName.TabIndex = 2;
            this.lblPlayerName.Text = "Tên người chơi:";
            // 
            // txtPlayerName
            // 
            this.txtPlayerName.Location = new System.Drawing.Point(330, 20);
            this.txtPlayerName.Name = "txtPlayerName";
            this.txtPlayerName.ReadOnly = true;
            this.txtPlayerName.Size = new System.Drawing.Size(214, 22);
            this.txtPlayerName.TabIndex = 3;
            this.txtPlayerName.Text = "Người chơi";
            // 
            // lblWins
            // 
            this.lblWins.Location = new System.Drawing.Point(220, 50);
            this.lblWins.Name = "lblWins";
            this.lblWins.Size = new System.Drawing.Size(114, 24);
            this.lblWins.TabIndex = 4;
            this.lblWins.Text = "Số trận đã chơi:";
            // 
            // txtWins
            // 
            this.txtWins.Location = new System.Drawing.Point(330, 50);
            this.txtWins.Name = "txtWins";
            this.txtWins.ReadOnly = true;
            this.txtWins.Size = new System.Drawing.Size(214, 22);
            this.txtWins.TabIndex = 5;
            this.txtWins.Text = "0";
            // 
            // lblLosses
            // 
            this.lblLosses.Location = new System.Drawing.Point(220, 80);
            this.lblLosses.Name = "lblLosses";
            this.lblLosses.Size = new System.Drawing.Size(114, 24);
            this.lblLosses.TabIndex = 6;
            this.lblLosses.Text = "Số trận thắng:";
            // 
            // txtLosses
            // 
            this.txtLosses.Location = new System.Drawing.Point(330, 80);
            this.txtLosses.Name = "txtLosses";
            this.txtLosses.ReadOnly = true;
            this.txtLosses.Size = new System.Drawing.Size(214, 22);
            this.txtLosses.TabIndex = 7;
            this.txtLosses.Text = "0";
            // 
            // lblDraws
            // 
            this.lblDraws.Location = new System.Drawing.Point(220, 110);
            this.lblDraws.Name = "lblDraws";
            this.lblDraws.Size = new System.Drawing.Size(114, 24);
            this.lblDraws.TabIndex = 8;
            this.lblDraws.Text = "Số trận thua:";
            // 
            // txtDraws
            // 
            this.txtDraws.Location = new System.Drawing.Point(330, 110);
            this.txtDraws.Name = "txtDraws";
            this.txtDraws.ReadOnly = true;
            this.txtDraws.Size = new System.Drawing.Size(214, 22);
            this.txtDraws.TabIndex = 9;
            this.txtDraws.Text = "0";
            // 
            // lblWinRate
            // 
            this.lblWinRate.Location = new System.Drawing.Point(220, 140);
            this.lblWinRate.Name = "lblWinRate";
            this.lblWinRate.Size = new System.Drawing.Size(114, 24);
            this.lblWinRate.TabIndex = 10;
            this.lblWinRate.Text = "Tỉ lệ thắng:";
            // 
            // txtWinRate
            // 
            this.txtWinRate.Location = new System.Drawing.Point(330, 140);
            this.txtWinRate.Name = "txtWinRate";
            this.txtWinRate.ReadOnly = true;
            this.txtWinRate.Size = new System.Drawing.Size(214, 22);
            this.txtWinRate.TabIndex = 11;
            this.txtWinRate.Text = "0%";
            // 
            // lblStatus
            // 
            this.lblStatus.Location = new System.Drawing.Point(220, 170);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(114, 24);
            this.lblStatus.TabIndex = 12;
            this.lblStatus.Text = "Trạng thái:";
            // 
            // txtStatus
            // 
            this.txtStatus.Location = new System.Drawing.Point(330, 170);
            this.txtStatus.Name = "txtStatus";
            this.txtStatus.ReadOnly = true;
            this.txtStatus.Size = new System.Drawing.Size(214, 22);
            this.txtStatus.TabIndex = 13;
            this.txtStatus.Text = "Hoạt động";
            // 
            // lblEmail
            // 
            this.lblEmail.Location = new System.Drawing.Point(220, 200);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(114, 24);
            this.lblEmail.TabIndex = 14;
            this.lblEmail.Text = "Email:";
            // 
            // txtEmail
            // 
            this.txtEmail.Location = new System.Drawing.Point(330, 200);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.ReadOnly = true;
            this.txtEmail.Size = new System.Drawing.Size(214, 22);
            this.txtEmail.TabIndex = 15;
            this.txtEmail.Text = "player@example.com";
            // 
            // lblBirthDate
            // 
            this.lblBirthDate.Location = new System.Drawing.Point(220, 230);
            this.lblBirthDate.Name = "lblBirthDate";
            this.lblBirthDate.Size = new System.Drawing.Size(114, 24);
            this.lblBirthDate.TabIndex = 16;
            this.lblBirthDate.Text = "Ngày sinh:";
            // 
            // txtBirthDate
            // 
            this.txtBirthDate.Location = new System.Drawing.Point(330, 230);
            this.txtBirthDate.Mask = "00/00/0000";
            this.txtBirthDate.Name = "txtBirthDate";
            this.txtBirthDate.ReadOnly = true;
            this.txtBirthDate.Size = new System.Drawing.Size(214, 22);
            this.txtBirthDate.TabIndex = 17;
            this.txtBirthDate.Text = "01012000";
            this.txtBirthDate.ValidatingType = typeof(System.DateTime);
            // 
            // lblNationality
            // 
            this.lblNationality.Location = new System.Drawing.Point(220, 260);
            this.lblNationality.Name = "lblNationality";
            this.lblNationality.Size = new System.Drawing.Size(114, 24);
            this.lblNationality.TabIndex = 18;
            this.lblNationality.Text = "Quốc gia:";
            // 
            // cmbNationality
            // 
            this.cmbNationality.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbNationality.Enabled = false;
            this.cmbNationality.ForeColor = System.Drawing.Color.Black;
            this.cmbNationality.Items.AddRange(new object[] {
            "Việt Nam",
            "United States",
            "Japan",
            "China",
            "United Kingdom",
            "France",
            "Germany",
            "South Korea",
            "Australia",
            "Canada"});
            this.cmbNationality.Location = new System.Drawing.Point(330, 260);
            this.cmbNationality.Name = "cmbNationality";
            this.cmbNationality.Size = new System.Drawing.Size(214, 24);
            this.cmbNationality.TabIndex = 19;
            this.cmbNationality.DrawMode = DrawMode.OwnerDrawFixed;
            this.cmbNationality.DrawItem += new DrawItemEventHandler(cmbNationality_DrawItem);
            // 
            // btnUpdateInfo
            // 
            this.btnUpdateInfo.Location = new System.Drawing.Point(388, 334);
            this.btnUpdateInfo.Name = "btnUpdateInfo";
            this.btnUpdateInfo.Size = new System.Drawing.Size(156, 34);
            this.btnUpdateInfo.TabIndex = 20;
            this.btnUpdateInfo.Text = "Change Information";
            // 
            // btnChangePassword
            // 
            this.btnChangePassword.Location = new System.Drawing.Point(223, 334);
            this.btnChangePassword.Name = "btnChangePassword";
            this.btnChangePassword.Size = new System.Drawing.Size(146, 34);
            this.btnChangePassword.TabIndex = 21;
            this.btnChangePassword.Text = "Change Password";
            // 
            // pictureBoxAvatar
            // 
            this.pictureBoxAvatar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxAvatar.Location = new System.Drawing.Point(47, 27);
            this.pictureBoxAvatar.Name = "pictureBoxAvatar";
            this.pictureBoxAvatar.Size = new System.Drawing.Size(133, 116);
            this.pictureBoxAvatar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxAvatar.TabIndex = 0;
            this.pictureBoxAvatar.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = global::GameCaro_SettingInforPlayer.Properties.Resources.download2;
            this.pictureBox1.Image = global::GameCaro_SettingInforPlayer.Properties.Resources.download___Copy;
            this.pictureBox1.Location = new System.Drawing.Point(12, 214);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(232, 168);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 22;
            this.pictureBox1.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(566, 394);
            this.Controls.Add(this.pictureBoxAvatar);
            this.Controls.Add(this.btnChangeAvatar);
            this.Controls.Add(this.lblPlayerName);
            this.Controls.Add(this.txtPlayerName);
            this.Controls.Add(this.lblWins);
            this.Controls.Add(this.txtWins);
            this.Controls.Add(this.lblLosses);
            this.Controls.Add(this.txtLosses);
            this.Controls.Add(this.lblDraws);
            this.Controls.Add(this.txtDraws);
            this.Controls.Add(this.lblWinRate);
            this.Controls.Add(this.txtWinRate);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.txtStatus);
            this.Controls.Add(this.lblEmail);
            this.Controls.Add(this.txtEmail);
            this.Controls.Add(this.lblBirthDate);
            this.Controls.Add(this.txtBirthDate);
            this.Controls.Add(this.lblNationality);
            this.Controls.Add(this.cmbNationality);
            this.Controls.Add(this.btnUpdateInfo);
            this.Controls.Add(this.btnChangePassword);
            this.Controls.Add(this.pictureBox1);
            this.Name = "Form1";
            this.Text = "Player Information";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxAvatar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void cmbNationality_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            // Lấy đối tượng ComboBox
            ComboBox combo = sender as ComboBox;

            // Thiết lập màu chữ
            Color textColor = combo.Enabled ? Color.Black : Color.Black; // Luôn là màu đen

            // Vẽ nền (giữ màu nền mặc định của hệ thống, sẽ là màu xám khi vô hiệu hóa)
            e.DrawBackground();

            // Vẽ chữ với màu tùy chỉnh
            using (SolidBrush textBrush = new SolidBrush(textColor))
            {
                e.Graphics.DrawString(combo.Items[e.Index].ToString(), e.Font, textBrush, e.Bounds);
            }

            // Vẽ viền khi item được chọn (chỉ khi ComboBox được bật)
            if (combo.Enabled)
            {
                e.DrawFocusRectangle();
            }
        }

        private PictureBox pictureBox1;
    }
}