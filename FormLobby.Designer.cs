namespace DoAnMonHocNT106
{
    partial class FormLobby
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.ListView lstUser;
        private System.Windows.Forms.ColumnHeader colUser;
        private System.Windows.Forms.ColumnHeader colStatus;
        private System.Windows.Forms.TextBox txtChat;
        private System.Windows.Forms.TextBox txtNhap;
        private System.Windows.Forms.Button btnGui;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lstUser = new System.Windows.Forms.ListView();
            this.colUser = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colStatus = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.txtChat = new System.Windows.Forms.TextBox();
            this.txtNhap = new System.Windows.Forms.TextBox();
            this.btnGui = new System.Windows.Forms.Button();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lstUser
            // 
            this.lstUser.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colUser,
            this.colStatus});
            this.lstUser.FullRowSelect = true;
            this.lstUser.GridLines = true;
            this.lstUser.HideSelection = false;
            this.lstUser.Location = new System.Drawing.Point(12, 36);
            this.lstUser.Name = "lstUser";
            this.lstUser.Size = new System.Drawing.Size(250, 358);
            this.lstUser.TabIndex = 0;
            this.lstUser.UseCompatibleStateImageBehavior = false;
            this.lstUser.View = System.Windows.Forms.View.Details;
            // 
            // colUser
            // 
            this.colUser.Text = "Tên người dùng";
            this.colUser.Width = 140;
            // 
            // colStatus
            // 
            this.colStatus.Text = "Trạng thái";
            this.colStatus.Width = 100;
            // 
            // txtChat
            // 
            this.txtChat.Location = new System.Drawing.Point(270, 36);
            this.txtChat.Multiline = true;
            this.txtChat.Name = "txtChat";
            this.txtChat.ReadOnly = true;
            this.txtChat.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtChat.Size = new System.Drawing.Size(400, 320);
            this.txtChat.TabIndex = 1;
            // 
            // txtNhap
            // 
            this.txtNhap.Location = new System.Drawing.Point(270, 374);
            this.txtNhap.Name = "txtNhap";
            this.txtNhap.Size = new System.Drawing.Size(300, 20);
            this.txtNhap.TabIndex = 2;
            // 
            // btnGui
            // 
            this.btnGui.Location = new System.Drawing.Point(580, 371);
            this.btnGui.Name = "btnGui";
            this.btnGui.Size = new System.Drawing.Size(90, 23);
            this.btnGui.TabIndex = 3;
            this.btnGui.Text = "Gửi";
            this.btnGui.UseVisualStyleBackColor = true;
            this.btnGui.Click += new System.EventHandler(this.btnGui_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // button1
            // 
            this.button1.BackgroundImage = global::DoAnMonHocNT106.Properties.Resources._251451_200;
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button1.Location = new System.Drawing.Point(12, 1);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(38, 29);
            this.button1.TabIndex = 4;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // FormLobby
            // 
            this.ClientSize = new System.Drawing.Size(684, 421);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnGui);
            this.Controls.Add(this.txtNhap);
            this.Controls.Add(this.txtChat);
            this.Controls.Add(this.lstUser);
            this.Name = "FormLobby";
            this.Text = "Sảnh chờ người chơi";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Lobby_FormClosing);
            this.Load += new System.EventHandler(this.Lobby_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.Button button1;
    }
}
