// BackgroundAppCloser.cs
// Lớp giám sát và tự động đóng ứng dụng khi thỏa điều kiện nhất định

using System;
using System.Diagnostics;
using System.Threading;

namespace DoAnMonHocNT106
{
    /// <summary>
    /// Giám sát ứng dụng trong nền và tự động đóng khi cần thiết
    /// </summary>
    public class BackgroundAppCloser
    {
        // Thread thực thi giám sát ở chế độ nền
        private static Thread backgroundThread;
        // Token để hủy vòng lặp giám sát
        private static CancellationTokenSource cancellationTokenSource;

        /// <summary>
        /// Bắt đầu luồng giám sát ứng dụng
        /// </summary>
        public static void StartBackgroundMonitor()
        {
            cancellationTokenSource = new CancellationTokenSource();
            var token = cancellationTokenSource.Token;

            backgroundThread = new Thread(() =>
            {
                while (!token.IsCancellationRequested)
                {
                    Thread.Sleep(1000);
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

        /// <summary>
        /// Dừng giám sát và hủy luồng nền
        /// </summary>
        public static void StopBackgroundMonitor()
        {
            cancellationTokenSource?.Cancel();
            backgroundThread?.Join(1000);
        }

        /// <summary>
        /// Xác định điều kiện để đóng ứng dụng
        /// </summary>
        private static bool ShouldCloseApp()
        {
            // TODO: Thêm logic kiểm tra điều kiện cần đóng
            return false;
        }

        /// <summary>
        /// Đóng các tiến trình ứng dụng chính
        /// </summary>
        private static void CloseMainApplication()
        {
            foreach (var process in Process.GetProcessesByName("DoAnMonHocNT106"))
            {
                process.Kill();
                process.WaitForExit();
            }
        }
    }
}
