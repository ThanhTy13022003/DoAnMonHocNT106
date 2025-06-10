namespace DoAnMonHocNT106
{
    partial class FormSetting
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Button btnToggleMusic;
        private System.Windows.Forms.Button btnToggleSound;
        private System.Windows.Forms.Button btnPlayerInfo;
        private System.Windows.Forms.Button btnIntroducing;
        private System.Windows.Forms.TrackBar trackBarMusicVolume;
        private System.Windows.Forms.TrackBar trackBarSoundVolume;
        private System.Windows.Forms.Button btnLeaderboard;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.btnToggleMusic = new System.Windows.Forms.Button();
            this.btnToggleSound = new System.Windows.Forms.Button();
            this.btnPlayerInfo = new System.Windows.Forms.Button();
            this.btnIntroducing = new System.Windows.Forms.Button();
            this.btnLeaderboard = new System.Windows.Forms.Button();
            this.trackBarMusicVolume = new System.Windows.Forms.TrackBar();
            this.trackBarSoundVolume = new System.Windows.Forms.TrackBar();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarMusicVolume)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarSoundVolume)).BeginInit();
            this.SuspendLayout();
            // 
            // btnToggleMusic
            // 
            this.btnToggleMusic.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.btnToggleMusic.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnToggleMusic.Location = new System.Drawing.Point(33, 306);
            this.btnToggleMusic.Name = "btnToggleMusic";
            this.btnToggleMusic.Size = new System.Drawing.Size(193, 57);
            this.btnToggleMusic.TabIndex = 0;
            this.btnToggleMusic.Text = "Bật/Tắt Nhạc Nền";
            this.btnToggleMusic.UseVisualStyleBackColor = false;
            this.btnToggleMusic.Click += new System.EventHandler(this.btnToggleMusic_Click);
            // 
            // btnToggleSound
            // 
            this.btnToggleSound.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.btnToggleSound.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnToggleSound.Location = new System.Drawing.Point(33, 387);
            this.btnToggleSound.Name = "btnToggleSound";
            this.btnToggleSound.Size = new System.Drawing.Size(193, 57);
            this.btnToggleSound.TabIndex = 1;
            this.btnToggleSound.Text = "Bật/Tắt Âm Thanh";
            this.btnToggleSound.UseVisualStyleBackColor = false;
            this.btnToggleSound.Click += new System.EventHandler(this.btnToggleSound_Click);
            // 
            // btnPlayerInfo
            // 
            this.btnPlayerInfo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.btnPlayerInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold);
            this.btnPlayerInfo.Location = new System.Drawing.Point(162, 39);
            this.btnPlayerInfo.Name = "btnPlayerInfo";
            this.btnPlayerInfo.Size = new System.Drawing.Size(193, 57);
            this.btnPlayerInfo.TabIndex = 2;
            this.btnPlayerInfo.Text = "Thông tin người chơi";
            this.btnPlayerInfo.UseVisualStyleBackColor = false;
            this.btnPlayerInfo.Click += new System.EventHandler(this.btnPlayerInfo_Click);
            // 
            // btnIntroducing
            // 
            this.btnIntroducing.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.btnIntroducing.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnIntroducing.Location = new System.Drawing.Point(162, 207);
            this.btnIntroducing.Name = "btnIntroducing";
            this.btnIntroducing.Size = new System.Drawing.Size(193, 57);
            this.btnIntroducing.TabIndex = 3;
            this.btnIntroducing.Text = "Giới thiệu";
            this.btnIntroducing.UseVisualStyleBackColor = false;
            this.btnIntroducing.Click += new System.EventHandler(this.btnIntroducing_Click);
            // 
            // btnLeaderboard
            // 
            this.btnLeaderboard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.btnLeaderboard.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold);
            this.btnLeaderboard.Location = new System.Drawing.Point(162, 121);
            this.btnLeaderboard.Name = "btnLeaderboard";
            this.btnLeaderboard.Size = new System.Drawing.Size(193, 57);
            this.btnLeaderboard.TabIndex = 8;
            this.btnLeaderboard.Text = "Bảng xếp hạng";
            this.btnLeaderboard.UseVisualStyleBackColor = false;
            this.btnLeaderboard.Click += new System.EventHandler(this.btnLeaderboard_Click);
            // 
            // trackBarMusicVolume
            // 
            this.trackBarMusicVolume.BackColor = System.Drawing.SystemColors.ControlText;
            this.trackBarMusicVolume.Location = new System.Drawing.Point(270, 306);
            this.trackBarMusicVolume.Maximum = 70;
            this.trackBarMusicVolume.Name = "trackBarMusicVolume";
            this.trackBarMusicVolume.Size = new System.Drawing.Size(193, 56);
            this.trackBarMusicVolume.TabIndex = 4;
            this.trackBarMusicVolume.TickFrequency = 10;
            this.trackBarMusicVolume.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.trackBarMusicVolume.Scroll += new System.EventHandler(this.trackBarMusicVolume_Scroll);
            // 
            // trackBarSoundVolume
            // 
            this.trackBarSoundVolume.BackColor = System.Drawing.SystemColors.MenuText;
            this.trackBarSoundVolume.Location = new System.Drawing.Point(270, 388);
            this.trackBarSoundVolume.Maximum = 70;
            this.trackBarSoundVolume.Name = "trackBarSoundVolume";
            this.trackBarSoundVolume.Size = new System.Drawing.Size(193, 56);
            this.trackBarSoundVolume.TabIndex = 5;
            this.trackBarSoundVolume.TickFrequency = 10;
            this.trackBarSoundVolume.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.trackBarSoundVolume.Scroll += new System.EventHandler(this.trackBarSoundVolume_Scroll);
            // 
            // FormSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::DoAnMonHocNT106.Properties.Resources._4768220eda2bdf16308f85bc566d46f7;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(497, 494);
            this.Controls.Add(this.btnLeaderboard);
            this.Controls.Add(this.btnToggleMusic);
            this.Controls.Add(this.btnToggleSound);
            this.Controls.Add(this.btnPlayerInfo);
            this.Controls.Add(this.btnIntroducing);
            this.Controls.Add(this.trackBarMusicVolume);
            this.Controls.Add(this.trackBarSoundVolume);
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