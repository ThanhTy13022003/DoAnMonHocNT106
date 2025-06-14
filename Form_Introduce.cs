using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FirebaseAdmin;
using System.IO;
using System.Net.Mail;

namespace DoAnMonHocNT106
{
    public partial class Form_Introduce : Form
    {
        private string tênUser;
        private readonly string currentUser;


        // Constructor chính: nhận loginIdentifier (có thể là email hoặc username)
        public Form_Introduce(string loginIdentifier)
        {
            InitializeComponent();
            currentUser = UserIdentifier.ExtractUsername(loginIdentifier);

            InitializeUser(loginIdentifier);
        }

        // Overload không tham số: tự động lấy UserId từ Settings
        public Form_Introduce()
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

        private void OpenPlayViaLan()
        {
            LaunchExternalApp(@"..\..\Contribute_PlayViaLan\bin\Debug",
                      "DoAn_EarlyAccess_PlayViaLan.exe",
                      currentUser);
        }

        private void OpenPlayToBot()
        {
            LaunchExternalApp(@"..\..\Contribute_PlayToBot\bin\Debug",
                  "DoAn_EarlyAccess_PlayToBot.exe",
                  currentUser);
        }

        private void OpenSettingInforPlayer()
        {
            LaunchExternalApp(@"..\..\Contribute_SettingInforPlayer\bin\Debug",
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

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenPlayViaLan();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenPlayToBot();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenSettingInforPlayer();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // Hiển thị hộp thoại cảm ơn
            string title = "Cảm ơn";
            string message = "Cảm ơn bạn đã đóng góp cho chúng tôi! \n" +
                             "Và mọi sự đóng góp của các bạn xin hãy gửi về mail admin@example.com.";
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Đóng form giới thiệu
            //this.Close();
        }

        private void Form_Introduce_Load(object sender, EventArgs e)
        {

        }
    }
}
