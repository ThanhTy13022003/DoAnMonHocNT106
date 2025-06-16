using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Firebase.Auth;


namespace DoAnMonHocNT106
{
    public partial class Login : Form
    {
        private string apiKey = "AIzaSyAtbgnNBlNDVe4tlvlXFf8lRVCeus8Dong";
        private FirebaseAuthProvider auth;

        public Login()
        {
            InitializeComponent();
            auth = new FirebaseAuthProvider(new FirebaseConfig(apiKey));
        }
        private void Password_Placeholder(object sender, EventArgs e)
        {
            TextBox tb = password; // Chỉ áp dụng cho password

            if (tb.Focused) // Khi nhập vào (Enter)
            {
                if (tb.Text == "Pass Word")
                {
                    tb.Text = "";
                    tb.ForeColor = Color.Black;
                }
            }
            else // Khi rời khỏi (Leave)
            {
                if (string.IsNullOrWhiteSpace(tb.Text))
                {
                    tb.Text = "Enter Password";
                    tb.ForeColor = Color.Gray;
                    tb.PasswordChar = '\0'; // Hiện lại chữ "Enter Password"
                }
            }
        }

        private void Login_Load(object sender, EventArgs e)
        {
            username.Text = "User Name Or Email";
            username.ForeColor = Color.Gray;

            password.Text = "Pass Word";
            password.ForeColor = Color.Gray;
            password.PasswordChar = '\0'; // Hiển thị dạng văn bản khi form mở

            username.GotFocus += Username_Placeholder;
            username.LostFocus += Username_Placeholder;

            password.GotFocus += Password_Placeholder;
            password.LostFocus += Password_Placeholder;

            // Thêm sự kiện KeyDown để nhấn Enter
            username.KeyDown += TextBox_KeyDown;
            password.KeyDown += TextBox_KeyDown;
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true; // Ngăn tiếng beep khi nhấn Enter
                signin_Click(sender, e); // Gọi hàm đăng nhập
            }
        }

        private void Username_Placeholder(object sender, EventArgs e)
        {
            TextBox tb = username; // Chỉ áp dụng cho username

            if (tb.Focused) // Khi nhập vào (Enter)
            {
                if (tb.Text == "User Name Or Email")
                {
                    tb.Text = "";
                    tb.ForeColor = Color.Black;
                }
            }
            else // Khi rời khỏi (Leave)
            {
                if (string.IsNullOrWhiteSpace(tb.Text))
                {
                    tb.Text = "User Name Or Email";
                    tb.ForeColor = Color.Gray;
                }
            }
        }

        private void show_CheckedChanged(object sender, EventArgs e)
        {
            password.UseSystemPasswordChar = !show.Checked;
        }

        private async void signin_Click(object sender, EventArgs e)
        {
            MusicPlayer.PlayClickSound();

            string input = username.Text.Trim();
            string passwordInput = password.Text.Trim();

            // Xác định email từ username hoặc email trực tiếp
            string email = IsValidEmail(input)
                ? input
                : await FirebaseHelper.GetEmailByUsername(input);

            if (email == null)
            {
                MessageBox.Show("Tài khoản không tồn tại hoặc sai định dạng!");
                return;
            }

            try
            {
                // 1) Lấy thông tin user từ Realtime DB
                var user = await FirebaseHelper.GetUserByUsername(input);
                if (user.IsOnline)
                {
                    MessageBox.Show("Tài khoản đang được đăng nhập ở nơi khác!", "Đã online", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 2) Xác thực email/password với FirebaseAuth
                var authResult = await auth.SignInWithEmailAndPasswordAsync(email, passwordInput);

                // 3) Đánh dấu user là online và cập nhật LastOnline
                await FirebaseHelper.SetUserOnlineStatus(user.Username, true);

                // 4) Lưu CurrentUsername để các form khác dùng chung
                FirebaseHelper.CurrentUsername = user.Username;

                // 5) Chuyển sang form chính (ví dụ Form1) và ẩn Login
                var mainForm = new Form1(user.Username);
                mainForm.Show();
                this.Hide();
            }
            catch (FirebaseAuthException ex)
            {
                MessageBox.Show("Đăng nhập thất bại: " + ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi xảy ra: " + ex.Message);
            }
        }


        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }


        private void ShowMessage(string message, Color color)
        {
            usererro.Text = message; // Cập nhật nội dung
            usererro.ForeColor = color; // Đổi màu chữ theo trạng thái
            usererro.Visible = true; // Hiển thị label

            // Tự động ẩn sau 3 giây
            Task.Delay(3000).ContinueWith(_ =>
            {
                if (usererro.InvokeRequired)
                {
                    usererro.Invoke(new Action(() => usererro.Visible = false));
                }
                else
                {
                    usererro.Visible = false;
                }
            });
        }

        private void BTsignup_Click(object sender, EventArgs e)
        {
            MusicPlayer.PlayClickSound();
            SignUp form2 = new SignUp();
            this.Hide();
            form2.Show();
        }

        private async void forgotpw_Click(object sender, EventArgs e)
        {
            MusicPlayer.PlayClickSound();

            string input = username.Text.Trim();
            string email;

            // 1) Xác định đây là email hay username
            if (IsValidEmail(input))
            {
                email = input;
            }
            else
            {
                // Lấy email đã đăng ký theo username
                email = await FirebaseHelper.GetEmailByUsername(input);
                if (email == null)
                {
                    ShowMessage("Không tìm thấy tài khoản này!", Color.Red);
                    return;
                }
            }

            // 2) Hiển thị thông báo đang xử lý, tự ẩn sau 3s
            ShowMessage("Đang gửi email đặt lại mật khẩu…", Color.Green);

            // 3) Chờ 3 giây để người dùng đọc thông báo
            await Task.Delay(3000);

            // 4) Thực sự gọi Firebase để gửi mail reset
            try
            {
                var authProvider = new FirebaseAuthProvider(new FirebaseConfig(apiKey));
                await authProvider.SendPasswordResetEmailAsync(email);
                ShowMessage("Email đặt lại mật khẩu đã được gửi!", Color.Green);
            }
            catch (FirebaseAuthException)
            {
                ShowMessage("Email không tồn tại trong hệ thống!", Color.Red);
            }
        }

    }
}
