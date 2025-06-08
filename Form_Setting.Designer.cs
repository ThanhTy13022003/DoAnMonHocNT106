namespace DoAnMonHocNT106
{
    partial class FormSetting
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Button btnToggleMusic;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private System.Windows.Forms.Button btnToggleSound;
        private System.Windows.Forms.TrackBar trackBarMusicVolume;
        private System.Windows.Forms.TrackBar trackBarSoundVolume;
        private System.Windows.Forms.Label lblMusicVolume;
        private System.Windows.Forms.Label lblSoundVolume;

        private void InitializeComponent()
        {
            this.btnToggleMusic = new System.Windows.Forms.Button();
            this.btnToggleSound = new System.Windows.Forms.Button();
            this.trackBarMusicVolume = new System.Windows.Forms.TrackBar();
            this.trackBarSoundVolume = new System.Windows.Forms.TrackBar();
            this.lblMusicVolume = new System.Windows.Forms.Label();
            this.lblSoundVolume = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarMusicVolume)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarSoundVolume)).BeginInit();
            this.SuspendLayout();
            // 
            // btnToggleMusic
            // 
            this.btnToggleMusic.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.btnToggleMusic.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnToggleMusic.Location = new System.Drawing.Point(103, 30);
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
            this.btnToggleSound.Location = new System.Drawing.Point(103, 100);
            this.btnToggleSound.Name = "btnToggleSound";
            this.btnToggleSound.Size = new System.Drawing.Size(193, 57);
            this.btnToggleSound.TabIndex = 1;
            this.btnToggleSound.Text = "Bật/Tắt Âm Thanh";
            this.btnToggleSound.UseVisualStyleBackColor = false;
            this.btnToggleSound.Click += new System.EventHandler(this.btnToggleSound_Click);
            // 
            // trackBarMusicVolume
            // 
            this.trackBarMusicVolume.BackColor = System.Drawing.SystemColors.ControlText;
            this.trackBarMusicVolume.Location = new System.Drawing.Point(103, 200);
            this.trackBarMusicVolume.Maximum = 70;
            this.trackBarMusicVolume.Name = "trackBarMusicVolume";
            this.trackBarMusicVolume.Size = new System.Drawing.Size(193, 56);
            this.trackBarMusicVolume.TabIndex = 2;
            this.trackBarMusicVolume.TickFrequency = 10;
            this.trackBarMusicVolume.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.trackBarMusicVolume.Scroll += new System.EventHandler(this.trackBarMusicVolume_Scroll);
            // 
            // trackBarSoundVolume
            // 
            this.trackBarSoundVolume.BackColor = System.Drawing.SystemColors.MenuText;
            this.trackBarSoundVolume.Location = new System.Drawing.Point(99, 300);
            this.trackBarSoundVolume.Maximum = 70;
            this.trackBarSoundVolume.Name = "trackBarSoundVolume";
            this.trackBarSoundVolume.Size = new System.Drawing.Size(193, 56);
            this.trackBarSoundVolume.TabIndex = 3;
            this.trackBarSoundVolume.TickFrequency = 10;
            this.trackBarSoundVolume.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.trackBarSoundVolume.Scroll += new System.EventHandler(this.trackBarSoundVolume_Scroll);
            // 
            // lblMusicVolume
            // 
            this.lblMusicVolume.AutoSize = true;
            this.lblMusicVolume.BackColor = System.Drawing.Color.Transparent;
            this.lblMusicVolume.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMusicVolume.ForeColor = System.Drawing.SystemColors.Control;
            this.lblMusicVolume.Location = new System.Drawing.Point(99, 173);
            this.lblMusicVolume.Name = "lblMusicVolume";
            this.lblMusicVolume.Size = new System.Drawing.Size(200, 24);
            this.lblMusicVolume.TabIndex = 2;
            this.lblMusicVolume.Text = "Âm lượng Nhạc Nền";
            // 
            // lblSoundVolume
            // 
            this.lblSoundVolume.AutoSize = true;
            this.lblSoundVolume.BackColor = System.Drawing.Color.Transparent;
            this.lblSoundVolume.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSoundVolume.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.lblSoundVolume.Location = new System.Drawing.Point(99, 273);
            this.lblSoundVolume.Name = "lblSoundVolume";
            this.lblSoundVolume.Size = new System.Drawing.Size(203, 24);
            this.lblSoundVolume.TabIndex = 3;
            this.lblSoundVolume.Text = "Âm lượng Âm Thanh";
            // 
            // FormSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::DoAnMonHocNT106.Properties.Resources._4768220eda2bdf16308f85bc566d46f7;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(400, 382);
            this.Controls.Add(this.btnToggleMusic);
            this.Controls.Add(this.btnToggleSound);
            this.Controls.Add(this.lblMusicVolume);
            this.Controls.Add(this.trackBarMusicVolume);
            this.Controls.Add(this.lblSoundVolume);
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