using System;
using System.Drawing;
using System.Windows.Forms;

namespace DoAnMonHocNT106
{
    public partial class Form_ChangeInfo : Form
    {
        private readonly string oldUsername;
        private readonly string oldPassword;

        public string NewUsername { get; private set; }
        public string NewPassword { get; private set; }

        // Cập nhật constructor để nhận giá trị cũ
        public Form_ChangeInfo(string currentUsername, string currentPassword)
        {
            InitializeComponent();
            oldUsername = currentUsername;
            oldPassword = currentPassword;
            SetupPlaceholderText();
        }

        private void SetupPlaceholderText()
        {
            // Username placeholder
            txtNewUsername.Text = "Tên người dùng mới";
            txtNewUsername.ForeColor = Color.Gray;
            txtNewUsername.GotFocus += (s, e) =>
            {
                if (txtNewUsername.ForeColor == Color.Gray)
                {
                    txtNewUsername.Text = string.Empty;
                    txtNewUsername.ForeColor = Color.White;
                }
            };
            txtNewUsername.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtNewUsername.Text))
                {
                    txtNewUsername.Text = "Tên người dùng mới";
                    txtNewUsername.ForeColor = Color.Gray;
                }
            };

            // Password placeholder
            txtNewPassword.Text = "Mật khẩu mới";
            txtNewPassword.ForeColor = Color.Gray;
            txtNewPassword.UseSystemPasswordChar = false;
            txtNewPassword.Enter += (s, e) =>
            {
                if (txtNewPassword.ForeColor == Color.Gray)
                {
                    txtNewPassword.Text = string.Empty;
                    txtNewPassword.ForeColor = Color.White;
                    txtNewPassword.UseSystemPasswordChar = true;
                }
            };
            txtNewPassword.Leave += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtNewPassword.Text))
                {
                    txtNewPassword.UseSystemPasswordChar = false;
                    txtNewPassword.Text = "Mật khẩu mới";
                    txtNewPassword.ForeColor = Color.Gray;
                }
            };
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            MusicPlayer.PlayClickSound();

            // Lấy giá trị người dùng nhập
            string inputUsername = txtNewUsername.ForeColor == Color.Gray ? null : txtNewUsername.Text.Trim();
            string inputPassword = txtNewPassword.ForeColor == Color.Gray ? null : txtNewPassword.Text.Trim();

            // Nếu không có gì thay đổi
            if (string.IsNullOrEmpty(inputUsername) && string.IsNullOrEmpty(inputPassword))
            {
                MessageBox.Show(
                    "Bạn chưa thay đổi tên người dùng hoặc mật khẩu.",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error
                );
                return;
            }

            // Thiết lập giá trị cuối cùng (giữ nguyên nếu không thay đổi)
            NewUsername = !string.IsNullOrEmpty(inputUsername) ? inputUsername : oldUsername;
            NewPassword = !string.IsNullOrEmpty(inputPassword) ? inputPassword : oldPassword;

            // Xác nhận thành công
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
