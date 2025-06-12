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
    public partial class Form1 : Form
    {
        private string tênUser;
        private readonly string currentUser;


        // Constructor chính: nhận loginIdentifier (có thể là email hoặc username)
        public Form1(string loginIdentifier)
        {
            InitializeComponent();
            currentUser = UserIdentifier.ExtractUsername(loginIdentifier);

            InitializeUser(loginIdentifier);
            InitializeMenu();
        }

        // Overload không tham số: tự động lấy UserId từ Settings
        public Form1()
            : this(Properties.Settings.Default.UserId)
        {
            // Không cần body nữa
        }

        // Tách ra hàm khởi tạo chung cho phần user/Music/Firebase
        private void InitializeUser(string loginIdentifier)
        {
            // 1) Xác định username
            tênUser = Properties.Settings.Default.UserId;

            // 2) Lưu vào FirebaseHelper
            FirebaseHelper.CurrentUsername = tênUser;

            // 3) Bắt đầu nhạc nền nếu chưa chạy
            if (!MusicPlayer.IsMusicPlaying())
                MusicPlayer.StartBackgroundMusic();
        }

        // Tách ra hàm khởi tạo chung cho phần menu Early Access
        private void InitializeMenu()
        {
            earlyAccessMenu = new ContextMenuStrip();
            earlyAccessMenu.Items.Add("PlayViaLan").Click += (s, e) => OpenPlayViaLan();
            earlyAccessMenu.Items.Add("PlayToBot").Click += (s, e) => OpenPlayToBot();
            earlyAccessMenu.Items.Add("SettingInforPlayer").Click += (s, e) => OpenSettingInforPlayer();
            button5.ContextMenuStrip = earlyAccessMenu;
        }


        private void OpenPlayViaLan()
        {
            LaunchExternalApp(@"..\..\EarlyAccess_PlayViaLan\bin\Debug",
                      "DoAn_EarlyAccess_PlayViaLan.exe",
                      currentUser);
        }

        private void OpenPlayToBot()
        {
            LaunchExternalApp(@"..\..\EarlyAccess_PlayToBot\bin\Debug",
                  "DoAn_EarlyAccess_PlayToBot.exe",
                  currentUser);
        }

        private void OpenSettingInforPlayer()
        {
            LaunchExternalApp(@"..\..\EarlyAccess_SettingInforPlayer\bin\Debug",
                  "DoAn_EarlyAccess_SettingInforPlayer.exe",
                  currentUser);
        }

        // Hàm tiện ích
        private void LaunchExternalApp(string relativeFolder, string exeName, string args)
        {
            // Tính đường dẫn tuyệt đối đến exe
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string exePath = Path.GetFullPath(Path.Combine(baseDir, relativeFolder, exeName));

            if (!File.Exists(exePath))
            {
                MessageBox.Show($"Không tìm thấy file: {exePath}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var psi = new ProcessStartInfo
            {
                FileName = exePath,
                Arguments = args,              // truyền username
                UseShellExecute = false,       // hoặc true nếu không cần redirect I/O
                WorkingDirectory = Path.GetDirectoryName(exePath)
            };

            try
            {
                Process.Start(psi);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không thể mở ứng dụng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MusicPlayer.PlayClickSound();
            FormLobby lobby = new FormLobby(tênUser);
            lobby.FormClosed += (s, args) =>
            {
                this.Show();  // Khi FormLobby đóng thì hiện lại Form1
            };
            lobby.Show();
            this.Hide();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            MusicPlayer.PlayClickSound();
            FormPvE Form = new FormPvE(tênUser); // Truyền tên người dùng vào PvE
            Form.Show();
            this.Hide();
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            // Phát âm thanh click như cũ
            MusicPlayer.PlayClickSound();

            // Hiện hộp thoại xác nhận
            var result = MessageBox.Show(
                "Bạn có muốn thoát hoàn toàn ứng dụng không?\n" +
                "Chọn Yes để thoát hoàn toàn, No để quay lại màn hình Sign In.",
                "Xác nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                // Thoát ứng dụng hoàn toàn
                Application.Exit();
                Environment.Exit(0);
            }
            else if (result == DialogResult.No)
            {
                // Dừng nhạc nền (nếu cần) và chuyển về form Login
                MusicPlayer.StopBackgroundMusic();
                var loginForm = new Login();
                loginForm.Show();
                this.Hide();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            MusicPlayer.PlayClickSound();
            earlyAccessMenu.Show(button5, new System.Drawing.Point(0, button5.Height)); // Hiển thị menu ngay dưới nút
        }

        private async void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Chỉ bắt khi người dùng nhấn nút X (UserClosing)
            if (e.CloseReason == CloseReason.UserClosing)
            {
                var result = MessageBox.Show(
                    "Bạn chắc chắn có muốn thoát game chứ?",
                    "Xác nhận thoát",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (result == DialogResult.No)
                {
                    e.Cancel = true; // Hủy đóng form
                    return;
                }
            }

            // Nếu người dùng xác nhận Yes, thì tiếp tục thực hiện cleanup như trước
            Console.WriteLine("Đang đóng Form1.");
            try
            {
                // 1. Dừng thread giám sát nền
                BackgroundAppCloser.StopBackgroundMonitor();

                // 2. Dừng nhạc và giải phóng tài nguyên
                MusicPlayer.StopBackgroundMusic();
                await Task.Delay(500); // Chờ 0.5s
                MusicPlayer.DisposeAll();

                // 3. Đóng tất cả form con
                foreach (Form form in Application.OpenForms.Cast<Form>().ToList())
                {
                    if (form != this)
                    {
                        form.Close();
                        form.Dispose();
                    }
                }

                // 4. Giải phóng Firebase
                FirebaseApp.DefaultInstance?.Delete();

                // 5. Thoát ứng dụng hoàn toàn
                Application.Exit();
                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Lỗi khi thoát ứng dụng: {ex.Message}",
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                Environment.Exit(1);
            }
        }


        protected override void OnFormClosed(FormClosedEventArgs e)
{
    base.OnFormClosed(e);
    this.Dispose(); // Giải phóng tài nguyên form
}

        private void button3_Click(object sender, EventArgs e)
        {
            MusicPlayer.PlayClickSound();
            FormSetting settingForm = new FormSetting();
            settingForm.ShowDialog();
        }
    }

    public static class UserIdentifier
    {
        /// <summary>
        /// Kiểm tra xem input có phải email hợp lệ không.
        /// Nếu đúng, trả về phần username (trước @).
        /// Nếu không, trả luôn input dưới dạng username.
        /// </summary>
        public static string ExtractUsername(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                throw new ArgumentException("Input không được để trống");

            try
            {
                // Nếu parse thành công => input là email hợp lệ
                var addr = new MailAddress(input);
                string localPart = addr.User;    // phần trước '@'
                return string.IsNullOrWhiteSpace(localPart)
                    ? input                   // phòng khi user là rỗng
                    : localPart;
            }
            catch (FormatException)
            {
                // Không phải email, coi input là username
                return input;
            }
        }
    }

}
