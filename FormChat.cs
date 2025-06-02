using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DoAnMonHocNT106
{
    public partial class FormChat : Form
    {
        private string fromUser;
        private string toUser;

        public FormChat(string fromUser, string toUser)
        {
            InitializeComponent();
            this.fromUser = fromUser;
            this.toUser = toUser;

            this.Text = $"Chat với {toUser}";
            Load += FormChat_Load;
        }

        private async void FormChat_Load(object sender, EventArgs e)
        {
            await LoadChatHistory();
        }

        private async Task LoadChatHistory()
        {
            var history = await FirebaseHelper.GetChatHistory(fromUser, toUser);
            foreach (var msg in history)
            {
                AddMessageToChat(msg);
            }
        }

        private void AddMessageToChat(ChatMessage msg)
        {
            string prefix = msg.FromUser == fromUser ? "Bạn: " : $"{toUser}: ";
            txtChat.AppendText($"{prefix}{msg.Message}\r\n");
        }

        private async void btnSend_Click(object sender, EventArgs e)
        {
            string message = txtInput.Text.Trim();
            if (string.IsNullOrEmpty(message)) return;

            var chatMsg = new ChatMessage
            {
                FromUser = fromUser,
                ToUser = toUser,
                Message = message,
                Time = DateTime.Now
            };

            await FirebaseHelper.SaveChatMessage(fromUser, toUser, chatMsg);

            AddMessageToChat(chatMsg);
            txtInput.Clear();
        }
    }
}
