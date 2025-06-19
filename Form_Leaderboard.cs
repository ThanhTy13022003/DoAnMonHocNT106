// Form_Leaderboard.cs
// Xử lý logic cho form bảng xếp hạng (Leaderboard):
// Bao gồm các chức năng tải dữ liệu bảng xếp hạng từ Firebase,
// hiển thị thông tin thứ hạng, tên người dùng, tỉ lệ thắng, tổng trận,
// xử lý phím ESC để đóng form và nút đóng form.

using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DoAnMonHocNT106
{
    public partial class Form_Leaderboard : Form
    {
        /// <summary>
        /// Constructor: khởi tạo component, thiết lập bắt sự kiện KeyDown và gọi LoadLeaderboard.
        /// </summary>
        public Form_Leaderboard()
        {
            InitializeComponent();
            // Cho phép nhận sự kiện bàn phím ở mức Form
            this.KeyPreview = true;
            this.KeyDown += FormLeaderboard_KeyDown;

            // Tải dữ liệu và hiển thị bảng xếp hạng
            LoadLeaderboard();
        }

        /// <summary>
        /// Tải và hiển thị danh sách xếp hạng người chơi từ Firebase.
        /// </summary>
        private async void LoadLeaderboard()
        {
            try
            {
                // Xóa hết mục cũ trước khi nạp mới
                lstLeaderboard.Items.Clear();

                // Đọc dữ liệu bảng xếp hạng từ Firebase
                var leaderboard = await FirebaseHelper.GetLeaderboard();

                // Thiết lập tiêu đề các cột của ListView
                lstLeaderboard.Columns.Clear();
                lstLeaderboard.Columns.Add("Hạng", 50);
                lstLeaderboard.Columns.Add("Tên người dùng", 150);
                lstLeaderboard.Columns.Add("Tỉ lệ thắng (%)", 100);
                lstLeaderboard.Columns.Add("Tổng trận", 80);
                lstLeaderboard.Columns.Add("Thắng", 60);
                lstLeaderboard.Columns.Add("Thua", 60);
                lstLeaderboard.Columns.Add("Hòa", 60);

                // Thêm từng người chơi vào ListView
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
                    // Nếu cần hiển thị thêm Timeouts, có thể thêm column và subitem tương ứng
                    lstLeaderboard.Items.Add(item);
                    rank++;
                }

                // Cập nhật ListView
                lstLeaderboard.Refresh();
            }
            catch (Exception ex)
            {
                // Hiển thị thông báo lỗi chi tiết nếu có vấn đề khi tải dữ liệu
                MessageBox.Show(
                    $"Lỗi khi tải bảng xếp hạng: {ex.Message}\nChi tiết: {ex.InnerException?.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error
                );
            }
        }

        /// <summary>
        /// Xử lý sự kiện khi nhấn phím: ESC để đóng form.
        /// </summary>
        private void FormLeaderboard_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                MusicPlayer.PlayClickSound(); // phát âm thanh click
                this.Close();                // đóng form
            }
        }

        /// <summary>
        /// Xử lý sự kiện khi nhấn nút Đóng form.
        /// </summary>
        private void btnClose_Click(object sender, EventArgs e)
        {
            MusicPlayer.PlayClickSound(); // phát âm thanh click
            this.Close();                // đóng form
        }

        /// <summary>
        /// Sự kiện khi chọn item trong ListView (hiện tại chưa sử dụng).
        /// </summary>
        private void lstLeaderboard_SelectedIndexChanged(object sender, EventArgs e)
        {
            // TODO: có thể xử lý khi người dùng chọn hàng trong bảng xếp hạng
        }
    }
}
