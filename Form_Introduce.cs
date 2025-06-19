// Form_Introduce.cs
// Xử lý logic cho form giới thiệu và điều hướng các chức năng chơi
// Bao gồm chơi LAN, chơi với Bot, cài đặt thông tin người chơi,
// phát nhạc nền và lưu cấu hình user vào FirebaseHelper.
//

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
        // Lưu tên hiển thị của user
        private string tênUser;
        // Lưu identifier gốc (email hoặc username) đã login
        private readonly string currentUser;

        /// <summary>
        /// Constructor chính: khởi tạo form với loginIdentifier
        /// </summary>
        /// <param name="loginIdentifier">Chuỗi định danh user (email hoặc username)</param>
        public Form_Introduce(string loginIdentifier)
        {
            InitializeComponent(); // Thiết lập giao diện
            // Trích username từ loginIdentifier
            currentUser = UserIdentifier.ExtractUsername(loginIdentifier);

            InitializeUser(loginIdentifier); // Khởi tạo các thiết lập user
        }

        /// <summary>
        /// Overload constructor không tham số: lấy UserId từ Settings
        /// </summary>
        public Form_Introduce()
            : this(Properties.Settings.Default.UserId)
        {
            // Body trống do logic được chuyển vào constructor chính
        }

        /// <summary>
        /// Hàm khởi tạo chung cho user / Firebase / nhạc nền
        /// </summary>
        /// <param name="loginIdentifier">Định danh user hiện tại</param>
        private void InitializeUser(string loginIdentifier)
        {
            // 1) Gán tên user từ Settings
            tênUser = Properties.Settings.Default.UserId;

            // 2) Lưu username vào FirebaseHelper để các form khác sử dụng
            FirebaseHelper.CurrentUsername = tênUser;

            // 3) Bắt đầu phát nhạc nền nếu chưa chạy
            if (!MusicPlayer.IsMusicPlaying())
                MusicPlayer.StartBackgroundMusic();
        }

        /// <summary>
        /// Mở ứng dụng Play Via LAN với đối số truyền vào là username
        /// </summary>
        private void OpenPlayViaLan()
        {
            LaunchExternalApp(
                @"..\..\Contribute_PlayViaLan\bin\Debug",
                "GameCaro_PlayViaLan.exe",
                currentUser // Truyền username
            );
        }

        /// <summary>
        /// Mở ứng dụng Play To Bot với đối số truyền vào là username
        /// </summary>
        private void OpenPlayToBot()
        {
            LaunchExternalApp(
                @"..\..\Contribute_PlayToBot\bin\Debug",
                "GameCaro_PlayToBot.exe",
                currentUser
            );
        }

        /// <summary>
        /// Mở ứng dụng Setting Infor Player với đối số truyền vào là username
        /// </summary>
        private void OpenSettingInforPlayer()
        {
            LaunchExternalApp(
                @"..\..\Contribute_SettingInforPlayer\bin\Debug",
                "GameCaro_SettingInforPlayer.exe",
                currentUser
            );
        }

        /// <summary>
        /// Tiện ích khởi chạy một ứng dụng bên ngoài theo đường dẫn và tên exe
        /// </summary>
        /// <param name="relativeFolder">Thư mục tương đối chứa exe</param>
        /// <param name="exeName">Tên file exe</param>
        /// <param name="args">Tham số truyền vào exe</param>
        private void LaunchExternalApp(string relativeFolder, string exeName, string args)
        {
            // Lấy đường dẫn cơ sở của ứng dụng hiện tại
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            // Tạo đường dẫn tuyệt đối đến file exe
            string exePath = Path.GetFullPath(Path.Combine(baseDir, relativeFolder, exeName));

            // Kiểm tra tồn tại file
            if (!File.Exists(exePath))
            {
                MessageBox.Show(
                    $"Không tìm thấy file: {exePath}",
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return;
            }

            var psi = new ProcessStartInfo
            {
                FileName = exePath,
                Arguments = args,
                UseShellExecute = false, // Không redirect I/O
                WorkingDirectory = Path.GetDirectoryName(exePath)
            };

            try
            {
                Process.Start(psi); // Thực thi process
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Không thể mở ứng dụng: {ex.Message}",
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        // Các sự kiện tự động do Visual Studio tạo khi click các control hoặc load form

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {
            // Có thể thêm logic vẽ tùy chỉnh nếu cần
        }

        private void label1_Click(object sender, EventArgs e)
        {
            // Xử lý sự kiện khi click label1 (nếu có)
        }

        private void label3_Click(object sender, EventArgs e)
        {
            // Xử lý sự kiện khi click label3
        }

        private void label4_Click(object sender, EventArgs e)
        {
            // Xử lý sự kiện khi click label4
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenPlayViaLan(); // Gọi chức năng chơi LAN
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenPlayToBot(); // Gọi chức năng chơi với Bot
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenSettingInforPlayer(); // Gọi cài đặt thông tin player
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // Hiển thị hộp thoại cảm ơn người dùng
            string title = "Cảm ơn";
            string message = "Cảm ơn bạn đã đóng góp cho chúng tôi! \n" +
                             "Và mọi sự đóng góp của các bạn xin hãy gửi về mail admin@example.com.";
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Đóng form giới thiệu nếu cần
            // this.Close();
        }

        private void Form_Introduce_Load(object sender, EventArgs e)
        {
            // Xử lý logic khi form load (nếu có)
        }
    }
}
