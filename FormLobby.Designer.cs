﻿namespace DoAnMonHocNT106
{
    partial class FormLobby
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.ListView lstUsers;
        private System.Windows.Forms.ColumnHeader colUsername;
        private System.Windows.Forms.ColumnHeader colStatus;
        private System.Windows.Forms.ListView lstChat;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Label lblUsers;
        private System.Windows.Forms.Label lblChat;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
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
            this.lstUsers.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colUsername,
            this.colStatus});
            this.lstUsers.FullRowSelect = true;
            this.lstUsers.GridLines = true;
            this.lstUsers.HideSelection = false;
            this.lstUsers.Location = new System.Drawing.Point(10, 35);
            this.lstUsers.MultiSelect = false;
            this.lstUsers.Name = "lstUsers";
            this.lstUsers.Size = new System.Drawing.Size(189, 347);
            this.lstUsers.TabIndex = 0;
            this.lstUsers.UseCompatibleStateImageBehavior = false;
            this.lstUsers.View = System.Windows.Forms.View.Details;
            this.lstUsers.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lstUsers_MouseDoubleClick);
            // 
            // colUsername
            // 
            this.colUsername.Text = "Tên người dùng";
            this.colUsername.Width = 120;
            // 
            // colStatus
            // 
            this.colStatus.Text = "Trạng thái";
            this.colStatus.Width = 80;
            // 
            // lstChat
            // 
            this.lstChat.HideSelection = false;
            this.lstChat.Location = new System.Drawing.Point(214, 35);
            this.lstChat.Name = "lstChat";
            this.lstChat.Size = new System.Drawing.Size(361, 319);
            this.lstChat.TabIndex = 1;
            this.lstChat.UseCompatibleStateImageBehavior = false;
            this.lstChat.View = System.Windows.Forms.View.List;
            // 
            // txtMessage
            // 
            this.txtMessage.Location = new System.Drawing.Point(214, 362);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(292, 20);
            this.txtMessage.TabIndex = 2;
            // 
            // btnSend
            // 
            this.btnSend.BackColor = System.Drawing.Color.Silver;
            this.btnSend.Location = new System.Drawing.Point(514, 360);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(60, 22);
            this.btnSend.TabIndex = 3;
            this.btnSend.Text = "Gửi";
            this.btnSend.UseVisualStyleBackColor = false;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // lblUsers
            // 
            this.lblUsers.AutoSize = true;
            this.lblUsers.BackColor = System.Drawing.Color.Silver;
            this.lblUsers.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblUsers.Location = new System.Drawing.Point(10, 13);
            this.lblUsers.Name = "lblUsers";
            this.lblUsers.Size = new System.Drawing.Size(116, 19);
            this.lblUsers.TabIndex = 4;
            this.lblUsers.Text = "Danh sách Users";
            // 
            // lblChat
            // 
            this.lblChat.AutoSize = true;
            this.lblChat.BackColor = System.Drawing.Color.Silver;
            this.lblChat.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblChat.Location = new System.Drawing.Point(214, 13);
            this.lblChat.Name = "lblChat";
            this.lblChat.Size = new System.Drawing.Size(108, 19);
            this.lblChat.TabIndex = 5;
            this.lblChat.Text = "Chat công khai";
            // 
            // FormLobby
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::DoAnMonHocNT106.Properties.Resources.sign_up;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(586, 400);
            this.Controls.Add(this.lblChat);
            this.Controls.Add(this.lblUsers);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.lstChat);
            this.Controls.Add(this.lstUsers);
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