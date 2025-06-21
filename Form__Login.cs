// Login.cs
// Form xử lý đăng nhập người dùng, placeholder, xác thực Firebase và điều hướng đến form chính

using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Firebase.Auth;

namespace DoAnMonHocNT106
{
    /// <summary>
    /// Form đăng nhập: xử lý placeholder, nhập liệu, xác thực bằng FirebaseAuth và điều hướng sau đăng nhập thành công.
    /// </summary>
    public partial class Login : Form
    {
        // Khóa API Firebase
        private string apiKey = "AIzaSyAtbgnNBlNDVe4tlvlXFf8lRVCeus8Dong";
        private FirebaseAuthProvider auth;

        public Login()
        {
            InitializeComponent();
            auth = new FirebaseAuthProvider(new FirebaseConfig(apiKey));
        }

        /// <summary>
        /// Xử lý placeholder cho ô password
        /// </summary>
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

        /// <summary>
        /// Thiết lập placeholder và sự kiện cho username và password khi form load
        /// </summary>
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

        /// <summary>
        /// Gọi hàm đăng nhập khi nhấn Enter
        /// </summary>
        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true; // Ngăn tiếng beep khi nhấn Enter
                signin_Click(sender, e); // Gọi hàm đăng nhập
            }
        }

        /// <summary>
        /// Xử lý placeholder cho ô username
        /// </summary>
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

        /// <summary>
        /// Hiển thị mật khẩu chữ thường hoặc dấu sao
        /// </summary>
        private void show_CheckedChanged(object sender, EventArgs e)
        {
            password.UseSystemPasswordChar = !show.Checked;
        }

        /// <summary>
        /// Xử lý sự kiện click đăng nhập: xác thực Firebase, kiểm tra trạng thái online và chuyển form
        /// </summary>
        private async void signin_Click(object sender, EventArgs e)
        {
            signin.Enabled = false; // Disable nút để chặn click chồng
            MusicPlayer.PlayClickSound(); // Phát âm thanh click

            try
            {
                string input = username.Text.Trim();
                string passwordInput = password.Text.Trim();

                // Xác định email từ username hoặc email trực tiếp
                string email = IsValidEmail(input)
                    ? input
                    : await FirebaseHelper.GetEmailByUsername(input);

                if (email == null)
                {
                    MessageBox.Show("Tài khoản không tồn tại hoặc sai định dạng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Kiểm tra thông tin user và trạng thái online
                var user = await FirebaseHelper.GetUserByUsername(input);
                if (user == null)
                {
                    MessageBox.Show("Không tìm thấy thông tin người dùng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (user.IsOnline)
                {
                    MessageBox.Show("Tài khoản đang được đăng nhập ở nơi khác!", "Đã online", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Xác thực với FirebaseAuth
                var authResult = await auth.SignInWithEmailAndPasswordAsync(email, passwordInput);

                // Đánh dấu online và lưu username hiện tại
                await FirebaseHelper.SetUserOnlineStatus(user.Username, true);
                FirebaseHelper.CurrentUsername = user.Username;

                // Mở form chính và ẩn Login
                var mainForm = new Form1(user.Username);
                mainForm.Show();
                this.Hide();
            }
            catch (FirebaseAuthException ex)
            {
                MessageBox.Show("Đăng nhập thất bại: " + ex.Message, "Lỗi xác thực", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi xảy ra: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (this.Visible)
                    signin.Enabled = true; // Re-enable nếu vẫn còn form
            }
        }

        /// <summary>
        /// Kiểm tra định dạng email hợp lệ
        /// </summary>
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

        /// <summary>
        /// Hiển thị thông báo tạm thời ngay trên form
        /// </summary>
        private void ShowMessage(string message, Color color)
        {
            usererro.Text = message;
            usererro.ForeColor = color;
            usererro.Visible = true;

            // Tự động ẩn sau 3 giây
            Task.Delay(3000).ContinueWith(_ =>
            {
                if (usererro.InvokeRequired)
                    usererro.Invoke(new Action(() => usererro.Visible = false));
                else
                    usererro.Visible = false;
            });
        }

        /// <summary>
        /// Chuyển đến form SignUp
        /// </summary>
        private void BTsignup_Click(object sender, EventArgs e)
        {
            MusicPlayer.PlayClickSound();
            SignUp form2 = new SignUp();
            this.Hide();
            form2.Show();
        }

        /// <summary>
        /// Xử lý quên mật khẩu: gửi email reset qua Firebase
        /// </summary>
        private async void forgotpw_Click(object sender, EventArgs e)
        {
            MusicPlayer.PlayClickSound();

            string input = username.Text.Trim();
            string email;

            // Xác định email hoặc lấy từ username
            if (IsValidEmail(input))
            {
                email = input;
            }
            else
            {
                email = await FirebaseHelper.GetEmailByUsername(input);
                if (email == null)
                {
                    ShowMessage("Không tìm thấy tài khoản này!", Color.Red);
                    return;
                }
            }

            ShowMessage("Đang gửi email đặt lại mật khẩu…", Color.Green);
            await Task.Delay(3000); // Đợi người dùng đọc thông báo

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
