using System;
using System.Diagnostics;
using System.Threading;

namespace DoAnMonHocNT106
{
    public class BackgroundAppCloser
    {
        private static Thread backgroundThread;
        private static CancellationTokenSource cancellationTokenSource;

        public static void StartBackgroundMonitor()
        {
            cancellationTokenSource = new CancellationTokenSource();
            var token = cancellationTokenSource.Token;

            backgroundThread = new Thread(() =>
            {
                while (!token.IsCancellationRequested)
                {
                    Thread.Sleep(1000);
                    if (token.IsCancellationRequested) break;

                    if (ShouldCloseApp())
                    {
                        CloseMainApplication();
                        break;
                    }
                }
            })
            {
                IsBackground = true
            };
            backgroundThread.Start();
        }

        public static void StopBackgroundMonitor()
        {
            cancellationTokenSource?.Cancel();
            backgroundThread?.Join(1000); // Chờ thread kết thúc trong 1s
        }

        private static bool ShouldCloseApp()
        {
            // Thêm logic để quyết định khi nào đóng ứng dụng
            // Ví dụ: kiểm tra file, thời gian, hoặc tín hiệu từ Firebase
            return false; // Thay bằng điều kiện thực tế
        }

        private static void CloseMainApplication()
        {
            // Tìm và đóng process của ứng dụng chính
            foreach (var process in Process.GetProcessesByName("DoAnMonHocNT106"))
            {
                process.Kill();
                process.WaitForExit();
            }
        }
    }
}