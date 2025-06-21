// Form1.cs
// Form chính của ứng dụng: quản lý điều hướng giữa các chế độ chơi, khởi động tiến trình phụ,
// xử lý đăng xuất, thoát ứng dụng và cấu hình các thành phần như menu, âm thanh nền.

using FirebaseAdmin;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Net.Mail;

namespace DoAnMonHocNT106
{
    /// <summary>
    /// Form1 là form chính hiển thị các tùy chọn chơi game,
    /// khởi tạo tiến trình khác, cập nhật trạng thái user và xử lý đóng ứng dụng.
    /// </summary>
    public partial class Form1 : Form
    {
        private string tenUser;
        private readonly string currentUser;

        /// <summary>
        /// Khởi tạo Form1 với thông tin đăng nhập (username hoặc email)
        /// </summary>
        public Form1(string loginIdentifier)
        {
            InitializeComponent();
            currentUser = UserIdentifier.ExtractUsername(loginIdentifier);
            InitializeUser(loginIdentifier);
            // Mở tự động file DoAn_Followonline.exe khi khởi động Form1
            Open_DoAn_Followonline();
        }

        /// <summary>
        /// Constructor mặc định sử dụng UserId từ Settings
        /// </summary>
        public Form1() : this(Properties.Settings.Default.UserId) { }

        /// <summary>
        /// Gán thông tin user và khởi động nhạc nền nếu chưa phát
        /// </summary>
        private void InitializeUser(string loginIdentifier)
        {
            tenUser = Properties.Settings.Default.UserId;
            FirebaseHelper.CurrentUsername = tenUser;

            if (!MusicPlayer.IsMusicPlaying())
                MusicPlayer.StartBackgroundMusic();
        }

        // Mở tiến trình Followonline khi form khởi động
        private void Open_DoAn_Followonline() => LaunchExternalApp(@"..\..\Followonline\bin\Debug", "DoAn_Followonline.exe", currentUser);

        /// <summary>
        /// Khởi chạy ứng dụng bên ngoài với đường dẫn tương đối, exe và đối số truyền username + PID
        /// </summary>
        private void LaunchExternalApp(string relativeFolder, string exeName, string userArg)
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string exePath = Path.Combine(baseDir, relativeFolder, exeName);

            if (!File.Exists(exePath))
            {
                MessageBox.Show($"Không tìm thấy file: {exePath}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Lấy PID của tiến trình hiện tại
            int pid = GetCurrentProcessId();
            string combinedArgs = $"{userArg} {pid}";

            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = exePath,
                    Arguments = combinedArgs,
                    UseShellExecute = false,
                    WorkingDirectory = Path.GetDirectoryName(exePath)
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không thể mở ứng dụng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Lấy Process ID của tiến trình đang chạy
        /// </summary>
        public static int GetCurrentProcessId() => Process.GetCurrentProcess().Id;

        /// <summary>
        /// Xử lý sự kiện nút lobby: mở FormLobby và ẩn Form1
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            MusicPlayer.PlayClickSound();
            var lobby = new FormLobby(currentUser);
            lobby.FormClosed += (s, args) => this.Show();
            lobby.Show();
            this.Hide();
        }

        /// <summary>
        /// Mở chế độ PvE
        /// </summary>
        private void button2_Click_1(object sender, EventArgs e)
        {
            MusicPlayer.PlayClickSound();
            var formPvE = new FormPvE(currentUser);
            formPvE.Show();
            this.Hide();
        }

        /// <summary>
        /// Xử lý nút thoát hoặc đăng xuất
        /// </summary>
        private async void button4_Click_1(object sender, EventArgs e)
        {
            MusicPlayer.PlayClickSound();

            var result = MessageBox.Show(
                "Bạn có muốn thoát hoàn toàn ứng dụng không?\nChọn Yes để thoát hoàn toàn, No để quay lại màn hình Sign In.",
                "Xác nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            await FirebaseHelper.SetUserOnlineStatus(currentUser, false);

            if (result == DialogResult.Yes)
                Application.Exit();
            else
            {
                MusicPlayer.StopBackgroundMusic();
                var loginForm = new Login();
                loginForm.Show();
                this.Hide();
            }
        }

        /// <summary>
        /// Xác nhận trước khi đóng form
        /// </summary>
        private async void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                var result = MessageBox.Show(
                    "Bạn chắc chắn có muốn thoát game chứ?",
                    "Xác nhận thoát",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.No)
                {
                    e.Cancel = true;
                    return;
                }
            }

            await HandleExitAsync();
        }

        /// <summary>
        /// Khi form đóng, cập nhật online và giải phóng tài nguyên
        /// </summary>
        protected override async void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            await FirebaseHelper.SetUserOnlineStatus(currentUser, false);
            this.Dispose();
        }

        /// <summary>
        /// Xử lý đóng toàn bộ ứng dụng: dừng nhạc, giám sát, đóng tất cả form và tiến trình Firebase
        /// </summary>
        private async Task HandleExitAsync()
        {
            try
            {
                await FirebaseHelper.SetUserOnlineStatus(currentUser, false);
                BackgroundAppCloser.StopBackgroundMonitor();
                MusicPlayer.StopBackgroundMusic();
                await Task.Delay(500);
                MusicPlayer.DisposeAll();

                foreach (Form form in Application.OpenForms.Cast<Form>().ToList())
                {
                    if (form != this)
                    {
                        form.Close();
                        form.Dispose();
                    }
                }

                FirebaseApp.DefaultInstance?.Delete();

                Application.Exit();
                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thoát ứng dụng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(1);
            }
        }

        /// <summary>
        /// Mở form cài đặt
        /// </summary>
        private void button3_Click(object sender, EventArgs e)
        {
            MusicPlayer.PlayClickSound();
            var settingForm = new FormSetting(currentUser);
            settingForm.ShowDialog();
        }
    }

    /// <summary>
    /// Helper để trích xuất username từ input (email hoặc chuỗi đơn giản)
    /// </summary>
    public static class UserIdentifier
    {
        public static string ExtractUsername(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                throw new ArgumentException("Input không được để trống");

            try
            {
                var addr = new MailAddress(input);
                return string.IsNullOrWhiteSpace(addr.User) ? input : addr.User;
            }
            catch
            {
                return input;
            }
        }
    }
}
