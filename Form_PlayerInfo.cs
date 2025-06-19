// Form_PlayerInfo.cs
// Xử lý logic cho form thông tin người chơi (Player Info):
// Bao gồm các chức năng lấy thông tin người dùng từ Firebase Realtime Database,
// hiển thị tên, email, lần cuối online, thống kê thắng/thua/hòa,
// ẩn/hiện mật khẩu, đăng ký lắng nghe thay đổi mật khẩu, cập nhật thông tin,
// xử lý phím ESC và nút đóng form.

using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DoAnMonHocNT106
{
    public partial class Form_PlayerInfo : Form
    {
        private string currentUser;               // Tên người dùng hiện tại
        private bool isPasswordVisible = false;   // Trạng thái hiển thị mật khẩu
        private string userPassword;              // Lưu mật khẩu người dùng
        private IDisposable passwordSubscription; // Subscription để lắng nghe thay đổi mật khẩu

        /// <summary>
        /// Constructor: khởi tạo component, thiết lập sự kiện bàn phím và nạp thông tin người chơi
        /// </summary>
        /// <param name="username">Tên người dùng cần hiển thị thông tin</param>
        public Form_PlayerInfo(string username)
        {
            InitializeComponent();
            currentUser = username;

            // Cho phép form nhận sự kiện bàn phím (ESC)
            this.KeyPreview = true;
            this.KeyDown += FormPlayerInfo_KeyDown;

            // Tải thông tin người chơi và đăng ký lắng nghe thay đổi mật khẩu
            LoadPlayerInfo(username);
            SubscribePasswordChanges(username);
        }

        /// <summary>
        /// Đăng ký lắng nghe sự kiện thay đổi mật khẩu của người dùng trên Firebase
        /// </summary>
        /// <param name="username">Tên người dùng</param>
        private void SubscribePasswordChanges(string username)
        {
            // Hủy subscription cũ nếu tồn tại
            passwordSubscription?.Dispose();

            try
            {
                // Đăng ký lắng nghe thay đổi user và cập nhật mật khẩu khi cần
                passwordSubscription = FirebaseHelper.SubscribeUserChanges(username, user =>
                {
                    if (user == null) return;

                    userPassword = user.Password;
                    if (isPasswordVisible)
                    {
                        // Cập nhật lblPassword trên UI thread
                        lblPassword.Invoke(new Action(() =>
                            lblPassword.Text = $"Mật khẩu: {userPassword}"
                        ));
                    }
                });
            }
            catch (Exception ex)
            {
                // In lỗi ra console để debug
                Console.WriteLine($"Lỗi khi đăng ký lắng nghe thay đổi mật khẩu: {ex.Message}");
            }
        }

        /// <summary>
        /// Nạp và hiển thị thông tin người chơi từ Firebase
        /// </summary>
        /// <param name="username">Tên người dùng</param>
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

                // Gán dữ liệu xuống các Label
                lblUsername.Text = $"Tên người dùng: {user.Username}";
                lblEmail.Text = $"Email: {user.Email}";
                lblLastOnline.Text = $"Lần cuối online: {user.LastOnline}";
                lblStats.Text = $"Thắng: {user.Wins} | Thua: {user.Losses} | Hòa: {user.Draws}";

                // Lưu mật khẩu và ẩn hiển thị
                userPassword = user.Password;
                lblPassword.Text = "Mật khẩu: ********";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải thông tin: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Xử lý sự kiện phím: ESC để đóng form
        /// </summary>
        private void FormPlayerInfo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                MusicPlayer.PlayClickSound();
                this.Close();
            }
        }

        /// <summary>
        /// Xử lý sự kiện nhấn nút Đóng form
        /// </summary>
        private void btnClose_Click(object sender, EventArgs e)
        {
            MusicPlayer.PlayClickSound();
            this.Close();
        }

        /// <summary>
        /// Xử lý nhấn nút ẩn/hiện mật khẩu
        /// </summary>
        private void btnTogglePassword_Click(object sender, EventArgs e)
        {
            MusicPlayer.PlayClickSound();
            if (isPasswordVisible)
            {
                lblPassword.Text = "Mật khẩu: ********";
                // Thay đổi icon nếu cần
            }
            else
            {
                lblPassword.Text = $"Mật khẩu: {userPassword}";
                // Thay đổi icon nếu cần
            }
            isPasswordVisible = !isPasswordVisible;
        }

        /// <summary>
        /// Xử lý nhấn nút thay đổi thông tin (mở form ChangeInfo)
        /// </summary>
        private async void btnChangeInfo_Click(object sender, EventArgs e)
        {
            MusicPlayer.PlayClickSound();
            var changeInfoForm = new Form_ChangeInfo(currentUser, userPassword);
            if (changeInfoForm.ShowDialog() != DialogResult.OK) return;

            string newUsername = changeInfoForm.NewUsername;
            string newPassword = changeInfoForm.NewPassword;

            // Kiểm tra nhập ít nhất 1 trường cần đổi
            if (string.IsNullOrWhiteSpace(newUsername) && string.IsNullOrWhiteSpace(newPassword))
            {
                MessageBox.Show("Vui lòng nhập ít nhất một thông tin cần thay đổi!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Cập nhật dữ liệu trên Firebase
                await FirebaseHelper.UpdateUserCredentials(
                    currentUser,
                    !string.IsNullOrWhiteSpace(newUsername) ? newUsername : currentUser,
                    !string.IsNullOrWhiteSpace(newPassword) ? newPassword : userPassword
                );

                // Cập nhật biến local
                currentUser = string.IsNullOrWhiteSpace(newUsername) ? currentUser : newUsername;
                userPassword = string.IsNullOrWhiteSpace(newPassword) ? userPassword : newPassword;

                if (isPasswordVisible)
                    lblPassword.Text = $"Mật khẩu: {userPassword}";

                // Đăng ký lại listener với username mới
                SubscribePasswordChanges(currentUser);
                lblUsername.Text = $"Tên người dùng: {currentUser}";

                MessageBox.Show("Thông tin đã được cập nhật thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật thông tin: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Hủy subscription khi form đóng
        /// </summary>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            passwordSubscription?.Dispose();
            base.OnFormClosing(e);
        }
    }
}
