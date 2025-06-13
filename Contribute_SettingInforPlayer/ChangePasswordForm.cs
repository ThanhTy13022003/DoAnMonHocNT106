using System;
using System.Drawing;
using System.Media;
using System.Windows.Forms;
using Firebase.Database;
using Firebase.Database.Query;

namespace GameCaro_SettingInforPlayer
{
    public partial class ChangePasswordForm : System.Windows.Forms.Form
    {
        private readonly string playerId;
        private readonly FirebaseClient firebaseClient;

        private TextBox txtOldPassword;
        private TextBox txtNewPassword;
        private TextBox txtConfirmNewPassword;
        private Button btnOK;
        private Button btnCancel;
        private Button btnShowPassword;

        // Placeholder text constants
        private const string OldPasswordPlaceholder = "Nhập mật khẩu cũ";
        private const string NewPasswordPlaceholder = "Nhập mật khẩu mới";
        private const string ConfirmNewPasswordPlaceholder = "Nhập lại mật khẩu mới";

        public ChangePasswordForm(string playerId, FirebaseClient firebaseClient)
        {
            this.playerId = playerId;
            this.firebaseClient = firebaseClient;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.txtOldPassword = new TextBox();
            this.txtNewPassword = new TextBox();
            this.txtConfirmNewPassword = new TextBox();
            this.btnOK = new Button();
            this.btnCancel = new Button();
            this.btnShowPassword = new Button();

            // txtOldPassword
            this.txtOldPassword.Location = new System.Drawing.Point(150, 20);
            this.txtOldPassword.Size = new System.Drawing.Size(200, 20);
            this.txtOldPassword.PasswordChar = '*';
            this.txtOldPassword.Text = OldPasswordPlaceholder;
            this.txtOldPassword.ForeColor = Color.Gray;
            this.txtOldPassword.Enter += (s, e) =>
            {
                if (txtOldPassword.Text == OldPasswordPlaceholder)
                {
                    txtOldPassword.Text = "";
                    txtOldPassword.ForeColor = Color.Black;
                }
            };
            this.txtOldPassword.Leave += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtOldPassword.Text))
                {
                    txtOldPassword.Text = OldPasswordPlaceholder;
                    txtOldPassword.ForeColor = Color.Gray;
                }
            };

            // txtNewPassword
            this.txtNewPassword.Location = new System.Drawing.Point(150, 50);
            this.txtNewPassword.Size = new System.Drawing.Size(200, 20);
            this.txtNewPassword.PasswordChar = '*';
            this.txtNewPassword.Text = NewPasswordPlaceholder;
            this.txtNewPassword.ForeColor = Color.Gray;
            this.txtNewPassword.Enter += (s, e) =>
            {
                if (txtNewPassword.Text == NewPasswordPlaceholder)
                {
                    txtNewPassword.Text = "";
                    txtNewPassword.ForeColor = Color.Black;
                }
            };
            this.txtNewPassword.Leave += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtNewPassword.Text))
                {
                    txtNewPassword.Text = NewPasswordPlaceholder;
                    txtNewPassword.ForeColor = Color.Gray;
                }
            };

            // txtConfirmNewPassword
            this.txtConfirmNewPassword.Location = new System.Drawing.Point(150, 80);
            this.txtConfirmNewPassword.Size = new System.Drawing.Size(200, 20);
            this.txtConfirmNewPassword.PasswordChar = '*';
            this.txtConfirmNewPassword.Text = ConfirmNewPasswordPlaceholder;
            this.txtConfirmNewPassword.ForeColor = Color.Gray;
            this.txtConfirmNewPassword.Enter += (s, e) =>
            {
                if (txtConfirmNewPassword.Text == ConfirmNewPasswordPlaceholder)
                {
                    txtConfirmNewPassword.Text = "";
                    txtConfirmNewPassword.ForeColor = Color.Black;
                }
            };
            this.txtConfirmNewPassword.Leave += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtConfirmNewPassword.Text))
                {
                    txtConfirmNewPassword.Text = ConfirmNewPasswordPlaceholder;
                    txtConfirmNewPassword.ForeColor = Color.Gray;
                }
            };

            // btnOK
            this.btnOK.Text = "OK";
            this.btnOK.Location = new System.Drawing.Point(150, 120);
            this.btnOK.Size = new System.Drawing.Size(75, 30);
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);

            // btnCancel
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Location = new System.Drawing.Point(250, 120);
            this.btnCancel.Size = new System.Drawing.Size(75, 30);
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);

            // btnShowPassword
            this.btnShowPassword.Text = "👁️";
            this.btnShowPassword.Location = new System.Drawing.Point(360, 50);
            this.btnShowPassword.Size = new System.Drawing.Size(30, 20);
            this.btnShowPassword.MouseDown += BtnShowPassword_MouseDown;
            this.btnShowPassword.MouseUp += BtnShowPassword_MouseUp;

            // Labels
            var lblOldPassword = new Label
            {
                Text = "Mật khẩu cũ:",
                Location = new System.Drawing.Point(20, 20),
                Size = new System.Drawing.Size(100, 20)
            };
            var lblNewPassword = new Label
            {
                Text = "Mật khẩu mới:",
                Location = new System.Drawing.Point(20, 50),
                Size = new System.Drawing.Size(100, 20)
            };
            var lblConfirmNewPassword = new Label
            {
                Text = "Xác nhận mật khẩu:",
                Location = new System.Drawing.Point(20, 80),
                Size = new System.Drawing.Size(100, 20)
            };

            // Form settings
            this.ClientSize = new System.Drawing.Size(410, 170);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Text = "Change Password";
            this.ShowInTaskbar = false; // Ẩn form khỏi taskbar
            this.ShowIcon = false; // Ẩn biểu tượng trên taskbar (tùy chọn)
            this.Controls.AddRange(new Control[] { txtOldPassword, txtNewPassword, txtConfirmNewPassword, btnOK, btnCancel, btnShowPassword, lblOldPassword, lblNewPassword, lblConfirmNewPassword });
        }

        private async void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                SoundPlayer player = new SoundPlayer("click-button-140881.wav");
                player.Play();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi phát âm thanh: " + ex.Message);
            }

            try
            {
                // Validate inputs, ignoring placeholder text
                string oldPassword = txtOldPassword.Text == OldPasswordPlaceholder ? "" : txtOldPassword.Text;
                string newPassword = txtNewPassword.Text == NewPasswordPlaceholder ? "" : txtNewPassword.Text;
                string confirmNewPassword = txtConfirmNewPassword.Text == ConfirmNewPasswordPlaceholder ? "" : txtConfirmNewPassword.Text;

                if (string.IsNullOrWhiteSpace(oldPassword) ||
                    string.IsNullOrWhiteSpace(newPassword) ||
                    string.IsNullOrWhiteSpace(confirmNewPassword))
                {
                    MessageBox.Show("Vui lòng điền đầy đủ thông tin!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (newPassword != confirmNewPassword)
                {
                    MessageBox.Show("Mật khẩu mới và xác nhận mật khẩu không khớp!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Validate password strength
                if (!System.Text.RegularExpressions.Regex.IsMatch(newPassword, @"^(?=.*\d)(?=.*[!@#$%^&*]).{8,}$"))
                {
                    MessageBox.Show("Mật khẩu mới phải có ít nhất 8 ký tự, bao gồm 1 số và 1 ký tự đặc biệt!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Check old password
                var playerData = await firebaseClient
                    .Child("Users")
                    .Child(playerId)
                    .OnceSingleAsync<Player>();

                if (playerData == null || playerData.Password != oldPassword)
                {
                    MessageBox.Show("Mật khẩu cũ không đúng!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Update password in Firebase with proper JSON structure
                var updateData = new { Password = newPassword };
                await firebaseClient
                    .Child("Users")
                    .Child(playerId)
                    .PatchAsync(updateData);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error changing password: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                SoundPlayer player = new SoundPlayer("click-button-140881.wav");
                player.Play();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi phát âm thanh: " + ex.Message);
            }
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void BtnShowPassword_MouseDown(object sender, MouseEventArgs e)
        {
            txtOldPassword.PasswordChar = '\0';
            txtNewPassword.PasswordChar = '\0';
            txtConfirmNewPassword.PasswordChar = '\0';
        }

        private void BtnShowPassword_MouseUp(object sender, MouseEventArgs e)
        {
            txtOldPassword.PasswordChar = '*';
            txtNewPassword.PasswordChar = '*';
            txtConfirmNewPassword.PasswordChar = '*';
        }
    }
}