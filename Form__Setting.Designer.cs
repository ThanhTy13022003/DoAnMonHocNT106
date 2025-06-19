// FormSetting.Designer.cs
// File này dùng để thiết lập các tùy chọn cài đặt cho game:
// bao gồm âm lượng nhạc nền, âm thanh hiệu ứng,
// và các thông tin khác liên quan đến người chơi.

namespace DoAnMonHocNT106
{
    partial class FormSetting
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Button để bật/tắt nhạc nền trong game
        /// </summary>
        private System.Windows.Forms.Button btnToggleMusic;

        /// <summary>
        /// Button để bật/tắt âm thanh hiệu ứng
        /// </summary>
        private System.Windows.Forms.Button btnToggleSound;

        /// <summary>
        /// Button hiển thị thông tin người chơi (điểm, cấp độ...)
        /// </summary>
        private System.Windows.Forms.Button btnPlayerInfo;

        /// <summary>
        /// Button hiển thị thông tin giới thiệu game hoặc hướng dẫn sử dụng
        /// </summary>
        private System.Windows.Forms.Button btnIntroducing;

        /// <summary>
        /// Button để xem bảng xếp hạng người chơi
        /// </summary>
        private System.Windows.Forms.Button btnLeaderboard;

        /// <summary>
        /// TrackBar điều chỉnh âm lượng nhạc nền
        /// </summary>
        private System.Windows.Forms.TrackBar trackBarMusicVolume;

        /// <summary>
        /// TrackBar điều chỉnh âm lượng âm thanh hiệu ứng
        /// </summary>
        private System.Windows.Forms.TrackBar trackBarSoundVolume;

