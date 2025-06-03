// FormLobby.cs
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Firebase.Database;
using Firebase.Database.Query;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace DoAnMonHocNT106
{
    public partial class FormLobby : Form
    {
        private FirebaseClient firebaseClient;
        private string firebaseUrl = "https://nt106-7c9fe-default-rtdb.firebaseio.com/";
        private string currentUser;
        private string tenUser;

        public FormLobby(string userName)
        {
            InitializeComponent();
            currentUser = userName;
            firebaseClient = new FirebaseClient(firebaseUrl);
            tenUser = Properties.Settings.Default["UserId"]?.ToString() ?? "Khách";
        }

        private async void Lobby_Load(object sender, EventArgs e)
        {
            await CapNhatTrangThaiOnline(true);
            TaiDanhSachUser();
            LangNgheTinNhan();
        }

        private async Task CapNhatTrangThaiOnline(bool online)
        {
            await firebaseClient
                .Child("Users")
                .Child(currentUser)
                .Child("IsOnline")
                .PutAsync(online);
        }

        private async void TaiDanhSachUser()
        {
            var users = await firebaseClient.Child("Users").OnceAsync<Dictionary<string, object>>();
            lstUser.Items.Clear();
            foreach (var user in users)
            {
                string ten = user.Key;
                bool online = false;

                if (user.Object != null && user.Object.ContainsKey("IsOnline"))
                {
                    bool.TryParse(user.Object["IsOnline"]?.ToString(), out online);
                }

                ListViewItem item = new ListViewItem(ten);
                item.SubItems.Add(online ? "Online" : "Offline");
                item.ForeColor = online ? Color.Green : Color.Gray;
                lstUser.Items.Add(item);
            }
        }

        private void LangNgheTinNhan()
        {
            firebaseClient
                .Child("PublicChat")
                .AsObservable<ChatMessage>()
                .Subscribe(d =>
                {
                    if (d.Object != null)
                    {
                        Invoke(new Action(() =>
                        {
                            txtChat.AppendText($"{d.Object.FromUser}: {d.Object.Message}\r\n");
                        }));
                    }
                });
        }

        private async void btnGui_Click(object sender, EventArgs e)
        {
            string msg = txtNhap.Text.Trim();
            if (!string.IsNullOrEmpty(msg))
            {
                var message = new ChatMessage
                {
                    FromUser = currentUser,
                    ToUser = "all",
                    Message = msg,
                    Time = DateTime.Now
                };

                await firebaseClient.Child("PublicChat").PostAsync(message);
                txtNhap.Clear();
            }
        }

        private async void Lobby_FormClosing(object sender, FormClosingEventArgs e)
        {
            await CapNhatTrangThaiOnline(false);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 Form = new Form1();
            Form.Show();
            this.Hide();
        }
    }
}
