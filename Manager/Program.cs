using System;
using System.Diagnostics;
using System.Threading;
using System.Runtime.InteropServices;
using System.Collections.Generic;

class Program
{
    // Import WinAPI để kiểm tra trạng thái cửa sổ
    [DllImport("user32.dll")]
    private static extern bool IsWindowVisible(IntPtr hWnd);

    // Import WinAPI để ẩn console
    [DllImport("kernel32.dll")]
    private static extern IntPtr GetConsoleWindow();

    [DllImport("user32.dll")]
    private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    private const int SW_HIDE = 0;

    // Dictionary to track process start times
    private static Dictionary<int, DateTime> processStartTimes = new Dictionary<int, DateTime>();

    [DllImport("user32.dll")]
    private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

    private const int GWL_EXSTYLE = -20;
    private const int WS_EX_TOOLWINDOW = 0x80;

    static void Main(string[] args)
    {
        // Ẩn cửa sổ console
        IntPtr consoleWindow = GetConsoleWindow();
        if (consoleWindow != IntPtr.Zero)
        {
            ShowWindow(consoleWindow, SW_HIDE);
            SetWindowLong(consoleWindow, GWL_EXSTYLE, WS_EX_TOOLWINDOW);
        }

        // Thời gian bắt đầu chương trình
        DateTime programStartTime = DateTime.Now;

        while (true)
        {
            try
            {
                // Get all processes with the specified name
                Process[] processes = Process.GetProcessesByName("DoAnMonHocNT106");
                DateTime currentTime = DateTime.Now;
                bool processKilled = false; // Flag to track if a process was killed

                // Kiểm tra nếu không có tiến trình nào và đã chạy quá 7 giây
                if (processes.Length == 0)
                {
                    TimeSpan programDuration = currentTime - programStartTime;
                    if (programDuration.TotalSeconds >= 7)
                    {
                        break; // Thoát chương trình nếu không có tiến trình sau 7 giây
                    }
                }

                // Update process start times and detect new processes
                foreach (Process process in processes)
                {
                    try
                    {
                        if (!processStartTimes.ContainsKey(process.Id))
                        {
                            // New process detected
                            processStartTimes[process.Id] = process.StartTime;
                        }
                    }
                    catch (Exception)
                    {
                        // Suppress errors to run silently
                    }
                }

                foreach (Process process in processes)
                {
                    try
                    {
                        IntPtr windowHandle = process.MainWindowHandle;
                        bool isHidden = (windowHandle == IntPtr.Zero) || !IsWindowVisible(windowHandle);

                        // Check if process is within 7-second grace period
                        if (processStartTimes.TryGetValue(process.Id, out DateTime startTime))
                        {
                            TimeSpan age = currentTime - startTime;
                            if (age.TotalSeconds < 7)
                            {
                                continue;
                            }
                        }

                        if (isHidden)
                        {
                            process.Kill();
                            processStartTimes.Remove(process.Id); // Clean up
                            processKilled = true; // Set flag to true
                            break; // Exit the foreach loop after killing a process
                        }
                    }
                    catch (Exception)
                    {
                        // Suppress errors to run silently
                    }
                }

                // Clean up start times for exited processes
                List<int> pidsToRemove = new List<int>();
                foreach (var pid in processStartTimes.Keys)
                {
                    bool processExists = false;
                    foreach (Process p in processes)
                    {
                        if (p.Id == pid)
                        {
                            processExists = true;
                            break;
                        }
                    }
                    if (!processExists)
                    {
                        pidsToRemove.Add(pid);
                    }
                }
                foreach (var pid in pidsToRemove)
                {
                    processStartTimes.Remove(pid);
                }

                // Exit the program if a process was killed
                if (processKilled)
                {
                    break; // Exit the while loop
                }
            }
            catch (Exception)
            {
                // Suppress errors to run silently
            }

            // Sleep for 5 seconds
            Thread.Sleep(5000);
        }
    }
}