        /// <summary>
        /// Giải phóng tài nguyên khi form bị đóng
        /// </summary>
        /// <param name="disposing">true nếu dispose managed resources; ngược lại false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Phương thức khởi tạo và cấu hình các Control trên form
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSetting));

            // btnToggleMusic (Bật/Tắt Nhạc Nền)
            this.btnToggleMusic = new System.Windows.Forms.Button();
            this.btnToggleMusic.BackColor = System.Drawing.Color.FromArgb(192, 192, 255);
            this.btnToggleMusic.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold);
            this.btnToggleMusic.Location = new System.Drawing.Point(51, 350);
            this.btnToggleMusic.Name = "btnToggleMusic";
            this.btnToggleMusic.Size = new System.Drawing.Size(193, 57);
            this.btnToggleMusic.TabIndex = 0;
            this.btnToggleMusic.Text = "Bật/Tắt Nhạc Nền";
            this.btnToggleMusic.UseVisualStyleBackColor = false;
            this.btnToggleMusic.Click += new System.EventHandler(this.btnToggleMusic_Click);

            // btnToggleSound (Bật/Tắt Âm Thanh)
            this.btnToggleSound = new System.Windows.Forms.Button();
            this.btnToggleSound.BackColor = System.Drawing.Color.FromArgb(192, 192, 255);
            this.btnToggleSound.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold);
            this.btnToggleSound.Location = new System.Drawing.Point(51, 431);
            this.btnToggleSound.Name = "btnToggleSound";
            this.btnToggleSound.Size = new System.Drawing.Size(193, 57);
            this.btnToggleSound.TabIndex = 1;
            this.btnToggleSound.Text = "Bật/Tắt Âm Thanh";
            this.btnToggleSound.UseVisualStyleBackColor = false;
            this.btnToggleSound.Click += new System.EventHandler(this.btnToggleSound_Click);

            // btnPlayerInfo (Thông tin người chơi)
            this.btnPlayerInfo = new System.Windows.Forms.Button();
            this.btnPlayerInfo.BackColor = System.Drawing.Color.FromArgb(192, 192, 255);
            this.btnPlayerInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold);
            this.btnPlayerInfo.Location = new System.Drawing.Point(164, 84);
            this.btnPlayerInfo.Name = "btnPlayerInfo";
            this.btnPlayerInfo.Size = new System.Drawing.Size(193, 57);
            this.btnPlayerInfo.TabIndex = 2;
            this.btnPlayerInfo.Text = "Thông tin người chơi";
            this.btnPlayerInfo.UseVisualStyleBackColor = false;
            this.btnPlayerInfo.Click += new System.EventHandler(this.btnPlayerInfo_Click);

            // btnLeaderboard (Bảng xếp hạng)
            this.btnLeaderboard = new System.Windows.Forms.Button();
            this.btnLeaderboard.BackColor = System.Drawing.Color.FromArgb(192, 192, 255);
            this.btnLeaderboard.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold);
            this.btnLeaderboard.Location = new System.Drawing.Point(164, 169);
            this.btnLeaderboard.Name = "btnLeaderboard";
            this.btnLeaderboard.Size = new System.Drawing.Size(193, 57);
            this.btnLeaderboard.TabIndex = 3;
            this.btnLeaderboard.Text = "Bảng xếp hạng";
            this.btnLeaderboard.UseVisualStyleBackColor = false;
            this.btnLeaderboard.Click += new System.EventHandler(this.btnLeaderboard_Click);

            // btnIntroducing (Giới thiệu)
            this.btnIntroducing = new System.Windows.Forms.Button();
            this.btnIntroducing.BackColor = System.Drawing.Color.FromArgb(192, 192, 255);
            this.btnIntroducing.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold);
            this.btnIntroducing.Location = new System.Drawing.Point(164, 256);
            this.btnIntroducing.Name = "btnIntroducing";
            this.btnIntroducing.Size = new System.Drawing.Size(193, 57);
            this.btnIntroducing.TabIndex = 4;
            this.btnIntroducing.Text = "Giới thiệu";
            this.btnIntroducing.UseVisualStyleBackColor = false;
            this.btnIntroducing.Click += new System.EventHandler(this.btnIntroducing_Click);

            // trackBarMusicVolume (Thanh điều chỉnh âm lượng nhạc nền)
            this.trackBarMusicVolume = new System.Windows.Forms.TrackBar();
            this.trackBarMusicVolume.BackColor = System.Drawing.SystemColors.ControlText;
            this.trackBarMusicVolume.Location = new System.Drawing.Point(288, 350);
            this.trackBarMusicVolume.Maximum = 70;
            this.trackBarMusicVolume.Name = "trackBarMusicVolume";
            this.trackBarMusicVolume.Size = new System.Drawing.Size(193, 56);
            this.trackBarMusicVolume.TabIndex = 5;
            this.trackBarMusicVolume.TickFrequency = 10;
            this.trackBarMusicVolume.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.trackBarMusicVolume.Scroll += new System.EventHandler(this.trackBarMusicVolume_Scroll);

            // trackBarSoundVolume (Thanh điều chỉnh âm lượng âm thanh hiệu ứng)
            this.trackBarSoundVolume = new System.Windows.Forms.TrackBar();
            this.trackBarSoundVolume.BackColor = System.Drawing.SystemColors.MenuText;
            this.trackBarSoundVolume.Location = new System.Drawing.Point(288, 432);
            this.trackBarSoundVolume.Maximum = 70;
            this.trackBarSoundVolume.Name = "trackBarSoundVolume";
            this.trackBarSoundVolume.Size = new System.Drawing.Size(193, 56);
            this.trackBarSoundVolume.TabIndex = 6;
            this.trackBarSoundVolume.TickFrequency = 10;
            this.trackBarSoundVolume.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.trackBarSoundVolume.Scroll += new System.EventHandler(this.trackBarSoundVolume_Scroll);

            // FormSetting (Thiết lập các thuộc tính chính cho form Cài Đặt)
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::DoAnMonHocNT106.Properties.Resources._4768220eda2bdf16308f85bc566d46f7;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(525, 528);
            this.Controls.Add(this.btnToggleMusic);
            this.Controls.Add(this.btnToggleSound);
            this.Controls.Add(this.btnPlayerInfo);
            this.Controls.Add(this.btnLeaderboard);
            this.Controls.Add(this.btnIntroducing);
            this.Controls.Add(this.trackBarMusicVolume);
            this.Controls.Add(this.trackBarSoundVolume);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormSetting";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Cài Đặt";
            this.Load += new System.EventHandler(this.FormSetting_Load);
            ((System.ComponentModel.ISupportInitialize)(this.trackBarMusicVolume)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarSoundVolume)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
