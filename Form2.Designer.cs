namespace GameCaro_Menu
{
    partial class Form2
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnBackgroundMusic = new System.Windows.Forms.Button();
            this.btnGameSound = new System.Windows.Forms.Button();
            this.btnPlayerInfo = new System.Windows.Forms.Button();
            this.btnGameIntro = new System.Windows.Forms.Button();
            this.btnLeaderboard = new System.Windows.Forms.Button();
            this.btnBack = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnBackgroundMusic
            // 
            this.btnBackgroundMusic.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold);
            this.btnBackgroundMusic.ForeColor = System.Drawing.Color.BlueViolet;
            this.btnBackgroundMusic.Location = new System.Drawing.Point(179, 227);
            this.btnBackgroundMusic.Name = "btnBackgroundMusic";
            this.btnBackgroundMusic.Size = new System.Drawing.Size(237, 40);
            this.btnBackgroundMusic.TabIndex = 0;
            this.btnBackgroundMusic.Text = "Toggle Background Music";
            this.btnBackgroundMusic.UseVisualStyleBackColor = true;
            this.btnBackgroundMusic.Click += new System.EventHandler(this.btnBackgroundMusic_Click);
            // 
            // btnGameSound
            // 
            this.btnGameSound.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold);
            this.btnGameSound.ForeColor = System.Drawing.Color.BlueViolet;
            this.btnGameSound.Location = new System.Drawing.Point(179, 300);
            this.btnGameSound.Name = "btnGameSound";
            this.btnGameSound.Size = new System.Drawing.Size(237, 40);
            this.btnGameSound.TabIndex = 1;
            this.btnGameSound.Text = "Toggle Game Sound";
            this.btnGameSound.UseVisualStyleBackColor = true;
            this.btnGameSound.Click += new System.EventHandler(this.btnGameSound_Click);
            // 
            // btnPlayerInfo
            // 
            this.btnPlayerInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold);
            this.btnPlayerInfo.ForeColor = System.Drawing.Color.BlueViolet;
            this.btnPlayerInfo.Location = new System.Drawing.Point(179, 87);
            this.btnPlayerInfo.Name = "btnPlayerInfo";
            this.btnPlayerInfo.Size = new System.Drawing.Size(237, 40);
            this.btnPlayerInfo.TabIndex = 2;
            this.btnPlayerInfo.Text = "Player Information";
            this.btnPlayerInfo.UseVisualStyleBackColor = true;
            this.btnPlayerInfo.Click += new System.EventHandler(this.btnPlayerInfo_Click);
            // 
            // btnGameIntro
            // 
            this.btnGameIntro.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold);
            this.btnGameIntro.ForeColor = System.Drawing.Color.BlueViolet;
            this.btnGameIntro.Location = new System.Drawing.Point(179, 382);
            this.btnGameIntro.Name = "btnGameIntro";
            this.btnGameIntro.Size = new System.Drawing.Size(237, 40);
            this.btnGameIntro.TabIndex = 3;
            this.btnGameIntro.Text = "Game Introduction";
            this.btnGameIntro.UseVisualStyleBackColor = true;
            this.btnGameIntro.Click += new System.EventHandler(this.btnGameIntro_Click);
            // 
            // btnLeaderboard
            // 
            this.btnLeaderboard.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold);
            this.btnLeaderboard.ForeColor = System.Drawing.Color.BlueViolet;
            this.btnLeaderboard.Location = new System.Drawing.Point(179, 154);
            this.btnLeaderboard.Name = "btnLeaderboard";
            this.btnLeaderboard.Size = new System.Drawing.Size(237, 40);
            this.btnLeaderboard.TabIndex = 4;
            this.btnLeaderboard.Text = "Leaderboard";
            this.btnLeaderboard.UseVisualStyleBackColor = true;
            this.btnLeaderboard.Click += new System.EventHandler(this.btnLeaderboard_Click);
            // 
            // btnBack
            // 
            this.btnBack.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold);
            this.btnBack.ForeColor = System.Drawing.Color.BlueViolet;
            this.btnBack.Location = new System.Drawing.Point(179, 455);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(237, 40);
            this.btnBack.TabIndex = 5;
            this.btnBack.Text = "Back";
            this.btnBack.UseVisualStyleBackColor = true;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::GameCaro_Menu.Properties.Resources._2025_06_01_10_34_02;
            this.ClientSize = new System.Drawing.Size(591, 542);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.btnLeaderboard);
            this.Controls.Add(this.btnGameIntro);
            this.Controls.Add(this.btnPlayerInfo);
            this.Controls.Add(this.btnGameSound);
            this.Controls.Add(this.btnBackgroundMusic);
            this.Name = "Form2";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Game Caro Settings";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnBackgroundMusic;
        private System.Windows.Forms.Button btnGameSound;
        private System.Windows.Forms.Button btnPlayerInfo;
        private System.Windows.Forms.Button btnGameIntro;
        private System.Windows.Forms.Button btnLeaderboard;
        private System.Windows.Forms.Button btnBack;
    }
}