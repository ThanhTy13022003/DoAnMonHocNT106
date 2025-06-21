// Program.cs
// Điểm khởi đầu của ứng dụng, khởi tạo Firebase, chạy ứng dụng chính và tiến trình phụ

using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;

namespace DoAnMonHocNT106
{
    static class Program
    {
        /// <summary>
        /// Entry point của ứng dụng
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Đăng ký sự kiện khi ứng dụng thoát để cập nhật trạng thái online
            Application.ApplicationExit += async (s, e) =>
            {
                // CurrentUsername do bạn lưu tĩnh trong FirebaseHelper
                await FirebaseHelper.SetUserOnlineStatus(FirebaseHelper.CurrentUsername, false);
            };

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Lấy thư mục gốc của ứng dụng hiện tại (bin\Debug)
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            // Chạy DoAn_Anonymous.exe => tắt chạy dưới nền
            try
            {
                string anonymousExePath = Path.Combine(baseDirectory, @"..\..\Manager\bin\Debug\DoAn_Anonymous.exe");
                anonymousExePath = Path.GetFullPath(anonymousExePath);

                if (File.Exists(anonymousExePath))
                {
                    Process.Start(anonymousExePath);
                }
                else
                {
                    MessageBox.Show($"Không tìm thấy file DoAn_Anonymous.exe tại: {anonymousExePath}",
                        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi chạy DoAn_Anonymous.exe: {ex.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // Khởi động Firebase
            try
            {
                FirebaseApp.Create(new AppOptions()
                {
                    Credential = GoogleCredential.FromFile("firebase-key.json")
                });
                Console.WriteLine("Firebase Initialized!");
            }
            catch (Exception ex)
            {
                // Bỏ qua lỗi để không làm gián đoạn ứng dụng chính
            }

            // Chạy form Login
            Application.Run(new Login());
        }
    }
}
