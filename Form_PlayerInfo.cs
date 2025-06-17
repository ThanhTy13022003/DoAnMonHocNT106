using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DoAnMonHocNT106
{
    public partial class Form_PlayerInfo : Form
    {
        private string currentUser;
        private bool isPasswordVisible = false;
        private string userPassword;
        private IDisposable passwordSubscription;

        public Form_PlayerInfo(string username)
        {
            InitializeComponent();
            currentUser = username;
            this.KeyPreview = true;
            this.KeyDown += FormPlayerInfo_KeyDown;
            LoadPlayerInfo(username);
            SubscribePasswordChanges(username);
        }

        private void SubscribePasswordChanges(string username)
        {
            passwordSubscription?.Dispose();

            try
            {
                passwordSubscription = FirebaseHelper.SubscribeUserChanges(username, user =>
                {
                    if (user == null) return;

                    userPassword = user.Password;
                    if (isPasswordVisible)
                    {
                        lblPassword.Invoke(new Action(() =>
                            lblPassword.Text = $"Mật khẩu: {userPassword}"
                        ));
                    }
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi đăng ký lắng nghe thay đổi mật khẩu: {ex.Message}");
            }
        }

        private async void LoadPlayerInfo(string username)
        {
            try
            {
                var user = await FirebaseHelper.GetUserByUsername(username);
                if (user == null)
                {
                    MessageBox.Show("Không tìm thấy thông tin người dùng.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                lblUsername.Text = $"Tên người dùng: {user.Username}";
                lblEmail.Text = $"Email: {user.Email}";
                lblLastOnline.Text = $"Lần cuối online: {user.LastOnline}";
                lblStats.Text = $"Thắng: {user.Wins} | Thua: {user.Losses} | Hòa: {user.Draws}";
                userPassword = user.Password;
                lblPassword.Text = "Mật khẩu: ********";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải thông tin: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormPlayerInfo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                MusicPlayer.PlayClickSound();
                this.Close();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            MusicPlayer.PlayClickSound();
            this.Close();
        }

        private void btnTogglePassword_Click(object sender, EventArgs e)
        {
            MusicPlayer.PlayClickSound();
            if (isPasswordVisible)
            {
                lblPassword.Text = "Mật khẩu: ********";
                //btnTogglePassword.BackgroundImage = Properties.Resources.eye_open;
            }
            else
            {
                lblPassword.Text = $"Mật khẩu: {userPassword}";
                //btnTogglePassword.BackgroundImage = Properties.Resources.eye_close;
            }
            isPasswordVisible = !isPasswordVisible;
        }

        private async void btnChangeInfo_Click(object sender, EventArgs e)
        {
            MusicPlayer.PlayClickSound();
            Form_ChangeInfo changeInfoForm = new Form_ChangeInfo(currentUser, userPassword);
            if (changeInfoForm.ShowDialog() != DialogResult.OK) return;

            string newUsername = changeInfoForm.NewUsername;
            string newPassword = changeInfoForm.NewPassword;

            if (string.IsNullOrWhiteSpace(newUsername) && string.IsNullOrWhiteSpace(newPassword))
            {
                MessageBox.Show("Vui lòng nhập ít nhất một thông tin cần thay đổi!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var oldUsername = currentUser;
                await FirebaseHelper.UpdateUserCredentials(oldUsername,
                    !string.IsNullOrWhiteSpace(newUsername) ? newUsername : oldUsername,
                    !string.IsNullOrWhiteSpace(newPassword) ? newPassword : userPassword);

                currentUser = !string.IsNullOrWhiteSpace(newUsername) ? newUsername : oldUsername;
                userPassword = !string.IsNullOrWhiteSpace(newPassword) ? newPassword : userPassword;

                if (isPasswordVisible)
                {
                    lblPassword.Text = $"Mật khẩu: {userPassword}";
                }

                SubscribePasswordChanges(currentUser);
                lblUsername.Text = $"Tên người dùng: {currentUser}";
                MessageBox.Show("Thông tin đã được cập nhật thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật thông tin: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            passwordSubscription?.Dispose();
            base.OnFormClosing(e);
        }
    }
}