using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DoAnMonHocNT106
{
    public partial class FormLobby : Form
    {
        private string currentUser;
        private List<User> onlineUsers;
        private SocketManager socketManager;

        public FormLobby(string username)
        {
            InitializeComponent();
            currentUser = username;
            socketManager = new SocketManager();
            socketManager.OnMessageReceived += SocketManager_OnMessageReceived;

            Load += FormLobby_Load;
        }

        private async void FormLobby_Load(object sender, EventArgs e)
        {
            await FirebaseHelper.SetUserOnlineStatus(currentUser, true);
            await LoadOnlineUsers();

            // Cập nhật lại danh sách mỗi 10s
            var timer = new Timer { Interval = 10000 };
            timer.Tick += async (s, ex) => await LoadOnlineUsers();
            timer.Start();
        }

        private async Task LoadOnlineUsers()
        {
            onlineUsers = await FirebaseHelper.GetOnlineUsers();
            listViewUsers.Items.Clear();

            foreach (var user in onlineUsers)
            {
                if (user.Username == currentUser) continue;

                var lvi = new ListViewItem(user.Username);
                lvi.SubItems.Add(user.IsOnline ? "Online" : "Offline");
                listViewUsers.Items.Add(lvi);
            }
        }

        // Double click user mở chat riêng
        private async void listViewUsers_DoubleClick(object sender, EventArgs e)
        {
            if (listViewUsers.SelectedItems.Count == 0) return;

            string targetUser = listViewUsers.SelectedItems[0].Text;
            var chatForm = new FormChat(currentUser, targetUser);
            chatForm.Show();
        }

        // Tạo phòng PvP (host)
        private async void btnHost_Click(object sender, EventArgs e)
        {
            int port = 9000;
            try
            {
                await socketManager.StartServer(port);
                MessageBox.Show("Đã tạo phòng, đang chờ đối thủ kết nối...");

                var gameForm = new FormGamePvP(currentUser, socketManager, true);
                gameForm.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tạo phòng: " + ex.Message);
            }
        }

        // Tham gia phòng PvP (join)
        private async void btnJoin_Click(object sender, EventArgs e)
        {
            string ip = txtIP.Text.Trim();
            int port = 9000;
            try
            {
                await socketManager.ConnectToServer(ip, port);
                MessageBox.Show("Đã kết nối đến phòng!");

                var gameForm = new FormGamePvP(currentUser, socketManager, false);
                gameForm.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối phòng: " + ex.Message);
            }
        }

        private void SocketManager_OnMessageReceived(string msg)
        {
            // Xử lý tin nhắn nhận trong lobby (nếu có)
            Console.WriteLine("Received: " + msg);
        }
    }
}
