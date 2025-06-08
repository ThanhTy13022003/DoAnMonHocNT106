using System;
using System.Windows.Forms;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;

namespace DoAnMonHocNT106
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Đăng ký sự kiện ApplicationExit
            Application.ApplicationExit += (sender, e) =>
            {
                BackgroundAppCloser.StopBackgroundMonitor();
                MusicPlayer.DisposeAll();
            };

            BackgroundAppCloser.StartBackgroundMonitor();
            Application.Run(new Login());
        }
    }
}