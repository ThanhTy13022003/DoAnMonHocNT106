using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DoAnMonHocNT106
{
    public partial class FormLobby : Form
    {
        private string currentUser;
        private FirebaseClient firebase = new FirebaseClient("https://nt106-7c9fe-default-rtdb.firebaseio.com/");
        private HashSet<string> processedInviteKeys = new HashSet<string>();
        private HashSet<string> processedChatKeys = new HashSet<string>();

        public FormLobby(string username)
        {
            InitializeComponent();
            currentUser = username;
        }

        private async void FormLobby_Load(object sender, EventArgs e)
        {
            await FirebaseHelper.SetUserOnlineStatus(currentUser, true);
            await LoadUsers();
            await LoadChatMessages();
            LangNgheLoiMoi();
            LangNgheNguoiDungThayDoi();
            LangNgheChat();
        }

        private async Task LoadUsers()
        {
            var users = await FirebaseHelper.GetAllUsers();

            var existingItems = lstUsers.Items.Cast<ListViewItem>()
                                    .ToDictionary(item => item.Text, item => item);

            foreach (var user in users)
            {
                if (user.Username == currentUser) continue;

                if (existingItems.ContainsKey(user.Username))
                {
                    var item = existingItems[user.Username];
                    string newStatus = user.IsOnline ? "Online" : "Offline";
                    if (item.SubItems[1].Text != newStatus)
                    {
                        item.SubItems[1].Text = newStatus;
                        item.ForeColor = user.IsOnline ? Color.Green : Color.Gray;
                    }
                }
                else
                {
                    var item = new ListViewItem(user.Username);
                    item.SubItems.Add(user.IsOnline ? "Online" : "Offline");
                    item.ForeColor = user.IsOnline ? Color.Green : Color.Gray;
                    lstUsers.Items.Add(item);
                }
            }

            // Xóa user không còn tồn tại
            foreach (var item in existingItems)
            {
                if (!users.Any(u => u.Username == item.Key))
                {
                    lstUsers.Items.Remove(item.Value);
                }
            }
        }

        private async Task LoadChatMessages()
        {
            lstChat.Items.Clear();
            var msgs = await FirebaseHelper.GetPublicChatMessages();
            foreach (var msg in msgs)
            {
                var item = new ListViewItem($"{msg.Time:T} - {msg.FromUser}: {msg.Message}");
                item.ForeColor = msg.FromUser == currentUser ? Color.Blue : Color.Black;
                lstChat.Items.Add(item);
            }
            if (lstChat.Items.Count > 0)
                lstChat.EnsureVisible(lstChat.Items.Count - 1);
        }

        private void LangNgheNguoiDungThayDoi()
        {
            firebase.Child("Users")
                .AsObservable<User>()
                .Subscribe(async ev =>
                {
                    if (ev.Object != null && !string.IsNullOrEmpty(ev.Key))
                    {
                        if (this.IsDisposed || !this.IsHandleCreated) return;

                        await this.InvokeAsync(() =>
                        {
                            var existingItem = lstUsers.Items
                                .Cast<ListViewItem>()
                                .FirstOrDefault(item => item.Text == ev.Object.Username);

                            if (existingItem != null)
                            {
                                string newStatus = ev.Object.IsOnline ? "Online" : "Offline";
                                if (existingItem.SubItems[1].Text != newStatus)
                                {
                                    existingItem.SubItems[1].Text = newStatus;
                                    existingItem.ForeColor = ev.Object.IsOnline ? Color.Green : Color.Gray;
                                }
                            }
                            else if (ev.Object.Username != currentUser)
                            {
                                var item = new ListViewItem(ev.Object.Username);
                                item.SubItems.Add(ev.Object.IsOnline ? "Online" : "Offline");
                                item.ForeColor = ev.Object.IsOnline ? Color.Green : Color.Gray;
                                lstUsers.Items.Add(item);
                            }
                        });
                    }
                });
        }

        private void LangNgheChat()
        {
            firebase.Child("PublicChat")
                .AsObservable<ChatMessage>()
                .Subscribe(async ev =>
                {
                    if (ev.Object != null && ev.Key != null && !processedChatKeys.Contains(ev.Key))
                    {
                        processedChatKeys.Add(ev.Key);

                        if (this.IsDisposed || !this.IsHandleCreated) return;

                        await this.InvokeAsync(() =>
                        {
                            var msg = ev.Object;
                            var text = $"{msg.Time:T} - {msg.FromUser}: {msg.Message}";

                            bool exists = lstChat.Items.Cast<ListViewItem>().Any(i => i.Text == text);

                            if (!exists)
                            {
                                var item = new ListViewItem(text);
                                item.ForeColor = msg.FromUser == currentUser ? Color.Blue : Color.Black;
                                lstChat.Items.Add(item);
                                lstChat.EnsureVisible(lstChat.Items.Count - 1);
                            }
                        });
                    }
                });
        }

        private void LangNgheLoiMoi()
        {
            firebase.Child("Invites")
                .AsObservable<Invite>()
                .Subscribe(async ev =>
                {
                    if (ev.Object != null &&
                        ev.Object.to == currentUser &&
                        !processedInviteKeys.Contains(ev.Key))
                    {
                        if (DateTime.TryParse(ev.Object.timestamp, out DateTime inviteTime))
                        {
                            var now = DateTime.UtcNow;
                            if ((now - inviteTime).TotalSeconds <= 30)
                            {
                                processedInviteKeys.Add(ev.Key);

                                if (this.IsDisposed || !this.IsHandleCreated) return;

                                await this.InvokeAsync(async () =>
                                {
                                    var result = MessageBox.Show($"{ev.Object.from} mời bạn chơi PvP. Chấp nhận?",
                                        "Lời mời chơi", MessageBoxButtons.YesNo);

                                    await firebase.Child("Invites").Child(ev.Key).DeleteAsync();

                                    if (result == DialogResult.Yes)
                                    {
                                        var form = new FormPvP(currentUser, ev.Object.from, ev.Object.roomId);
                                        form.Show();
                                    }
                                });
                            }
                            else
                            {
                                await firebase.Child("Invites").Child(ev.Key).DeleteAsync();
                            }
                        }
                    }
                });
        }

        private async void btnSend_Click(object sender, EventArgs e)
        {
            MusicPlayer.PlayClickSound();
            string msg = txtMessage.Text.Trim();
            if (!string.IsNullOrEmpty(msg))
            {
                await FirebaseHelper.SendChatMessage(currentUser, msg);
                txtMessage.Clear();
            }
        }

        private async void lstUsers_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (lstUsers.SelectedItems.Count > 0)
            {
                var selectedUser = lstUsers.SelectedItems[0];
                if (selectedUser.SubItems[1].Text == "Online")
                {
                    string targetUser = selectedUser.Text;
                    string roomId = Guid.NewGuid().ToString();

                    var invite = new Invite
                    {
                        from = currentUser,
                        to = targetUser,
                        roomId = roomId,
                        timestamp = DateTime.UtcNow.ToString("o")
                    };

                    await firebase.Child("Invites").PostAsync(invite);

                    var formPvp = new FormPvP(currentUser, targetUser, roomId);
                    formPvp.Show();
                }
                else
                {
                    MessageBox.Show("Người chơi này hiện đang offline.");
                }
            }
        }

        private async void FormLobby_FormClosing(object sender, FormClosingEventArgs e)
        {
            await FirebaseHelper.SetUserOnlineStatus(currentUser, false);
        }
    }

    public static class ControlExtensions
    {
        public static Task InvokeAsync(this Control control, Action action)
        {
            var tcs = new TaskCompletionSource<object>();

            if (control == null || control.IsDisposed || !control.IsHandleCreated)
            {
                try
                {
                    action();
                    tcs.SetResult(null);
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            }
            else
            {
                control.BeginInvoke(new MethodInvoker(() =>
                {
                    try
                    {
                        action();
                        tcs.SetResult(null);
                    }
                    catch (Exception ex)
                    {
                        tcs.SetException(ex);
                    }
                }));
            }

            return tcs.Task;
        }
    }
}
