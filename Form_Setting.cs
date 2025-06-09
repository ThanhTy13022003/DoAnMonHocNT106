using System;
using System.Windows.Forms;

namespace DoAnMonHocNT106
{
    public partial class FormSetting : Form
    {
        public FormSetting()
        {
            InitializeComponent();
            this.KeyPreview = true; // Cho phép form nhận sự kiện phím trước các control
            this.KeyDown += FormSetting_KeyDown; // Gắn sự kiện KeyDown
        }

        private void btnChangeLanguage_Click(object sender, EventArgs e)
        {
            MusicPlayer.PlayClickSound();
            // Danh sách ngôn ngữ hỗ trợ
            string[] languages = { "Tiếng Anh", "Tiếng Việt", "Tiếng Trung Quốc", "Tiếng Nga" };
            // Lấy ngôn ngữ hiện tại
            string currentLanguage = Properties.Settings.Default["Language"]?.ToString() ?? "Tiếng Anh";
            // Tìm chỉ số ngôn ngữ hiện tại
            int currentIndex = Array.IndexOf(languages, currentLanguage);
            // Chuyển sang ngôn ngữ tiếp theo
            int nextIndex = (currentIndex + 1) % languages.Length;
            string nextLanguage = languages[nextIndex];

            // Lưu ngôn ngữ mới
            Properties.Settings.Default["Language"] = nextLanguage;
            Properties.Settings.Default.Save();

            // Cập nhật tên nút
            btnChangeLanguage.Text = Translator.Translate("Ngôn ngữ: ") + nextLanguage;

            // Cập nhật ngôn ngữ cho form hiện tại
            UpdateLanguage();
        }

        private void UpdateLanguage()
        {
            btnToggleMusic.Text = Translator.Translate(MusicPlayer.IsMusicPlaying() ? "Tắt Nhạc Nền" : "Bật Nhạc Nền");
            btnToggleSound.Text = Translator.Translate(MusicPlayer.IsSoundEnabled() ? "Tắt Âm Thanh Game" : "Bật Âm Thanh Game");
            btnPlayerInfo.Text = Translator.Translate("Thông tin người chơi");
            btnIntroducing.Text = Translator.Translate("Giới thiệu");
            btnLeaderboard.Text = Translator.Translate("Bảng xếp hạng");
            btnChangeLanguage.Text = Translator.Translate("Ngôn ngữ: ") + (Properties.Settings.Default["Language"]?.ToString() ?? "Tiếng Anh");
            this.Text = Translator.Translate("Cài Đặt");
        }

        private void FormSetting_Load(object sender, EventArgs e)
        {
            btnToggleMusic.Text = MusicPlayer.IsMusicPlaying() ? "Tắt Nhạc Nền" : "Bật Nhạc Nền";
            btnToggleSound.Text = MusicPlayer.IsSoundEnabled() ? "Tắt Âm Thanh Game" : "Bật Âm Thanh Game";
            trackBarMusicVolume.Value = Math.Min((int)(MusicPlayer.GetVolume() * 70), 70);
            trackBarSoundVolume.Value = Math.Min((int)(MusicPlayer.GetSoundVolume() * 70), 70);
            // Đảm bảo nút Introducing được hiển thị
        }

        private void FormSetting_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                MusicPlayer.PlayClickSound(); // Phát âm thanh click
                this.Close(); // Đóng form
            }
        }

        private void btnIntroducing_Click(object sender, EventArgs e)
        {
            MusicPlayer.PlayClickSound();
            Form_Introduce introduceForm = new Form_Introduce();
            introduceForm.ShowDialog();
        }

        private void btnToggleMusic_Click(object sender, EventArgs e)
        {
            MusicPlayer.PlayClickSound();
            MusicPlayer.ToggleMusic();
            btnToggleMusic.Text = MusicPlayer.IsMusicPlaying() ? "Tắt Nhạc Nền" : "Bật Nhạc Nền";
            trackBarMusicVolume.Enabled = MusicPlayer.IsMusicPlaying(); // Làm mờ/mở thanh chỉnh âm lượng nhạc nền
        }

        private void btnPlayerInfo_Click(object sender, EventArgs e)
        {
            MusicPlayer.PlayClickSound();
            Form_PlayerInfo playerInfoForm = new Form_PlayerInfo(FirebaseHelper.CurrentUsername);
            playerInfoForm.ShowDialog();
        }

        private void btnLeaderboard_Click(object sender, EventArgs e)
        {
            MusicPlayer.PlayClickSound();
            Form_Leaderboard leaderboardForm = new Form_Leaderboard();
            leaderboardForm.ShowDialog();
        }

        private void btnToggleSound_Click(object sender, EventArgs e)
        {
            MusicPlayer.PlayClickSound();
            MusicPlayer.SetSoundEnabled(!MusicPlayer.IsSoundEnabled());
            btnToggleSound.Text = MusicPlayer.IsSoundEnabled() ? "Tắt Âm Thanh Game" : "Bật Âm Thanh Game";
            trackBarSoundVolume.Enabled = MusicPlayer.IsSoundEnabled(); // Làm mờ/mở thanh chỉnh âm lượng âm thanh game
        }

        private void trackBarMusicVolume_Scroll(object sender, EventArgs e)
        {
            // Làm tròn giá trị về nấc gần nhất
            int roundedValue = (int)Math.Round(trackBarMusicVolume.Value / 10.0) * 10;
            trackBarMusicVolume.Value = roundedValue; // Đặt lại giá trị thanh trượt
            MusicPlayer.SetVolume(roundedValue / 70f);
        }

        private void trackBarSoundVolume_Scroll(object sender, EventArgs e)
        {
            // Làm tròn giá trị về nấc gần nhất
            int roundedValue = (int)Math.Round(trackBarSoundVolume.Value / 10.0) * 10;

            // Chỉ phát âm thanh click nếu giá trị thay đổi sang một nấc mới
            if (trackBarSoundVolume.Value != roundedValue)
            {
                MusicPlayer.PlayClickSound();
            }

            trackBarSoundVolume.Value = roundedValue; // Đặt lại giá trị thanh trượt
            MusicPlayer.SetSoundVolume(roundedValue / 70f);
        }
    }
}