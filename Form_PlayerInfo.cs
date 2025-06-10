using Firebase.Database.Query;
using System;
using System.Windows.Forms;
using Firebase.Database;

namespace DoAnMonHocNT106
{
    public partial class Form_PlayerInfo : Form
    {
        private string currentUser;
        private bool isPasswordVisible = false;
        private string userPassword;
        private FirebaseClient firebase;
        private IDisposable passwordSubscription;

        public Form_PlayerInfo(string username)
        {
            InitializeComponent();
            currentUser = username;
            this.KeyPreview = true; // Cho phép form nhận sự kiện phím
            this.KeyDown += FormPlayerInfo_KeyDown; // Gắn sự kiện KeyDown
            LoadPlayerInfo(username);
            firebase = new FirebaseClient("https://nt106-7c9fe-default-rtdb.firebaseio.com/");
            LoadPlayerInfo(username);
            SubscribePasswordChanges(username);
        }

        private void SubscribePasswordChanges(string username)
        {
            passwordSubscription?.Dispose(); // Huỷ listener cũ nếu có

            passwordSubscription = firebase
                .Child("Users")
                .Child(username)
                .AsObservable<User>()
                .Subscribe(ev =>
                {
                    if (ev.Object != null)
                    {
                        userPassword = ev.Object.Password;

                        if (isPasswordVisible)
                        {
                            lblPassword.Invoke(new Action(() =>
                                lblPassword.Text = $"Mật khẩu: {userPassword}"
                            ));
                        }
                    }
                });
        }


        private async void LoadPlayerInfo(string username)
        {
            try
            {
                var user = await FirebaseHelper.GetUserByUsername(username);
                lblUsername.Text = $"Tên người dùng: {user?.Username}";
                lblEmail.Text = $"Email: {user?.Email}";
                lblLastOnline.Text = $"Lần cuối online: {user?.LastOnline}";
                var stats = await FirebaseHelper.GetStats(username);
                lblStats.Text = $"Thắng: {stats.Wins} | Thua: {stats.Losses} | Hết thời gian: {stats.Timeouts}";
                userPassword = user?.Password; // Lưu mật khẩu
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
                MessageBox.Show("Vui lòng nhập ít nhất một thông tin cần thay đổi!");
                return;
            }

            try
            {
                // 1) Lưu tên cũ
                var oldUsername = currentUser;

                // 2) Lấy đối tượng user
                var user = await FirebaseHelper.GetUserByUsername(oldUsername);
                if (user == null) throw new Exception("Không tìm thấy người dùng.");

                // 3) Áp dụng thay đổi lên đối tượng
                if (!string.IsNullOrWhiteSpace(newUsername))
                {
                    user.Username = newUsername;
                }
                if (!string.IsNullOrWhiteSpace(newPassword))
                {
                    user.Password = newPassword;
                }

                // 4) Xác định key mới để Put
                var targetKey = !string.IsNullOrWhiteSpace(newUsername) ? newUsername : oldUsername;

                // 5) Ghi dữ liệu lên node targetKey
                await FirebaseHelper.firebase
                    .Child("Users")
                    .Child(targetKey)
                    .PutAsync(user);

                // 👉 Cập nhật biến nội bộ ngay sau khi push
                userPassword = user.Password;
                if (isPasswordVisible)
                {
                    lblPassword.Text = $"Mật khẩu: {userPassword}";
                }

                // 6) Nếu đã đổi tên, xoá node cũ
                if (!string.IsNullOrWhiteSpace(newUsername) && oldUsername != targetKey)
                {
                    await FirebaseHelper.firebase
                        .Child("Users")
                        .Child(oldUsername)
                        .DeleteAsync();
                }

                // 7) Cập nhật biến currentUser và UI
                currentUser = targetKey;
                SubscribePasswordChanges(currentUser); // Cập nhật listener theo username mới
                lblUsername.Text = $"Tên người dùng: {currentUser}";
                MessageBox.Show("Thông tin đã được cập nhật thành công!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật thông tin: {ex.Message}");
            }
        }
    }
}