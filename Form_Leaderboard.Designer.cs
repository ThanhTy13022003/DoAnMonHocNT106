// Form_Leaderboard.Designer.cs
// Thiết lập giao diện cho form Bảng xếp hạng (Leaderboard):
// Bao gồm ListView hiển thị thứ hạng, tên người dùng, thống kê trận,
// nhãn tiêu đề và nút đóng form.

namespace DoAnMonHocNT106
{
    partial class Form_Leaderboard
    {
        /// <summary>
        /// Biến chứa các component cần dispose khi không sử dụng nữa.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.ListView lstLeaderboard; // ListView chứa dữ liệu bảng xếp hạng
        private System.Windows.Forms.Button btnClose;         // Nút đóng form
        private System.Windows.Forms.Label lblTitle;          // Nhãn tiêu đề bảng xếp hạng

        /// <summary>
        /// Giải phóng tài nguyên đang sử dụng.
        /// </summary>
        /// <param name="disposing">
        /// true nếu tài nguyên quản lý cần được dispose, ngược lại false.
        /// </param>
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
            // Resource manager để lấy icon, hình ảnh nền...
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_Leaderboard));

            // Khởi tạo ListView hiển thị bảng xếp hạng
            this.lstLeaderboard = new System.Windows.Forms.ListView()
            {
                FullRowSelect = true,     // chọn cả hàng
                GridLines = true,         // hiển thị lưới
                HideSelection = false,    // không ẩn khi mất focus
                Location = new System.Drawing.Point(24, 72),
                Margin = new System.Windows.Forms.Padding(2),
                Name = "lstLeaderboard",
                Size = new System.Drawing.Size(553, 304),
                TabIndex = 0,
                UseCompatibleStateImageBehavior = false,
                View = System.Windows.Forms.View.Details // chế độ chi tiết
            };
            this.lstLeaderboard.SelectedIndexChanged += new System.EventHandler(this.lstLeaderboard_SelectedIndexChanged);

            // Khởi tạo nút Đóng form
            this.btnClose = new System.Windows.Forms.Button()
            {
                BackColor = System.Drawing.Color.IndianRed,                                // màu nền đỏ
                Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.White,                                    // chữ trắng
                Location = new System.Drawing.Point(257, 392),
                Margin = new System.Windows.Forms.Padding(2),
                Name = "btnClose",
                Size = new System.Drawing.Size(86, 35),
                TabIndex = 1,
                Text = "Đóng",
                UseVisualStyleBackColor = false
            };
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);

            // Khởi tạo Label tiêu đề
            this.lblTitle = new System.Windows.Forms.Label()
            {
                AutoSize = true,                                                         // tự động kích thước
                BackColor = System.Drawing.Color.Silver,                                  // nền xám
                Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.Black,                                   // chữ đen
                Location = new System.Drawing.Point(24, 37),
                Margin = new System.Windows.Forms.Padding(2, 0, 2, 0),
                Name = "lblTitle",
                Size = new System.Drawing.Size(146, 25),
                TabIndex = 2,
                Text = "Bảng xếp hạng"
            };

            // Cấu hình Form chính
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::DoAnMonHocNT106.Properties.Resources._4768220eda2bdf16308f85bc566d46f7; // ảnh nền
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;          // căng nền
            this.ClientSize = new System.Drawing.Size(604, 447);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lstLeaderboard);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;       // không thay đổi kích thước
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));       // icon ứng dụng
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;                                                       // vô hiệu hóa Maximize
            this.Name = "Form_Leaderboard";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;       // mở giữa màn hình
            this.Text = "Bảng xếp hạng";                                                   // tiêu đề cửa sổ
            this.ResumeLayout(false);
            this.PerformLayout(); // áp dụng layout
        }

        #endregion
    }
}
