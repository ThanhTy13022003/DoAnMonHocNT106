// FormPvP.cs
// Xử lý giao diện và logic cho chế độ chơi đối kháng người với người (PvP) trong game Cờ Caro.
// Bao gồm các thành phần: bàn cờ, nút thoát, nút chơi lại, thông tin người chơi và đối thủ, đồng hồ đếm ngược.
// Cho phép người dùng tương tác và cập nhật giao diện trong suốt quá trình chơi.

namespace DoAnMonHocNT106
{
    partial class FormPvP
    {
        // Khai báo thành phần giao diện của Form
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel panelBoard;           // Bàn cờ chính
        private System.Windows.Forms.Button btnBack;             // Nút thoát trò chơi
        private System.Windows.Forms.Button btnRestart;          // Nút chơi lại
        private System.Windows.Forms.Label lblYou;               // Hiển thị người chơi (Bạn)
        private System.Windows.Forms.Label lblOpponent;          // Hiển thị đối thủ
        private System.Windows.Forms.Label lblCountdown;         // Hiển thị thời gian đếm ngược mỗi lượt

        // Hàm hủy để giải phóng tài nguyên đang sử dụng
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        // Khởi tạo và cấu hình các thành phần giao diện của Form
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPvP));
            this.panelBoard = new System.Windows.Forms.Panel();
            this.btnBack = new System.Windows.Forms.Button();
            this.btnRestart = new System.Windows.Forms.Button();
            this.lblYou = new System.Windows.Forms.Label();
            this.lblOpponent = new System.Windows.Forms.Label();
            this.lblCountdown = new System.Windows.Forms.Label();
            this.SuspendLayout();

            // 
            // panelBoard
            // Cấu hình bàn cờ: nền màu tối, viền bao xung quanh, kích thước cố định
            // Bắt sự kiện Paint để vẽ bàn cờ mỗi khi cập nhật
            // 
            this.panelBoard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.panelBoard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelBoard.Location = new System.Drawing.Point(20, 20);
            this.panelBoard.Name = "panelBoard";
            this.panelBoard.Size = new System.Drawing.Size(500, 600);
            this.panelBoard.TabIndex = 0;
            this.panelBoard.Paint += new System.Windows.Forms.PaintEventHandler(this.panelBoard_Paint);

            // 
            // btnBack
            // Nút "Thoát" dùng để quay lại màn hình trước hoặc thoát game
            // Có màu nền đậm, chữ đỏ để nổi bật
            // 
            this.btnBack.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.btnBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBack.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnBack.ForeColor = System.Drawing.Color.Red;
            this.btnBack.Location = new System.Drawing.Point(553, 32);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(120, 40);
            this.btnBack.TabIndex = 1;
            this.btnBack.Text = "❌ Thoát";
            this.btnBack.UseVisualStyleBackColor = false;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);

            // 
            // btnRestart
            // Nút "Chơi lại" để bắt đầu ván mới
            // Màu xanh nổi bật, chữ trắng để tạo cảm giác tích cực
            // 
            this.btnRestart.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(120)))), ((int)(((byte)(80)))));
            this.btnRestart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRestart.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnRestart.ForeColor = System.Drawing.Color.White;
            this.btnRestart.Location = new System.Drawing.Point(553, 78);
            this.btnRestart.Name = "btnRestart";
            this.btnRestart.Size = new System.Drawing.Size(120, 40);
            this.btnRestart.TabIndex = 2;
            this.btnRestart.Text = "🔁 Chơi lại";
            this.btnRestart.UseVisualStyleBackColor = false;
            this.btnRestart.Click += new System.EventHandler(this.btnRestart_Click);

            // 
            // lblYou
            // Hiển thị thông tin người chơi với ký hiệu 'X'
            // 
            this.lblYou.AutoSize = true;
            this.lblYou.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblYou.ForeColor = System.Drawing.Color.Cyan;
            this.lblYou.Location = new System.Drawing.Point(565, 146);
            this.lblYou.Name = "lblYou";
            this.lblYou.Size = new System.Drawing.Size(51, 19);
            this.lblYou.TabIndex = 3;
            this.lblYou.Text = "Bạn: X";

            // 
            // lblOpponent
            // Hiển thị thông tin đối thủ với ký hiệu 'O'
            // 
            this.lblOpponent.AutoSize = true;
            this.lblOpponent.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblOpponent.ForeColor = System.Drawing.Color.Orange;
            this.lblOpponent.Location = new System.Drawing.Point(565, 177);
            this.lblOpponent.Name = "lblOpponent";
            this.lblOpponent.Size = new System.Drawing.Size(76, 19);
            this.lblOpponent.TabIndex = 4;
            this.lblOpponent.Text = "Đối thủ: O";

            // 
            // lblCountdown
            // Hiển thị thời gian đếm ngược mỗi lượt chơi
            // Giúp người chơi biết còn bao nhiêu giây để đi
            // 
            this.lblCountdown.AutoSize = true;
            this.lblCountdown.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblCountdown.ForeColor = System.Drawing.Color.Lime;
            this.lblCountdown.Location = new System.Drawing.Point(565, 217);
            this.lblCountdown.Name = "lblCountdown";
            this.lblCountdown.Size = new System.Drawing.Size(101, 19);
            this.lblCountdown.TabIndex = 5;
            this.lblCountdown.Text = "Thời gian: 20s";

            // 
            // FormPvP
            // Thiết lập thông số chính cho form: nền tối, cố định kích thước, biểu tượng, vị trí hiển thị
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(15)))), ((int)(((byte)(15)))));
            this.BackgroundImage = global::DoAnMonHocNT106.Properties.Resources.pngtree_retro_futuristic_gaming_desk_scene_image_16555312;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(699, 650);
            this.Controls.Add(this.lblCountdown);
            this.Controls.Add(this.lblOpponent);
            this.Controls.Add(this.lblYou);
            this.Controls.Add(this.btnRestart);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.panelBoard);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FormPvP";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "🎮 Cờ Caro PvP - NT106";
            this.Load += new System.EventHandler(this.FormPvP_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
