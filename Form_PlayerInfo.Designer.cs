// Form_PlayerInfo.Designer.cs
// Thiết lập giao diện cho form Thông tin người chơi (Player Info):
// Bao gồm các nhãn hiển thị tên người dùng, email, lần cuối online, thống kê thắng/thua/hòa,
// nhãn mật khẩu với nút ẩn/hiện, nút thay đổi thông tin và nút đóng form.

namespace DoAnMonHocNT106
{
    partial class Form_PlayerInfo
    {
        /// <summary>
        /// Biến chứa các component cần dispose khi không sử dụng.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label lblUsername;        // Nhãn hiển thị tên người dùng
        private System.Windows.Forms.Label lblEmail;           // Nhãn hiển thị email
        private System.Windows.Forms.Label lblLastOnline;      // Nhãn hiển thị lần cuối online
        private System.Windows.Forms.Label lblStats;           // Nhãn hiển thị thống kê thắng/thua/hòa
        private System.Windows.Forms.Label lblPassword;        // Nhãn hiển thị mật khẩu (ẩn/hiện)
        private System.Windows.Forms.Button btnTogglePassword;  // Nút ẩn/hiện mật khẩu
        private System.Windows.Forms.Button btnChangeInfo;      // Nút mở form Thay đổi thông tin
        private System.Windows.Forms.Button btnClose;           // Nút đóng form

        /// <summary>
        /// Giải phóng tài nguyên đang sử dụng.
        /// </summary>
        /// <param name="disposing">true nếu tài nguyên quản lý cần được dispose; ngược lại false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Phương thức khởi tạo và cấu hình các control trên form.
        /// </summary>
        private void InitializeComponent()
        {
            // Resource manager để lấy icon và hình nền
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_PlayerInfo));

            // btnChangeInfo: Nút thay đổi thông tin người dùng
            this.btnChangeInfo = new System.Windows.Forms.Button()
            {
                BackColor = System.Drawing.SystemColors.ActiveCaptionText,
                FlatAppearance = { BorderSize = 0 },
                FlatStyle = System.Windows.Forms.FlatStyle.Flat,
                Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.White,
                Location = new System.Drawing.Point(54, 257),
                Name = "btnChangeInfo",
                Size = new System.Drawing.Size(208, 36),
                TabIndex = 0,
                Text = "Thay đổi thông tin",
                UseVisualStyleBackColor = false
            };
            this.btnChangeInfo.Click += new System.EventHandler(this.btnChangeInfo_Click);

            // lblPassword: Nhãn hiển thị mật khẩu, mặc định ẩn
            this.lblPassword = new System.Windows.Forms.Label()
            {
                AutoSize = true,
                Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold),
                Location = new System.Drawing.Point(54, 139),
                Name = "lblPassword",
                Size = new System.Drawing.Size(178, 25),
                TabIndex = 5,
                Text = "Mật khẩu: ********"
            };

            // btnTogglePassword: Nút ẩn/hiện mật khẩu
            this.btnTogglePassword = new System.Windows.Forms.Button()
            {
                BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch,
                Location = new System.Drawing.Point(18, 139),
                Name = "btnTogglePassword",
                Size = new System.Drawing.Size(30, 25),
                TabIndex = 6,
                UseVisualStyleBackColor = true
            };
            this.btnTogglePassword.Click += new System.EventHandler(this.btnTogglePassword_Click);

            // lblUsername: Nhãn hiển thị tên người dùng
            this.lblUsername = new System.Windows.Forms.Label()
            {
                AutoSize = true,
                Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold),
                Location = new System.Drawing.Point(54, 60),
                Name = "lblUsername",
                Size = new System.Drawing.Size(180, 25),
                TabIndex = 1,
                Text = "Tên người dùng: ..."
            };

            // lblEmail: Nhãn hiển thị địa chỉ email
            this.lblEmail = new System.Windows.Forms.Label()
            {
                AutoSize = true,
                Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold),
                Location = new System.Drawing.Point(54, 100),
                Name = "lblEmail",
                Size = new System.Drawing.Size(84, 25),
                TabIndex = 2,
                Text = "Email: ..."
            };

            // lblLastOnline: Nhãn hiển thị lần cuối online
            this.lblLastOnline = new System.Windows.Forms.Label()
            {
                AutoSize = true,
                Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold),
                Location = new System.Drawing.Point(54, 178),
                Name = "lblLastOnline",
                Size = new System.Drawing.Size(173, 25),
                TabIndex = 3,
                Text = "Lần cuối online: ..."
            };

            // lblStats: Nhãn hiển thị thống kê thắng/thua/hòa
            this.lblStats = new System.Windows.Forms.Label()
            {
                AutoSize = true,
                Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold),
                Location = new System.Drawing.Point(54, 218),
                Name = "lblStats",
                Size = new System.Drawing.Size(323, 25),
                TabIndex = 4,
                Text = "Thắng: 0 | Thua: 0 | Hòa: 0"
            };

            // btnClose: Nút đóng form
            this.btnClose = new System.Windows.Forms.Button()
            {
                BackColor = System.Drawing.Color.FromArgb(192, 192, 255),
                Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold),
                Location = new System.Drawing.Point(211, 313),
                Name = "btnClose",
                Size = new System.Drawing.Size(100, 40),
                TabIndex = 7,
                Text = "Đóng",
                UseVisualStyleBackColor = false
            };
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);

            // Thiết lập chung cho Form
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::DoAnMonHocNT106.Properties.Resources._4768220eda2bdf16308f85bc566d46f7;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(520, 377);
            this.Controls.Add(this.btnChangeInfo);
            this.Controls.Add(this.btnTogglePassword);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.lblUsername);
            this.Controls.Add(this.lblEmail);
            this.Controls.Add(this.lblLastOnline);
            this.Controls.Add(this.lblStats);
            this.Controls.Add(this.btnClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form_PlayerInfo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Thông tin người chơi";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
    }
}
