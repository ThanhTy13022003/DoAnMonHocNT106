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
            auth = new FirebaseAuthProvider(new FirebaseConfig("AIzaSy..."));
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
            string input = username.Text; // Người dùng nhập username hoặc email
            string Password = this.password.Text;

            // Kiểm tra nếu nhập username thì lấy email tương ứng từ Firebase
            string email = input;
            if (!input.Contains("@"))
            {
                email = await FirebaseHelper.GetEmailByUsername(input);
                if (email == null)
                {
                    ShowMessage("Sai tài khoản hoặc mật khẩu!", Color.Red);
                    return;
                }
            }

            try
            {
                var authProvider = new FirebaseAuthProvider(new FirebaseConfig(apiKey));
                var auth = await authProvider.SignInWithEmailAndPasswordAsync(email, Password);

                string userName = input.Contains("@") ? email.Split('@')[0] : input;

                Properties.Settings.Default["UserId"] = userName;
                Properties.Settings.Default.Save();

                await FirebaseHelper.SetUserOnlineStatus(userName, true);

                Form1 mainForm = new Form1();
                mainForm.Show();
                this.Hide();
            }
            catch (Exception)
            {
                ShowMessage("Sai tài khoản hoặc mật khẩu!", Color.Red);
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
            SignUp form2 = new SignUp();
            this.Hide();
            form2.Show();
        }

        private async void forgotpw_Click(object sender, EventArgs e)
        {
            string email = username.Text.Trim(); // Lấy email từ textbox

            if (string.IsNullOrWhiteSpace(email) || !email.Contains("@"))
            {
                ShowMessage("Vui lòng nhập email hợp lệ!", Color.Red);
                return;
            }

            try
            {
                var authProvider = new FirebaseAuthProvider(new FirebaseConfig(apiKey));
                // Thử gửi email reset mật khẩu
                await authProvider.SendPasswordResetEmailAsync(email);
                ShowMessage("Email đặt lại mật khẩu đã được gửi!", Color.Green);
            }
            catch (FirebaseAuthException)
            {
                // Firebase sẽ trả lỗi nếu email không tồn tại
                ShowMessage("Email không tồn tại trong hệ thống!", Color.Red);
            }
        }
    }
}
