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
        }
        private void button1_Click(object sender, EventArgs e)
        {
            FormLobby lobby = new FormLobby(tênUser); // Truyền tên người dùng vào FormLobby
            lobby.Show();
            this.Hide();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            PvE Form = new PvE(tênUser); // Truyền tên người dùng vào PvE
            Form.Show();
            this.Hide();
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            Login Form = new Login();
            Form.Show();
            this.Hide();
        }
    }
}
