// Form_Introduce.cs
// Xử lý giao diện và sự kiện cho màn hình giới thiệu trò chơi:
// Bao gồm các thành phần Label cho tiêu đề, nội dung giới thiệu, đội ngũ phát triển,
// hình ảnh minh họa, và các nút điều hướng như đóng form, PlayViaLAN, PlayToBot, cài đặt người chơi.

using System;
using System.Drawing;
using System.Windows.Forms;

namespace DoAnMonHocNT106
{
    public partial class Form_Introduce : Form
    {
        /// <summary>
        /// Biến chứa các component của form, dùng để dispose khi không còn sử dụng.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
        /// Phương thức khởi tạo và thiết lập các control trên form giới thiệu.
        /// </summary>
        private void InitializeComponent()
        {
            // Tạo resource manager để lấy tài nguyên (hình ảnh, icon...)
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_Introduce));

            // Khởi tạo Label tiêu đề
            this.labelTitle = new System.Windows.Forms.Label();
            // Khởi tạo PictureBox hiển thị hình ảnh minh họa
            this.pictureBoxImage = new System.Windows.Forms.PictureBox();
            // Khởi tạo nút Đóng form
            this.buttonClose = new System.Windows.Forms.Button();
            // Khởi tạo các Label nội dung giới thiệu
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            // Khởi tạo các Button chức năng mở form khác
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();

            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxImage)).BeginInit();
            this.SuspendLayout();

            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.BackColor = Color.Transparent;  // nền trong suốt để hiển thị background image
            this.labelTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            this.labelTitle.ForeColor = Color.Aqua;        // màu chữ nổi bật
            this.labelTitle.Location = new Point(27, 25);
            this.labelTitle.Margin = new Padding(4, 0, 4, 0);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new Size(228, 32);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "Giới thiệu trò chơi:";

            // 
            // pictureBoxImage
            // 
            this.pictureBoxImage.Image = ((Image)(resources.GetObject("pictureBoxImage.Image"))); // ảnh minh họa
            this.pictureBoxImage.Location = new Point(710, 341);
            this.pictureBoxImage.Margin = new Padding(4);
            this.pictureBoxImage.Name = "pictureBoxImage";
            this.pictureBoxImage.Size = new Size(221, 168);
            this.pictureBoxImage.SizeMode = PictureBoxSizeMode.StretchImage; // co giãn ảnh
            this.pictureBoxImage.TabIndex = 2;
            this.pictureBoxImage.TabStop = false;

            // 
            // buttonClose
            // 
            this.buttonClose.BackColor = SystemColors.Highlight; // màu nền nút
            this.buttonClose.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.buttonClose.ForeColor = Color.White;              // màu chữ trắng
            this.buttonClose.Location = new Point(760, 547);
            this.buttonClose.Margin = new Padding(4);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new Size(133, 49);
            this.buttonClose.TabIndex = 3;
            this.buttonClose.Text = "Đóng";
            this.buttonClose.UseVisualStyleBackColor = false;
            this.buttonClose.Click += new EventHandler(this.buttonClose_Click);

            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = Color.Transparent;
            this.label1.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = SystemColors.ControlLightLight;
            this.label1.Location = new Point(28, 85);
            this.label1.Name = "label1";
            this.label1.Size = new Size(852, 100);
            this.label1.TabIndex = 4;
            this.label1.Text = resources.GetString("label1.Text"); // nội dung giới thiệu chi tiết
            this.label1.Click += new EventHandler(this.label1_Click);

            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = Color.Transparent;
            this.label2.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            this.label2.ForeColor = Color.Aqua;
            this.label2.Location = new Point(27, 213);
            this.label2.Margin = new Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new Size(232, 32);
            this.label2.TabIndex = 5;
            this.label2.Text = "Đội ngũ phát triển:";

            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = Color.Transparent;
            this.label3.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = SystemColors.ControlLightLight;
            this.label3.Location = new Point(28, 276);
            this.label3.Name = "label3";
            this.label3.Size = new Size(883, 150);
            this.label3.TabIndex = 6;
            this.label3.Text = resources.GetString("label3.Text"); // thông tin nhóm dev
            this.label3.Click += new EventHandler(this.label3_Click);

            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = Color.Transparent;
            this.label4.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            this.label4.ForeColor = Color.Aqua;
            this.label4.Location = new Point(27, 454);
            this.label4.Margin = new Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new Size(137, 32);
            this.label4.TabIndex = 7;
            this.label4.Text = "Cuối cùng:";
            this.label4.Click += new EventHandler(this.label4_Click);

            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = Color.Transparent;
            this.label5.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = SystemColors.ControlLightLight;
            this.label5.Location = new Point(28, 516);
            this.label5.Name = "label5";
            this.label5.Size = new Size(420, 100);
            this.label5.TabIndex = 8;
            this.label5.Text = "Và cuối cùng xin chân thành cảm ơn tới những \r\nngười hâm mộ đã đóng góp ý kiến và công sức \r\nđể chúng tôi có thể hoàn thiện hơn được game này.";

            // 
            // button1: chức năng chơi qua LAN
            // 
            this.button1.Font = new Font("Microsoft Sans Serif", 7.8F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new Point(497, 492);
            this.button1.Name = "button1";
            this.button1.Size = new Size(179, 26);
            this.button1.TabIndex = 9;
            this.button1.Text = "PlayViaLan";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new EventHandler(this.button1_Click);

            // 
            // button2: chức năng chơi với Bot
            // 
            this.button2.Font = new Font("Microsoft Sans Serif", 7.8F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            this.button2.Location = new Point(497, 524);
            this.button2.Name = "button2";
            this.button2.Size = new Size(179, 26);
            this.button2.TabIndex = 10;
            this.button2.Text = "PlayToBot";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new EventHandler(this.button2_Click);

            // 
            // button3: mở form Cài đặt thông tin người chơi
            // 
            this.button3.Font = new Font("Microsoft Sans Serif", 7.8F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            this.button3.Location = new Point(497, 556);
            this.button3.Name = "button3";
            this.button3.Size = new Size(179, 26);
            this.button3.TabIndex = 11;
            this.button3.Text = "SettingInforPlayer";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new EventHandler(this.button3_Click);

            // 
            // button4: hành động bổ sung (ví dụ ...)
            // 
            this.button4.Font = new Font("Microsoft Sans Serif", 7.8F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            this.button4.Location = new Point(497, 588);
            this.button4.Name = "button4";
            this.button4.Size = new Size(179, 26);
            this.button4.TabIndex = 12;
            this.button4.Text = "...";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new EventHandler(this.button4_Click);

            // 
            // Form_Introduce: thiết lập chung cho form
            // 
            this.AutoScaleDimensions = new SizeF(8F, 16F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.WhiteSmoke;  // màu nền chính
            this.BackgroundImage = global::DoAnMonHocNT106.Properties.Resources._4768220eda2bdf16308f85bc566d46f7; // ảnh nền
            this.ClientSize = new Size(979, 643);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.pictureBoxImage);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.labelTitle);
            this.FormBorderStyle = FormBorderStyle.FixedDialog; // không cho resize
            this.Icon = ((Icon)(resources.GetObject("$this.Icon"))); // icon ứng dụng
            this.Margin = new Padding(4);
            this.MaximizeBox = false; // vô hiệu hóa nút phóng to
            this.Name = "Form_Introduce";
            this.StartPosition = FormStartPosition.CenterScreen; // mở form ở giữa màn hình
            this.Text = "Giới thiệu"; // tiêu đề cửa sổ
            this.Load += new EventHandler(this.Form_Introduce_Load);

            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.PictureBox pictureBoxImage;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;

        /// <summary>
        /// Xử lý sự kiện khi người dùng nhấn nút Đóng.
        /// Phát âm thanh click và đóng form.
        /// </summary>
        private void buttonClose_Click(object sender, EventArgs e)
        {
            MusicPlayer.PlayClickSound(); // phát âm thanh
            this.Close();                // đóng form giới thiệu
        }
    }
}
