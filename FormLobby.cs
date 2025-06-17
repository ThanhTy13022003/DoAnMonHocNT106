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
        private const int MaxProcessedKeys = 1000; // Giới hạn số key để tránh rò rỉ bộ nhớ

        public FormLobby(string username)
        {
            InitializeComponent();
            currentUser = username;
        }

        private async void FormLobby_Load(object sender, EventArgs e)
        {
            try
            {
                await FirebaseHelper.SetUserOnlineStatus(currentUser, true);
                await LoadUsers();
                await LoadChatMessages();
                LangNgheLoiMoi();
                LangNgheNguoiDungThayDoi();
                LangNgheChat();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải lobby: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task LoadUsers()
        {
            try
            {
                var users = await FirebaseHelper.GetAllUsers();
                var existingItems = lstUsers.Items.Cast<ListViewItem>()
                                    .ToDictionary(item => item.Text, item => item);

                foreach (var user in users)
                {
                    string displayName = user.Username == currentUser
                        ? $"{user.Username} (You)"
                        : user.Username;

                    if (existingItems.ContainsKey(user.Username))
                    {
                        var item = existingItems[user.Username];
                        item.Text = displayName;
                        item.SubItems[1].Text = user.IsOnline ? "Online" : "Offline";
                        item.ForeColor = user.IsOnline ? Color.Green : Color.Gray;
                    }
                    else
                    {
                        var item = new ListViewItem(displayName);
                        item.SubItems.Add(user.IsOnline ? "Online" : "Offline");
                        item.ForeColor = user.IsOnline ? Color.Green : Color.Gray;
                        item.Font = user.Username == currentUser
                            ? new Font(lstUsers.Font, FontStyle.Bold)
                            : lstUsers.Font;
                        lstUsers.Items.Add(item);
                    }
                }

                foreach (var item in existingItems)
                {
                    if (!users.Any(u => u.Username == item.Key))
                    {
                        lstUsers.Items.Remove(item.Value);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách người dùng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task LoadChatMessages()
        {
            try
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
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải tin nhắn chat: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LangNgheNguoiDungThayDoi()
        {
            firebase.Child("Users")
                .AsObservable<User>()
                .Subscribe(async ev =>
                {
                    if (ev.Object == null || string.IsNullOrEmpty(ev.Key)) return;

                    if (this.IsDisposed || !this.IsHandleCreated) return;

                    await this.InvokeAsync(() =>
                    {
                        try
                        {
                            var existingItem = lstUsers.Items
                                .Cast<ListViewItem>()
                                .FirstOrDefault(item => item.Text == ev.Object.Username || item.Text == $"{ev.Object.Username} (You)");

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
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Lỗi khi cập nhật danh sách người dùng: {ex.Message}");
                        }
                    });
                });
        }

        private void LangNgheChat()
        {
            firebase.Child("PublicChat")
                .AsObservable<ChatMessage>()
                .Subscribe(async ev =>
                {
                    if (ev.Object == null || ev.Key == null || processedChatKeys.Contains(ev.Key)) return;

                    if (processedChatKeys.Count > MaxProcessedKeys)
                        processedChatKeys.Clear(); // Làm sạch để tránh rò rỉ bộ nhớ

                    processedChatKeys.Add(ev.Key);

                    if (this.IsDisposed || !this.IsHandleCreated) return;

                    await this.InvokeAsync(() =>
                    {
                        try
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
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Lỗi khi cập nhật tin nhắn: {ex.Message}");
                        }
                    });
                });
        }

        private void LangNgheLoiMoi()
        {
            firebase.Child("Invites")
                .AsObservable<Invite>()
                .Subscribe(async ev =>
                {
                    if (ev.Object == null || ev.Object.to != currentUser || processedInviteKeys.Contains(ev.Key)) return;

                    if (DateTime.TryParse(ev.Object.timestamp, out DateTime inviteTime))
                    {
                        var now = DateTime.UtcNow;
                        if ((now - inviteTime).TotalSeconds > 30)
                        {
                            await firebase.Child("Invites").Child(ev.Key).DeleteAsync();
                            return;
                        }

                        if (processedInviteKeys.Count > MaxProcessedKeys)
                            processedInviteKeys.Clear(); // Làm sạch để tránh rò rỉ bộ nhớ

                        processedInviteKeys.Add(ev.Key);

                        if (this.IsDisposed || !this.IsHandleCreated) return;

                        await this.InvokeAsync(async () =>
                        {
                            try
                            {
                                var result = MessageBox.Show($"{ev.Object.from} mời bạn chơi PvP. Chấp nhận?",
                                    "Lời mời chơi", MessageBoxButtons.YesNo);

                                await firebase.Child("Invites").Child(ev.Key).DeleteAsync();

                                if (result == DialogResult.Yes)
                                {
                                    var form = new FormPvP(currentUser, ev.Object.from, ev.Object.roomId);
                                    form.Show();
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Lỗi khi xử lý lời mời: {ex.Message}");
                            }
                        });
                    }
                });
        }

        private async void btnSend_Click(object sender, EventArgs e)
        {
            MusicPlayer.PlayClickSound();
            string msg = txtMessage.Text.Trim();
            if (string.IsNullOrEmpty(msg))
            {
                MessageBox.Show("Vui lòng nhập tin nhắn!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                await FirebaseHelper.SendChatMessage(currentUser, msg);
                txtMessage.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi gửi tin nhắn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void lstUsers_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (lstUsers.SelectedItems.Count == 0) return;

            var selectedUser = lstUsers.SelectedItems[0];
            string targetUser = selectedUser.Text.Replace(" (You)", "");

            if (targetUser == currentUser)
            {
                MessageBox.Show("Đây chính là bạn!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                var user = await FirebaseHelper.GetUserByUsername(targetUser);
                if (user?.IsOnline != true)
                {
                    MessageBox.Show("Người chơi này hiện đang offline.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

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
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi gửi lời mời: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void FormLobby_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                await FirebaseHelper.SetUserOnlineStatus(currentUser, false);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi cập nhật trạng thái offline: {ex.Message}");
            }
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