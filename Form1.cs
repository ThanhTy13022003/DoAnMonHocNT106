using System;
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
            FormPvE Form = new FormPvE(tênUser); // Truyền tên người dùng vào PvE
            Form.Show();
            this.Hide();
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            MusicPlayer.StopBackgroundMusic();  // Dừng nhạc khi log out
            Login Form = new Login();
            Form.Show();
            this.Hide();
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            MusicPlayer.StopBackgroundMusic();
        }
    }
}
