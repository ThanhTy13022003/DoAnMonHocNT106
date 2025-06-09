using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DoAnMonHocNT106
{
    public partial class Form_Leaderboard : Form
    {
        public Form_Leaderboard()
        {
            InitializeComponent();
            this.KeyPreview = true;
            this.KeyDown += FormLeaderboard_KeyDown;
            LoadLeaderboard();
        }

        private async void LoadLeaderboard()
        {
            try
            {
                lstLeaderboard.Items.Clear();
                var leaderboard = await FirebaseHelper.GetLeaderboard();

                // Thiết lập tiêu đề cột
                lstLeaderboard.Columns.Clear();
                lstLeaderboard.Columns.Add("Hạng", 50);
                lstLeaderboard.Columns.Add("Tên người dùng", 150);
                lstLeaderboard.Columns.Add("Tỉ lệ thắng (%)", 100);
                lstLeaderboard.Columns.Add("Tổng trận", 80);
                lstLeaderboard.Columns.Add("Thắng", 60);
                lstLeaderboard.Columns.Add("Thua", 60);
                lstLeaderboard.Columns.Add("Hòa", 60);
                lstLeaderboard.Columns.Add("Hết thời gian", 100);

                // Thêm dữ liệu
                int rank = 1;
                foreach (var player in leaderboard)
                {
                    var item = new ListViewItem(rank.ToString());
                    item.SubItems.Add(player.Username ?? "N/A");
                    item.SubItems.Add(player.WinRate.ToString("0.00"));
                    item.SubItems.Add(player.TotalGames.ToString());
                    item.SubItems.Add(player.Wins.ToString());
                    item.SubItems.Add(player.Losses.ToString());
                    item.SubItems.Add(player.Draws.ToString());
                    //item.SubItems.Add(player.Timeouts.ToString()); // Sử dụng Timeouts từ tuple
                    lstLeaderboard.Items.Add(item);
                    rank++;
                }

                lstLeaderboard.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải bảng xếp hạng: {ex.Message}\nChi tiết: {ex.InnerException?.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormLeaderboard_KeyDown(object sender, KeyEventArgs e)
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