// Form_ChangeInfo.Designer.cs
// Thiết lập giao diện cho form thay đổi thông tin người dùng.
// Bao gồm: ô nhập tên người dùng mới, mật khẩu mới và nút xác nhận cập nhật.

using System.Drawing;

namespace DoAnMonHocNT106
{
    partial class Form_ChangeInfo : System.Windows.Forms.Form
    {
        /// <summary>
        /// Container để chứa các thành phần giao diện, cần được giải phóng khi không sử dụng nữa.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Giải phóng tài nguyên đang sử dụng.
        /// </summary>
        /// <param name="disposing">true nếu cần giải phóng tài nguyên quản lý; ngược lại là false.</param>
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
        /// Khởi tạo và cấu hình các control trên form.
        /// Gồm: TextBox cho tên người dùng mới, TextBox cho mật khẩu mới và Button xác nhận.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_ChangeInfo));

            this.txtNewUsername = new System.Windows.Forms.TextBox();
            this.txtNewPassword = new System.Windows.Forms.TextBox();
            this.btnConfirm = new System.Windows.Forms.Button();
            this.SuspendLayout();

            // 
            // txtNewUsername
            // Ô nhập tên người dùng mới
            // 
            this.txtNewUsername.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            this.txtNewUsername.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtNewUsername.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtNewUsername.ForeColor = System.Drawing.Color.White;
            this.txtNewUsername.Location = new System.Drawing.Point(72, 62);
            this.txtNewUsername.Name = "txtNewUsername";
            this.txtNewUsername.Size = new System.Drawing.Size(265, 25);
            this.txtNewUsername.TabIndex = 2;

            // 
            // txtNewPassword
            // Ô nhập mật khẩu mới
            // 
            this.txtNewPassword.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            this.txtNewPassword.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtNewPassword.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtNewPassword.ForeColor = System.Drawing.Color.Gray;
            this.txtNewPassword.Location = new System.Drawing.Point(72, 110);
            this.txtNewPassword.Name = "txtNewPassword";
            this.txtNewPassword.Size = new System.Drawing.Size(265, 25);
            this.txtNewPassword.TabIndex = 1;
            this.txtNewPassword.Text = "Mật khẩu mới"; // Gợi ý văn bản ban đầu

            // 
            // btnConfirm
            // Nút xác nhận cập nhật thông tin
            // 
            this.btnConfirm.BackColor = System.Drawing.Color.SteelBlue;
            this.btnConfirm.FlatAppearance.BorderSize = 0;
            this.btnConfirm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConfirm.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Bold);
            this.btnConfirm.ForeColor = System.Drawing.Color.White;
            this.btnConfirm.Location = new System.Drawing.Point(211, 157);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(126, 45);
            this.btnConfirm.TabIndex = 0;
            this.btnConfirm.Text = "CẬP NHẬT";
            this.btnConfirm.UseVisualStyleBackColor = false;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);

            // 
            // Form_ChangeInfo
            // Thiết lập chung cho Form thay đổi thông tin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::DoAnMonHocNT106.Properties.Resources.sign_up; // Ảnh nền form
            this.ClientSize = new System.Drawing.Size(400, 246);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.txtNewPassword);
            this.Controls.Add(this.txtNewUsername);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon"))); // Biểu tượng ứng dụng
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form_ChangeInfo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen; // Hiển thị giữa màn hình
            this.Text = "Thay đổi thông tin";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        // Các thành phần giao diện được khai báo ở đây để sử dụng trong toàn bộ lớp
        private System.Windows.Forms.TextBox txtNewUsername;
        private System.Windows.Forms.TextBox txtNewPassword;
        private System.Windows.Forms.Button btnConfirm;
    }
}
