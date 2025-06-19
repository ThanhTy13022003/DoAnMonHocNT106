// Form_ChangeInfo.cs
// Xử lý logic cho form thay đổi thông tin người dùng:
// Bao gồm các chức năng thay đổi tên người dùng và mật khẩu,
// hiển thị placeholder cho các ô nhập liệu, kiểm tra thông tin có thay đổi,
// và truyền dữ liệu mới về form gọi sau khi xác nhận thành công.
using System;
using System.Drawing;
using System.Windows.Forms;

namespace DoAnMonHocNT106
{
    public partial class Form_ChangeInfo : Form
    {
        // Lưu trữ thông tin tên người dùng và mật khẩu hiện tại (trước khi thay đổi)
        private readonly string oldUsername;
        private readonly string oldPassword;

        // Thuộc tính chứa thông tin mới sau khi thay đổi
        public string NewUsername { get; private set; }
        public string NewPassword { get; private set; }

        // Khởi tạo form và gán thông tin cũ từ người dùng hiện tại
        public Form_ChangeInfo(string currentUsername, string currentPassword)
        {
            InitializeComponent();
            oldUsername = currentUsername;
            oldPassword = currentPassword;
            SetupPlaceholderText(); // Thiết lập placeholder cho các ô nhập liệu
        }

        // Thiết lập văn bản gợi ý (placeholder) cho các ô nhập tên và mật khẩu
        private void SetupPlaceholderText()
        {
            // Thiết lập placeholder cho ô nhập tên người dùng
            txtNewUsername.Text = "Tên người dùng mới";
            txtNewUsername.ForeColor = Color.Gray;

            // Khi ô được focus, xóa placeholder nếu chưa có nội dung
            txtNewUsername.GotFocus += (s, e) =>
            {
                if (txtNewUsername.ForeColor == Color.Gray)
                {
                    txtNewUsername.Text = string.Empty;
                    txtNewUsername.ForeColor = Color.White;
                }
            };

            // Khi ô mất focus, hiển thị lại placeholder nếu trống
            txtNewUsername.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtNewUsername.Text))
                {
                    txtNewUsername.Text = "Tên người dùng mới";
                    txtNewUsername.ForeColor = Color.Gray;
                }
            };

            // Thiết lập placeholder cho ô nhập mật khẩu
            txtNewPassword.Text = "Mật khẩu mới";
            txtNewPassword.ForeColor = Color.Gray;
            txtNewPassword.UseSystemPasswordChar = false;

            // Khi bắt đầu nhập mật khẩu, xóa placeholder và bật ẩn ký tự
            txtNewPassword.Enter += (s, e) =>
            {
                if (txtNewPassword.ForeColor == Color.Gray)
                {
                    txtNewPassword.Text = string.Empty;
                    txtNewPassword.ForeColor = Color.White;
                    txtNewPassword.UseSystemPasswordChar = true;
                }
            };

            // Khi mất focus, hiển thị lại placeholder nếu trống và tắt ẩn ký tự
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

        // Sự kiện khi người dùng nhấn nút xác nhận thay đổi
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            MusicPlayer.PlayClickSound(); // Phát âm thanh khi nhấn nút

            // Lấy giá trị người dùng nhập, bỏ qua nếu còn placeholder
            string inputUsername = txtNewUsername.ForeColor == Color.Gray ? null : txtNewUsername.Text.Trim();
            string inputPassword = txtNewPassword.ForeColor == Color.Gray ? null : txtNewPassword.Text.Trim();

            // Nếu người dùng không thay đổi gì
            if (string.IsNullOrEmpty(inputUsername) && string.IsNullOrEmpty(inputPassword))
            {
                MessageBox.Show(
                    "Bạn chưa thay đổi tên người dùng hoặc mật khẩu.",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error
                );
                return;
            }

            // Gán giá trị mới, nếu không thay đổi thì giữ nguyên
            NewUsername = !string.IsNullOrEmpty(inputUsername) ? inputUsername : oldUsername;
            NewPassword = !string.IsNullOrEmpty(inputPassword) ? inputPassword : oldPassword;

            // Trả về kết quả thành công và đóng form
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
