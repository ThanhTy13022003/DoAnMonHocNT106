// FormSetting.cs
// Form cài đặt tùy chọn người dùng: bật/tắt nhạc nền và âm thanh game,
// điều chỉnh âm lượng, mở các form giới thiệu, thông tin người chơi và bảng xếp hạng.

using System;
using System.Windows.Forms;

namespace DoAnMonHocNT106
{
    /// <summary>
    /// FormSetting cho phép người dùng cấu hình:
    /// - Nhạc nền (bật/tắt, điều chỉnh âm lượng)
    /// - Âm thanh game (bật/tắt, điều chỉnh âm lượng)
    /// - Hiển thị form giới thiệu, thông tin người chơi và bảng xếp hạng
    /// Đồng thời hỗ trợ phím ESC để đóng form.
    /// </summary>
    public partial class FormSetting : Form
    {
        // Biến lưu username hiện tại để truyền vào các form con
        private readonly string currentUser;

        /// <summary>
        /// Khởi tạo FormSetting với username của người dùng
        /// </summary>
        public FormSetting(string currentUser)
        {
            InitializeComponent();
            this.KeyPreview = true; // Cho phép form nhận sự kiện phím trước các control
            this.KeyDown += FormSetting_KeyDown;
            this.currentUser = currentUser;
        }

        /// <summary>
        /// Thiết lập trạng thái của nút và thanh trượt khi form load
        /// </summary>
        private void FormSetting_Load(object sender, EventArgs e)
        {
            // Cập nhật nhãn nút dựa trên trạng thái hiện tại
            btnToggleMusic.Text = MusicPlayer.IsMusicPlaying() ? "Tắt Nhạc Nền" : "Bật Nhạc Nền";
            btnToggleSound.Text = MusicPlayer.IsSoundEnabled() ? "Tắt Âm Thanh Game" : "Bật Âm Thanh Game";

            // Khởi tạo giá trị cho thanh trượt âm lượng
            trackBarMusicVolume.Value = Math.Min((int)(MusicPlayer.GetVolume() * 70), 70);
            trackBarSoundVolume.Value = Math.Min((int)(MusicPlayer.GetSoundVolume() * 70), 70);
        }

        /// <summary>
        /// Nhấn ESC đóng form và phát âm thanh click
        /// </summary>
        private void FormSetting_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                MusicPlayer.PlayClickSound();
                this.Close();
            }
        }

        /// <summary>
        /// Mở form giới thiệu (Introduce) dưới dạng dialog
        /// </summary>
        private void btnIntroducing_Click(object sender, EventArgs e)
        {
            MusicPlayer.PlayClickSound();
            Form_Introduce introduceForm = new Form_Introduce(currentUser);
            introduceForm.ShowDialog();
        }

        /// <summary>
        /// Bật/tắt nhạc nền, cập nhật nhãn và trạng thái thanh trượt
        /// </summary>
        private void btnToggleMusic_Click(object sender, EventArgs e)
        {
            MusicPlayer.PlayClickSound();
            MusicPlayer.ToggleMusic();
            btnToggleMusic.Text = MusicPlayer.IsMusicPlaying() ? "Tắt Nhạc Nền" : "Bật Nhạc Nền";
            trackBarMusicVolume.Enabled = MusicPlayer.IsMusicPlaying();
        }

        /// <summary>
        /// Mở form thông tin người chơi dưới dạng dialog
        /// </summary>
        private void btnPlayerInfo_Click(object sender, EventArgs e)
        {
            MusicPlayer.PlayClickSound();
            Form_PlayerInfo playerInfoForm = new Form_PlayerInfo(currentUser);
            playerInfoForm.ShowDialog();
        }

        /// <summary>
        /// Mở form bảng xếp hạng dưới dạng dialog
        /// </summary>
        private void btnLeaderboard_Click(object sender, EventArgs e)
        {
            MusicPlayer.PlayClickSound();
            Form_Leaderboard leaderboardForm = new Form_Leaderboard();
            leaderboardForm.ShowDialog();
        }

        /// <summary>
        /// Bật/tắt âm thanh game, cập nhật nhãn và trạng thái thanh trượt
        /// </summary>
        private void btnToggleSound_Click(object sender, EventArgs e)
        {
            MusicPlayer.PlayClickSound();
            MusicPlayer.SetSoundEnabled(!MusicPlayer.IsSoundEnabled());
            btnToggleSound.Text = MusicPlayer.IsSoundEnabled() ? "Tắt Âm Thanh Game" : "Bật Âm Thanh Game";
            trackBarSoundVolume.Enabled = MusicPlayer.IsSoundEnabled();
        }

        /// <summary>
        /// Xử lý thay đổi âm lượng nhạc nền khi thanh trượt được kéo
        /// </summary>
        private void trackBarMusicVolume_Scroll(object sender, EventArgs e)
        {
            int roundedValue = (int)Math.Round(trackBarMusicVolume.Value / 10.0) * 10;
            trackBarMusicVolume.Value = roundedValue;
            MusicPlayer.SetVolume(roundedValue / 70f);
        }

        /// <summary>
        /// Xử lý thay đổi âm lượng âm thanh game khi thanh trượt được kéo
        /// </summary>
        private void trackBarSoundVolume_Scroll(object sender, EventArgs e)
        {
            int roundedValue = (int)Math.Round(trackBarSoundVolume.Value / 10.0) * 10;
            if (trackBarSoundVolume.Value != roundedValue)
                MusicPlayer.PlayClickSound();
            trackBarSoundVolume.Value = roundedValue;
            MusicPlayer.SetSoundVolume(roundedValue / 70f);
        }
    }
}
