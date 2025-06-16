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
        private string tenUser;
        private readonly string currentUser;

        public Form1(string loginIdentifier)
        {
            InitializeComponent();
            currentUser = UserIdentifier.ExtractUsername(loginIdentifier);
            InitializeUser(loginIdentifier);
            InitializeMenu();
            // mở tự động file DoAn_Followonline.exe khi khởi động Form1
            Open_DoAn_Followonline();
        }

        public Form1() : this(Properties.Settings.Default.UserId) { }

        private void InitializeUser(string loginIdentifier)
        {
            tenUser = Properties.Settings.Default.UserId;
            FirebaseHelper.CurrentUsername = tenUser;

            if (!MusicPlayer.IsMusicPlaying())
                MusicPlayer.StartBackgroundMusic();
        }

        private void InitializeMenu()
        {
            earlyAccessMenu = new ContextMenuStrip();
            earlyAccessMenu.Items.Add("PlayViaLan").Click += (s, e) => OpenPlayViaLan();
            earlyAccessMenu.Items.Add("PlayToBot").Click += (s, e) => OpenPlayToBot();
            earlyAccessMenu.Items.Add("SettingInforPlayer").Click += (s, e) => OpenSettingInforPlayer();
            button5.ContextMenuStrip = earlyAccessMenu;
        }

        private void OpenPlayViaLan() => LaunchExternalApp(@"..\..\EarlyAccess_PlayViaLan\bin\Debug", "DoAn_EarlyAccess_PlayViaLan.exe", currentUser);
        private void OpenPlayToBot() => LaunchExternalApp(@"..\..\EarlyAccess_PlayToBot\bin\Debug", "DoAn_EarlyAccess_PlayToBot.exe", currentUser);
        private void OpenSettingInforPlayer() => LaunchExternalApp(@"..\..\EarlyAccess_SettingInforPlayer\bin\Debug", "DoAn_EarlyAccess_SettingInforPlayer.exe", currentUser);

        // truyền vào tên ứng dụng DoAn_Followonline.exe và tên người dùng hiện tại, còn id pid sẽ được lấy tự động
        private void Open_DoAn_Followonline() => LaunchExternalApp(@"..\..\Followonline\bin\Debug", "DoAn_Followonline.exe", currentUser);

        // Mở ứng dụng bên ngoài với đường dẫn tương đối và tên file
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
            // Kết hợp username và PID thành chuỗi arguments
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


        public static int GetCurrentProcessId()
        {
            return Process.GetCurrentProcess().Id;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MusicPlayer.PlayClickSound();
            var lobby = new FormLobby(currentUser);
            lobby.FormClosed += (s, args) => this.Show();
            lobby.Show();
            this.Hide();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            MusicPlayer.PlayClickSound();
            var formPvE = new FormPvE(currentUser);
            formPvE.Show();
            this.Hide();
        }

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
            {
                Application.Exit();
            }
            else
            {
                MusicPlayer.StopBackgroundMusic();
                var loginForm = new Login();
                loginForm.Show();
                this.Hide();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            MusicPlayer.PlayClickSound();
            earlyAccessMenu.Show(button5, new System.Drawing.Point(0, button5.Height));
        }

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

        protected override async void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            await FirebaseHelper.SetUserOnlineStatus(currentUser, false);
            this.Dispose();
        }

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

        private void button3_Click(object sender, EventArgs e)
        {
            MusicPlayer.PlayClickSound();
            var settingForm = new FormSetting(currentUser);
            settingForm.ShowDialog();
        }
    }

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
