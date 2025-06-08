using FirebaseAdmin;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DoAnMonHocNT106
{
    public partial class Form1 : Form
    {
        private string tênUser;

        public Form1()
        {
            InitializeComponent();
            // Lấy tên người dùng từ Settings
            tênUser = Properties.Settings.Default.UserId;
            FirebaseHelper.CurrentUsername = tênUser;
            if (!MusicPlayer.IsMusicPlaying())
                MusicPlayer.StartBackgroundMusic();
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
            MusicPlayer.PlayClickSound();
            MusicPlayer.StopBackgroundMusic();  // Dừng nhạc khi log out
            Login Form = new Login();
            Form.Show();
            this.Hide();
        }

        private async void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Console.WriteLine("Đang đóng Form1...");
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
                MessageBox.Show($"Lỗi khi thoát ứng dụng: {ex.Message}",
                                "Lỗi",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
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
}
