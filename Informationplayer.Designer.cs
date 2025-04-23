using System.Drawing;
using System.Windows.Forms;
using System;

namespace DoAnMonHocNT106
{
    public partial class Informationplayer : Form
    {
        private System.ComponentModel.IContainer components = null;
        private PictureBox picAvatar;
        private Button btnEditAvatar;
        private Label lblName, lblGamesPlayed, lblWins, lblLosses, lblWinRate, lblStatus;
        private TextBox txtName, txtGamesPlayed, txtWins, txtLosses, txtWinRate, txtStatus;
        private Button btnEditInfo;
        private bool isEditing = false;

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
            this.components = new System.ComponentModel.Container();
            this.picAvatar = new PictureBox();
            this.btnEditAvatar = new Button();
            this.lblName = new Label();
            this.txtName = new TextBox();
            this.lblGamesPlayed = new Label();
            this.txtGamesPlayed = new TextBox();
            this.lblWins = new Label();
            this.txtWins = new TextBox();
            this.lblLosses = new Label();
            this.txtLosses = new TextBox();
            this.lblWinRate = new Label();
            this.txtWinRate = new TextBox();
            this.lblStatus = new Label();
            this.txtStatus = new TextBox();
            this.btnEditInfo = new Button();

            ((System.ComponentModel.ISupportInitialize)(this.picAvatar)).BeginInit();
            this.SuspendLayout();

            // picAvatar
            this.picAvatar.Location = new Point(20, 20);
            this.picAvatar.Name = "picAvatar";
            this.picAvatar.Size = new Size(100, 100);
            this.picAvatar.SizeMode = PictureBoxSizeMode.Zoom;
            this.picAvatar.BorderStyle = BorderStyle.FixedSingle;

            // btnEditAvatar
            this.btnEditAvatar.Location = new Point(20, 130);
            this.btnEditAvatar.Name = "btnEditAvatar";
            this.btnEditAvatar.Size = new Size(100, 30);
            this.btnEditAvatar.Text = "Chỉnh sửa avatar";
            this.btnEditAvatar.UseVisualStyleBackColor = true;
            this.btnEditAvatar.Click += new EventHandler(this.BtnEditAvatar_Click);

            // lblName
            this.lblName.Text = "Tên người chơi:";
            this.lblName.Location = new Point(150, 30);
            this.lblName.Size = new Size(140, 25);

            // txtName
            this.txtName.Location = new Point(300, 30);
            this.txtName.Size = new Size(180, 25);

            // lblGamesPlayed
            this.lblGamesPlayed.Text = "Số trận đã chơi:";
            this.lblGamesPlayed.Location = new Point(150, 70);
            this.lblGamesPlayed.Size = new Size(140, 25);

            // txtGamesPlayed
            this.txtGamesPlayed.Location = new Point(300, 70);
            this.txtGamesPlayed.Size = new Size(180, 25);

            // lblWins
            this.lblWins.Text = "Số trận thắng:";
            this.lblWins.Location = new Point(150, 110);
            this.lblWins.Size = new Size(140, 25);

            // txtWins
            this.txtWins.Location = new Point(300, 110);
            this.txtWins.Size = new Size(180, 25);

            // lblLosses
            this.lblLosses.Text = "Số trận thua:";
            this.lblLosses.Location = new Point(150, 150);
            this.lblLosses.Size = new Size(140, 25);

            // txtLosses
            this.txtLosses.Location = new Point(300, 150);
            this.txtLosses.Size = new Size(180, 25);

            // lblWinRate
            this.lblWinRate.Text = "Tỉ lệ thắng:";
            this.lblWinRate.Location = new Point(150, 190);
            this.lblWinRate.Size = new Size(140, 25);

            // txtWinRate
            this.txtWinRate.Location = new Point(300, 190);
            this.txtWinRate.Size = new Size(180, 25);

            // lblStatus
            this.lblStatus.Text = "Trạng thái:";
            this.lblStatus.Location = new Point(150, 230);
            this.lblStatus.Size = new Size(140, 25);

            // txtStatus
            this.txtStatus.Location = new Point(300, 230);
            this.txtStatus.Size = new Size(180, 25);

            // btnEditInfo
            this.btnEditInfo.Location = new Point(320, 320);
            this.btnEditInfo.Name = "btnEditInfo";
            this.btnEditInfo.Size = new Size(140, 30);
            this.btnEditInfo.Text = "Sửa đổi thông tin";
            this.btnEditInfo.UseVisualStyleBackColor = true;
            this.btnEditInfo.Click += new EventHandler(this.BtnEditInfo_Click);

            // Form
            this.AutoScaleDimensions = new SizeF(8F, 16F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(500, 400);
            this.Controls.Add(this.picAvatar);
            this.Controls.Add(this.btnEditAvatar);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.lblGamesPlayed);
            this.Controls.Add(this.txtGamesPlayed);
            this.Controls.Add(this.lblWins);
            this.Controls.Add(this.txtWins);
            this.Controls.Add(this.lblLosses);
            this.Controls.Add(this.txtLosses);
            this.Controls.Add(this.lblWinRate);
            this.Controls.Add(this.txtWinRate);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.txtStatus);
            this.Controls.Add(this.btnEditInfo);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Information";
            this.Text = "Thông tin người chơi";
            ((System.ComponentModel.ISupportInitialize)(this.picAvatar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void SetTextBoxesReadOnly(bool readOnly)
        {
            txtName.ReadOnly = readOnly;
            txtGamesPlayed.ReadOnly = readOnly;
            txtWins.ReadOnly = readOnly;
            txtLosses.ReadOnly = readOnly;
            txtWinRate.ReadOnly = readOnly;
            txtStatus.ReadOnly = readOnly;
        }

        private void BtnEditAvatar_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "Chọn ảnh đại diện";
                ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        Image selectedImage = Image.FromFile(ofd.FileName);
                        picAvatar.Image = selectedImage;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Không thể tải ảnh: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void BtnEditInfo_Click(object sender, EventArgs e)
        {
            isEditing = !isEditing;
            SetTextBoxesReadOnly(!isEditing);
            btnEditInfo.Text = isEditing ? "Lưu thay đổi" : "Sửa đổi thông tin";

            if (!isEditing)
            {
                MessageBox.Show("Thông tin đã được lưu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // Save data logic here if needed
            }
        }
    }
}
