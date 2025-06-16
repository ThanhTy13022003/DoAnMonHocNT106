using System;
using System.Diagnostics;
using System.Timers;
using System.Windows.Forms;
using WinFormsTimer = System.Windows.Forms.Timer;
using TimersTimer = System.Timers.Timer;
using Firebase.Database;
using Firebase.Database.Query;
using System.Threading.Tasks;


namespace Controlonline
{
    // https://nt106-7c9fe-default-rtdb.firebaseio.com/

    internal static class Program
    {
        [STAThread]
        // Thêm tham số args để nhận từ dòng lệnh
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Kiểm tra args hợp lệ
            if (args.Length < 2
                || string.IsNullOrWhiteSpace(args[0])
                || !int.TryParse(args[1], out int parentPid))
            {
                MessageBox.Show(
                    "Sai tham số! Cú pháp phải là:\nControlonline.exe <username> <parentPid>",
                    "Lỗi tham số", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string username = args[0];
            //string username = "baongdqu";

            // parentPid đã parse thành công
            _ = args[1];
            //_ = "000";

            // Chạy message‑loop với context nhận username + parentPid
            Application.Run(new HiddenContext(username, parentPid));
        }
    }

    class HiddenContext : ApplicationContext
    {
        private readonly string _username;
        private readonly int _parentPid;
        private readonly TimersTimer _watchdog;
        private async Task UpdateIsOnlineAsync(string username, bool status)
        {
            var firebase = new FirebaseClient("https://nt106-7c9fe-default-rtdb.firebaseio.com/");
            await firebase
                .Child("Users")
                .Child(username)
                .Child("IsOnline")
                .PutAsync(status);
        }

        public HiddenContext(string username, int parentPid)
        {
            _username = username;
            _parentPid = parentPid;

            // Khởi động timer để kiểm tra tiến trình cứ 1s
            _watchdog = new TimersTimer(1000);
            _watchdog.AutoReset = true;
            _watchdog.Elapsed += Watchdog_Elapsed;
            _watchdog.Start();

            // Bạn có thể khởi động các logic khác ở đây,
            // sử dụng _username nếu cần.
        }

        private void Watchdog_Elapsed(object sender, ElapsedEventArgs e)
        {
            bool isRunning;
            try
            {
                // Nếu process không tồn tại, GetProcessById sẽ ném
                Process.GetProcessById(_parentPid);
                isRunning = true;
            }
            catch
            {
                isRunning = false;
            }

            if (!isRunning)
            {
                // Dừng watchdog ngay lập tức
                _watchdog.Stop();

                // Đợi thêm 1s trước khi thoát
                var exitTimer = new TimersTimer(1000) { AutoReset = false };
                exitTimer.Elapsed += (s, ev) =>
                {
                    exitTimer.Dispose();

                    Task.Run(async () =>
                    {
                        await UpdateIsOnlineAsync(_username, true);
                        // Chờ 1s
                        await Task.Delay(1000);
                        await UpdateIsOnlineAsync(_username, false);
                        ExitThread();
                    });
                };
                exitTimer.Start();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _watchdog?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}