using System;
using System.Windows.Forms;

namespace DoAnMonHocNT106
{
    public partial class Form_PlayerInfo : Form
    {
        private string currentUser;

        public Form_PlayerInfo(string username)
        {
            InitializeComponent();
            currentUser = username;
            this.KeyPreview = true; // Cho phép form nhận sự kiện phím
            this.KeyDown += FormPlayerInfo_KeyDown; // Gắn sự kiện KeyDown
            LoadPlayerInfo();
        }

        private async void LoadPlayerInfo()
        {
            try
            {
                // Lấy thông tin người dùng từ Firebase
                var user = await FirebaseHelper.GetUserByUsername(currentUser);
                if (user != null)
                {
                    lblUsername.Text = $"Tên người dùng: {user.Username}";
                    lblEmail.Text = $"Email: {user.Email}";
                    lblLastOnline.Text = $"Lần cuối online: {user.LastOnline:dd/MM/yyyy HH:mm:ss}";
                }

                // Lấy thống kê thắng/thua
                var stats = await FirebaseHelper.GetStats(currentUser);
                lblStats.Text = $"Thắng: {stats.Wins} | Thua: {stats.Losses} | Hết thời gian: {stats.Timeouts}";
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
    }
}