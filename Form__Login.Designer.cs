namespace DoAnMonHocNT106
{
    partial class Login
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Login));
            this.username = new System.Windows.Forms.TextBox();
            this.password = new System.Windows.Forms.TextBox();
            this.signin = new System.Windows.Forms.Button();
            this.BTsignup = new System.Windows.Forms.Button();
            this.forgotpw = new System.Windows.Forms.Label();
            this.show = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.usererro = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // username
            // 
            this.username.Font = new System.Drawing.Font("Times New Roman", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.username.ForeColor = System.Drawing.Color.Gray;
            this.username.Location = new System.Drawing.Point(87, 120);
            this.username.Name = "username";
            this.username.Size = new System.Drawing.Size(294, 28);
            this.username.TabIndex = 2;
            this.username.Text = "User Name Or Email";
            this.username.TextChanged += new System.EventHandler(this.Username_Placeholder);
            // 
            // password
            // 
            this.password.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.password.ForeColor = System.Drawing.Color.Gray;
            this.password.Location = new System.Drawing.Point(87, 195);
            this.password.Name = "password";
            this.password.Size = new System.Drawing.Size(294, 27);
            this.password.TabIndex = 3;
            this.password.Text = "Pass Word";
            this.password.UseSystemPasswordChar = true;
            this.password.TextChanged += new System.EventHandler(this.Password_Placeholder);
            // 
            // signin
            // 
            this.signin.BackColor = System.Drawing.Color.DarkSlateBlue;
            this.signin.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.signin.ForeColor = System.Drawing.Color.White;
            this.signin.Location = new System.Drawing.Point(87, 286);
            this.signin.Name = "signin";
            this.signin.Size = new System.Drawing.Size(120, 50);
            this.signin.TabIndex = 4;
            this.signin.Text = "Sign In";
            this.signin.UseVisualStyleBackColor = false;
            this.signin.Click += new System.EventHandler(this.signin_Click);
            // 
            // BTsignup
            // 
            this.BTsignup.BackColor = System.Drawing.Color.DarkSlateBlue;
            this.BTsignup.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTsignup.ForeColor = System.Drawing.Color.White;
            this.BTsignup.Location = new System.Drawing.Point(261, 286);
            this.BTsignup.Name = "BTsignup";
            this.BTsignup.Size = new System.Drawing.Size(120, 50);
            this.BTsignup.TabIndex = 5;
            this.BTsignup.Text = "Sign Up";
            this.BTsignup.UseVisualStyleBackColor = false;
            this.BTsignup.Click += new System.EventHandler(this.BTsignup_Click);
            // 
            // forgotpw
            // 
            this.forgotpw.AutoSize = true;
            this.forgotpw.BackColor = System.Drawing.Color.White;
            this.forgotpw.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.forgotpw.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.forgotpw.Location = new System.Drawing.Point(172, 357);
            this.forgotpw.Name = "forgotpw";
            this.forgotpw.Size = new System.Drawing.Size(131, 19);
            this.forgotpw.TabIndex = 6;
            this.forgotpw.Text = "Forgot Password?";
            this.forgotpw.Click += new System.EventHandler(this.forgotpw_Click);
            // 
            // show
            // 
            this.show.AutoSize = true;
            this.show.BackColor = System.Drawing.Color.White;
            this.show.ForeColor = System.Drawing.SystemColors.GrayText;
            this.show.Location = new System.Drawing.Point(257, 238);
            this.show.Name = "show";
            this.show.Size = new System.Drawing.Size(124, 20);
            this.show.TabIndex = 8;
            this.show.Text = "Show password";
            this.show.UseVisualStyleBackColor = false;
            this.show.CheckedChanged += new System.EventHandler(this.show_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Gainsboro;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 28.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label1.Location = new System.Drawing.Point(139, 46);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(168, 53);
            this.label1.TabIndex = 9;
            this.label1.Text = "Sign In";
            // 
            // usererro
            // 
            this.usererro.AutoSize = true;
            this.usererro.ForeColor = System.Drawing.Color.Gray;
            this.usererro.Location = new System.Drawing.Point(84, 161);
            this.usererro.Name = "usererro";
            this.usererro.Size = new System.Drawing.Size(0, 16);
            this.usererro.TabIndex = 10;
            // 
            // Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DarkGray;
            this.BackgroundImage = global::DoAnMonHocNT106.Properties.Resources.register_form_01;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.usererro);
            this.Controls.Add(this.show);
            this.Controls.Add(this.forgotpw);
            this.Controls.Add(this.BTsignup);
            this.Controls.Add(this.signin);
            this.Controls.Add(this.password);
            this.Controls.Add(this.username);
            this.Controls.Add(this.label1);
            this.DoubleBuffered = true;
            this.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Login";
            this.Text = "Login";
            this.Load += new System.EventHandler(this.Login_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox username;
        private System.Windows.Forms.TextBox password;
        internal System.Windows.Forms.Button signin;
        internal System.Windows.Forms.Button BTsignup;
        private System.Windows.Forms.Label forgotpw;
        private System.Windows.Forms.CheckBox show;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label usererro;
    }
}