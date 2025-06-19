// FormLobby.cs
// Xử lý giao diện và logic cho sảnh chờ (Lobby):
// Hiển thị danh sách người dùng, trạng thái online, chat công khai,
// và xử lý các sự kiện như gửi tin nhắn, mời chơi PvP khi double-click vào người dùng.

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DoAnMonHocNT106
{
    partial class FormLobby
    {
        // Thành phần giao diện
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.ListView lstUsers; // Danh sách người dùng
        private System.Windows.Forms.ColumnHeader colUsername; // Cột tên người dùng
        private System.Windows.Forms.ColumnHeader colStatus; // Cột trạng thái
        private System.Windows.Forms.ListView lstChat; // Danh sách tin nhắn chat
        private System.Windows.Forms.TextBox txtMessage; // Hộp nhập tin nhắn
        private System.Windows.Forms.Button btnSend; // Nút gửi tin nhắn
        private System.Windows.Forms.Label lblUsers; // Nhãn danh sách người dùng
        private System.Windows.Forms.Label lblChat; // Nhãn chat công khai

        // Giải phóng tài nguyên khi đóng form
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        // Khởi tạo các thành phần giao diện
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLobby));
            this.lstUsers = new System.Windows.Forms.ListView();
            this.colUsername = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colStatus = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lstChat = new System.Windows.Forms.ListView();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.lblUsers = new System.Windows.Forms.Label();
            this.lblChat = new System.Windows.Forms.Label();
            this.SuspendLayout();

            // 
            // lstUsers
            // 
            // Cấu hình danh sách người dùng
            this.lstUsers.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
                this.colUsername,
                this.colStatus});
            this.lstUsers.FullRowSelect = true;
            this.lstUsers.GridLines = true;
            this.lstUsers.HideSelection = false;
            this.lstUsers.Location = new System.Drawing.Point(13, 43);
            this.lstUsers.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.lstUsers.MultiSelect = false;
            this.lstUsers.Name = "lstUsers";
            this.lstUsers.Size = new System.Drawing.Size(251, 426);
            this.lstUsers.TabIndex = 0;
            this.lstUsers.UseCompatibleStateImageBehavior = false;
            this.lstUsers.View = System.Windows.Forms.View.Details;
            this.lstUsers.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lstUsers_MouseDoubleClick);

            // 
            // colUsername
            // 
            // Cột hiển thị tên người dùng
            this.colUsername.Text = "Tên người dùng";
            this.colUsername.Width = 120;

            // 
            // colStatus
            // 
            // Cột hiển thị trạng thái online/offline
            this.colStatus.Text = "Trạng thái";
            this.colStatus.Width = 80;

            // 
            // lstChat
            // 
            // Cấu hình danh sách tin nhắn chat công khai
            this.lstChat.HideSelection = false;
            this.lstChat.Location = new System.Drawing.Point(285, 43);
            this.lstChat.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.lstChat.Name = "lstChat";
            this.lstChat.Size = new System.Drawing.Size(480, 392);
            this.lstChat.TabIndex = 1;
            this.lstChat.UseCompatibleStateImageBehavior = false;
            this.lstChat.View = System.Windows.Forms.View.List;

            // 
            // txtMessage
            // 
            // Hộp nhập văn bản để gửi tin nhắn
            this.txtMessage.Location = new System.Drawing.Point(285, 446);
            this.txtMessage.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(388, 22);
            this.txtMessage.TabIndex = 2;

            // 
            // btnSend
            // 
            // Nút gửi tin nhắn chat
            this.btnSend.BackColor = System.Drawing.Color.Silver;
            this.btnSend.Location = new System.Drawing.Point(685, 443);
            this.btnSend.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(80, 27);
            this.btnSend.TabIndex = 3;
            this.btnSend.Text = "Gửi";
            this.btnSend.UseVisualStyleBackColor = false;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);

            // 
            // lblUsers
            // 
            // Nhãn hiển thị tiêu đề danh sách người dùng
            this.lblUsers.AutoSize = true;
            this.lblUsers.BackColor = System.Drawing.Color.Silver;
            this.lblUsers.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblUsers.Location = new System.Drawing.Point(13, 16);
            this.lblUsers.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblUsers.Name = "lblUsers";
            this.lblUsers.Size = new System.Drawing.Size(138, 23);
            this.lblUsers.TabIndex = 4;
            this.lblUsers.Text = "Danh sách Users";

            // 
            // lblChat
            // 
            // Nhãn hiển thị tiêu đề khu vực chat
            this.lblChat.AutoSize = true;
            this.lblChat.BackColor = System.Drawing.Color.Silver;
            this.lblChat.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblChat.Location = new System.Drawing.Point(285, 16);
            this.lblChat.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblChat.Name = "lblChat";
            this.lblChat.Size = new System.Drawing.Size(130, 23);
            this.lblChat.TabIndex = 5;
            this.lblChat.Text = "Chat công khai";

            // 
            // FormLobby
            // 
            // Cấu hình giao diện chính của form
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::DoAnMonHocNT106.Properties.Resources.sign_up;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(781, 492);
            this.Controls.Add(this.lblChat);
            this.Controls.Add(this.lblUsers);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.lstChat);
            this.Controls.Add(this.lstUsers);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "FormLobby";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sảnh chờ - PvP Lobby";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormLobby_FormClosing);
            this.Load += new System.EventHandler(this.FormLobby_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}