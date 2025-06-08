namespace DoAnMonHocNT106
{
    partial class SignUp
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
            this.gm = new System.Windows.Forms.Button();
            this.sdt = new System.Windows.Forms.Button();
            this.username = new System.Windows.Forms.TextBox();
            this.mail = new System.Windows.Forms.TextBox();
            this.pw = new System.Windows.Forms.TextBox();
            this.repw = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.show = new System.Windows.Forms.CheckBox();
            this.fb = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // gm
            // 
            this.gm.BackColor = System.Drawing.Color.DarkRed;
            this.gm.Font = new System.Drawing.Font("Times New Roman", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gm.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.gm.Location = new System.Drawing.Point(43, 213);
            this.gm.Name = "gm";
            this.gm.Size = new System.Drawing.Size(235, 45);
            this.gm.TabIndex = 2;
            this.gm.Text = "Login With Gmail";
            this.gm.UseVisualStyleBackColor = false;
            this.gm.Click += new System.EventHandler(this.gm_Click);
            // 
            // sdt
            // 
            this.sdt.BackColor = System.Drawing.Color.DarkGreen;
            this.sdt.Font = new System.Drawing.Font("Times New Roman", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sdt.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.sdt.Location = new System.Drawing.Point(43, 274);
            this.sdt.Name = "sdt";
            this.sdt.Size = new System.Drawing.Size(235, 40);
            this.sdt.TabIndex = 3;
            this.sdt.Text = "Login With PhoneNumber";
            this.sdt.UseVisualStyleBackColor = false;
            // 
            // username
            // 
            this.username.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.username.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.username.Location = new System.Drawing.Point(398, 111);
            this.username.Name = "username";
            this.username.Size = new System.Drawing.Size(223, 27);
            this.username.TabIndex = 4;
            this.username.Text = "Username";
            // 
            // mail
            // 
            this.mail.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mail.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.mail.Location = new System.Drawing.Point(398, 164);
            this.mail.Name = "mail";
            this.mail.Size = new System.Drawing.Size(223, 27);
            this.mail.TabIndex = 5;
            this.mail.Text = "E-mail";
            // 
            // pw
            // 
            this.pw.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pw.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.pw.Location = new System.Drawing.Point(398, 220);
            this.pw.Name = "pw";
            this.pw.Size = new System.Drawing.Size(223, 27);
            this.pw.TabIndex = 6;
            this.pw.Text = "Password";
            this.pw.UseSystemPasswordChar = true;
            // 
            // repw
            // 
            this.repw.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.repw.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.repw.Location = new System.Drawing.Point(398, 274);
            this.repw.Name = "repw";
            this.repw.Size = new System.Drawing.Size(223, 27);
            this.repw.TabIndex = 7;
            this.repw.Text = "Re-password";
            this.repw.UseSystemPasswordChar = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 28.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(426, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(184, 53);
            this.label1.TabIndex = 8;
            this.label1.Text = "Sign Up";
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.LightSeaGreen;
            this.button1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button1.Location = new System.Drawing.Point(425, 357);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(162, 44);
            this.button1.TabIndex = 9;
            this.button1.Text = "Confirm";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.Image = global::DoAnMonHocNT106.Properties.Resources.Untitled1;
            this.pictureBox1.Location = new System.Drawing.Point(-5, -1);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(354, 422);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // show
            // 
            this.show.AutoSize = true;
            this.show.BackColor = System.Drawing.Color.White;
            this.show.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.show.Location = new System.Drawing.Point(510, 318);
            this.show.Name = "show";
            this.show.Size = new System.Drawing.Size(111, 19);
            this.show.TabIndex = 10;
            this.show.Text = "Show password";
            this.show.UseVisualStyleBackColor = false;
            this.show.CheckedChanged += new System.EventHandler(this.show_CheckedChanged);
            // 
            // fb
            // 
            this.fb.BackColor = System.Drawing.Color.Navy;
            this.fb.Font = new System.Drawing.Font("Times New Roman", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fb.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.fb.Location = new System.Drawing.Point(43, 157);
            this.fb.Name = "fb";
            this.fb.Size = new System.Drawing.Size(235, 40);
            this.fb.TabIndex = 1;
            this.fb.Text = "Login With Facebook";
            this.fb.UseVisualStyleBackColor = false;
            // 
            // SignUp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.PeachPuff;
            this.ClientSize = new System.Drawing.Size(700, 422);
            this.Controls.Add(this.show);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.repw);
            this.Controls.Add(this.pw);
            this.Controls.Add(this.mail);
            this.Controls.Add(this.username);
            this.Controls.Add(this.sdt);
            this.Controls.Add(this.gm);
            this.Controls.Add(this.fb);
            this.Controls.Add(this.pictureBox1);
            this.Font = new System.Drawing.Font("Times New Roman", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "SignUp";
            this.Text = "Form2";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button gm;
        private System.Windows.Forms.Button sdt;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TextBox username;
        private System.Windows.Forms.TextBox mail;
        private System.Windows.Forms.TextBox pw;
        private System.Windows.Forms.TextBox repw;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox show;
        private System.Windows.Forms.Button fb;
    }
}