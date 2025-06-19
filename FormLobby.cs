// FormLobby.cs
// Xử lý logic và giao diện cho sảnh chờ (Lobby):
// Hiển thị danh sách người dùng online/offline, hỗ trợ chat công khai,
// gửi và nhận lời mời chơi PvP, lắng nghe thay đổi trạng thái người dùng,
// và quản lý trạng thái online của người dùng hiện tại.

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
        // Tên người dùng hiện tại
        private string currentUser;
        // Kết nối tới Firebase Realtime Database
        private FirebaseClient firebase = new FirebaseClient("https://nt106-7c9fe-default-rtdb.firebaseio.com/");
        // Lưu trữ các key lời mời đã xử lý để tránh lặp
        private HashSet<string> processedInviteKeys = new HashSet<string>();
        // Lưu trữ các key tin nhắn chat đã xử lý để tránh lặp
        private HashSet<string> processedChatKeys = new HashSet<string>();
        // Giới hạn số key lưu trữ để tránh rò rỉ bộ nhớ
        private const int MaxProcessedKeys = 1000;

        // Khởi tạo form với tên người dùng
        public FormLobby(string username)
        {
            InitializeComponent();
            currentUser = username;
        }

        // Xử lý khi form được tải
        private async void FormLobby_Load(object sender, EventArgs e)
        {
            try
            {
                // Cập nhật trạng thái online cho người dùng hiện tại
                await FirebaseHelper.SetUserOnlineStatus(currentUser, true);
                // Tải danh sách người dùng
                await LoadUsers();
                // Tải tin nhắn chat công khai
                await LoadChatMessages();
                // Lắng nghe lời mời chơi PvP
                LangNgheLoiMoi();
                // Lắng nghe thay đổi trạng thái người dùng
                LangNgheNguoiDungThayDoi();
                // Lắng nghe tin nhắn chat mới
                LangNgheChat();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải lobby: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Tải danh sách người dùng từ Firebase
        private async Task LoadUsers()
        {
            try
            {
                var users = await FirebaseHelper.GetAllUsers();
                var existingItems = lstUsers.Items.Cast<ListViewItem>()
                                    .ToDictionary(item => item.Text, item => item);

                foreach (var user in users)
                {
                    // Hiển thị tên người dùng, thêm "(You)" nếu là người dùng hiện tại
                    string displayName = user.Username == currentUser
                        ? $"{user.Username} (You)"
                        : user.Username;

                    if (existingItems.ContainsKey(user.Username))
                    {
                        // Cập nhật thông tin nếu người dùng đã tồn tại trong danh sách
                        var item = existingItems[user.Username];
                        item.Text = displayName;
                        item.SubItems[1].Text = user.IsOnline ? "Online" : "Offline";
                        item.ForeColor = user.IsOnline ? Color.Green : Color.Gray;
                    }
                    else
                    {
                        // Thêm người dùng mới vào danh sách
                        var item = new ListViewItem(displayName);
                        item.SubItems.Add(user.IsOnline ? "Online" : "Offline");
                        item.ForeColor = user.IsOnline ? Color.Green : Color.Gray;
                        item.Font = user.Username == currentUser
                            ? new Font(lstUsers.Font, FontStyle.Bold)
                            : lstUsers.Font;
                        lstUsers.Items.Add(item);
                    }
                }

                // Xóa các người dùng không còn trong danh sách từ Firebase
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

        // Tải tin nhắn chat công khai từ Firebase
        private async Task LoadChatMessages()
        {
            try
            {
                lstChat.Items.Clear();
                var msgs = await FirebaseHelper.GetPublicChatMessages();
                foreach (var msg in msgs)
                {
                    // Hiển thị tin nhắn với thời gian, người gửi và nội dung
                    var item = new ListViewItem($"{msg.Time:T} - {msg.FromUser}: {msg.Message}");
                    item.ForeColor = msg.FromUser == currentUser ? Color.Blue : Color.Black;
                    lstChat.Items.Add(item);
                }
                // Cuộn xuống tin nhắn mới nhất
                if (lstChat.Items.Count > 0)
                    lstChat.EnsureVisible(lstChat.Items.Count - 1);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải tin nhắn chat: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Lắng nghe thay đổi trạng thái của người dùng
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
                                // Cập nhật trạng thái online/offline
                                string newStatus = ev.Object.IsOnline ? "Online" : "Offline";
                                if (existingItem.SubItems[1].Text != newStatus)
                                {
                                    existingItem.SubItems[1].Text = newStatus;
                                    existingItem.ForeColor = ev.Object.IsOnline ? Color.Green : Color.Gray;
                                }
                            }
                            else if (ev.Object.Username != currentUser)
                            {
                                // Thêm người dùng mới vào danh sách
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

        // Lắng nghe tin nhắn chat công khai mới
        private void LangNgheChat()
        {
            firebase.Child("PublicChat")
                .AsObservable<ChatMessage>()
                .Subscribe(async ev =>
                {
                    if (ev.Object == null || ev.Key == null || processedChatKeys.Contains(ev.Key)) return;

                    if (processedChatKeys.Count > MaxProcessedKeys)
                        processedChatKeys.Clear(); // Xóa danh sách key để tránh rò rỉ bộ nhớ

                    processedChatKeys.Add(ev.Key);

                    if (this.IsDisposed || !this.IsHandleCreated) return;

                    await this.InvokeAsync(() =>
                    {
                        try
                        {
                            var msg = ev.Object;
                            var text = $"{msg.Time:T} - {msg.FromUser}: {msg.Message}";

                            // Kiểm tra xem tin nhắn đã tồn tại chưa
                            bool exists = lstChat.Items.Cast<ListViewItem>().Any(i => i.Text == text);

                            if (!exists)
                            {
                                // Thêm tin nhắn mới vào danh sách
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

        // Lắng nghe lời mời chơi PvP
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
                            // Xóa lời mời nếu quá 30 giây
                            await firebase.Child("Invites").Child(ev.Key).DeleteAsync();
                            return;
                        }

                        if (processedInviteKeys.Count > MaxProcessedKeys)
                            processedInviteKeys.Clear(); // Xóa danh sách key để tránh rò rỉ bộ nhớ

                        processedInviteKeys.Add(ev.Key);

                        if (this.IsDisposed || !this.IsHandleCreated) return;

                        await this.InvokeAsync(async () =>
                        {
                            try
                            {
                                // Hiển thị thông báo lời mời
                                var result = MessageBox.Show($"{ev.Object.from} mời bạn chơi PvP. Chấp nhận?",
                                    "Lời mời chơi", MessageBoxButtons.YesNo);

                                // Xóa lời mời sau khi xử lý
                                await firebase.Child("Invites").Child(ev.Key).DeleteAsync();

                                if (result == DialogResult.Yes)
                                {
                                    // Mở form PvP nếu chấp nhận lời mời
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

        // Xử lý sự kiện nhấn nút gửi tin nhắn
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
                // Gửi tin nhắn chat công khai
                await FirebaseHelper.SendChatMessage(currentUser, msg);
                txtMessage.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi gửi tin nhắn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Xử lý sự kiện double-click vào người dùng để gửi lời mời PvP
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

                // Tạo ID phòng chơi mới
                string roomId = Guid.NewGuid().ToString();
                var invite = new Invite
                {
                    from = currentUser,
                    to = targetUser,
                    roomId = roomId,
                    timestamp = DateTime.UtcNow.ToString("o")
                };

                // Gửi lời mời và mở form PvP
                await firebase.Child("Invites").PostAsync(invite);
                var formPvp = new FormPvP(currentUser, targetUser, roomId);
                formPvp.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi gửi lời mời: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Xử lý sự kiện khi đóng form
        private async void FormLobby_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                // Cập nhật trạng thái offline cho người dùng
                await FirebaseHelper.SetUserOnlineStatus(currentUser, false);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi cập nhật trạng thái offline: {ex.Message}");
            }
        }
    }

    // Lớp mở rộng để hỗ trợ gọi bất đồng bộ trên Control
    public static class ControlExtensions
    {
        // Thực thi hành động bất đồng bộ trên luồng UI
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