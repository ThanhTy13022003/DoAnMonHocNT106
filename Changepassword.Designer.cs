namespace DoAnMonHocNT106
{
    partial class Changepassword
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.labelCurrent = new System.Windows.Forms.Label();
            this.labelNew = new System.Windows.Forms.Label();
            this.labelVerify = new System.Windows.Forms.Label();
            this.textBoxCurrent = new System.Windows.Forms.TextBox();
            this.textBoxNew = new System.Windows.Forms.TextBox();
            this.textBoxVerify = new System.Windows.Forms.TextBox();
            this.buttonChange = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelCurrent
            // 
            this.labelCurrent.AutoSize = true;
            this.labelCurrent.Location = new System.Drawing.Point(100, 50);
            this.labelCurrent.Name = "labelCurrent";
            this.labelCurrent.Size = new System.Drawing.Size(113, 16);
            this.labelCurrent.TabIndex = 0;
            this.labelCurrent.Text = "Current Password";
            // 
            // labelNew
            // 
            this.labelNew.AutoSize = true;
            this.labelNew.Location = new System.Drawing.Point(100, 100);
            this.labelNew.Name = "labelNew";
            this.labelNew.Size = new System.Drawing.Size(95, 16);
            this.labelNew.TabIndex = 1;
            this.labelNew.Text = "New Password";
            // 
            // labelVerify
            // 
            this.labelVerify.AutoSize = true;
            this.labelVerify.Location = new System.Drawing.Point(100, 150);
            this.labelVerify.Name = "labelVerify";
            this.labelVerify.Size = new System.Drawing.Size(124, 16);
            this.labelVerify.TabIndex = 2;
            this.labelVerify.Text = "Verify New Password";
            // 
            // textBoxCurrent
            // 
            this.textBoxCurrent.Location = new System.Drawing.Point(250, 47);
            this.textBoxCurrent.Name = "textBoxCurrent";
            this.textBoxCurrent.Size = new System.Drawing.Size(250, 22);
            this.textBoxCurrent.TabIndex = 3;
            this.textBoxCurrent.UseSystemPasswordChar = true;
            // 
            // textBoxNew
            // 
            this.textBoxNew.Location = new System.Drawing.Point(250, 97);
            this.textBoxNew.Name = "textBoxNew";
            this.textBoxNew.Size = new System.Drawing.Size(250, 22);
            this.textBoxNew.TabIndex = 4;
            this.textBoxNew.UseSystemPasswordChar = true;
            // 
            // textBoxVerify
            // 
            this.textBoxVerify.Location = new System.Drawing.Point(250, 147);
            this.textBoxVerify.Name = "textBoxVerify";
            this.textBoxVerify.Size = new System.Drawing.Size(250, 22);
            this.textBoxVerify.TabIndex = 5;
            this.textBoxVerify.UseSystemPasswordChar = true;
            // 
            // buttonChange
            // 
            this.buttonChange.Location = new System.Drawing.Point(250, 200);
            this.buttonChange.Name = "buttonChange";
            this.buttonChange.Size = new System.Drawing.Size(100, 30);
            this.buttonChange.TabIndex = 6;
            this.buttonChange.Text = "Change";
            this.buttonChange.UseVisualStyleBackColor = true;
            this.buttonChange.Click += new System.EventHandler(this.buttonChange_Click);
            // 
            // Changepassword
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 300);
            this.Controls.Add(this.buttonChange);
            this.Controls.Add(this.textBoxVerify);
            this.Controls.Add(this.textBoxNew);
            this.Controls.Add(this.textBoxCurrent);
            this.Controls.Add(this.labelVerify);
            this.Controls.Add(this.labelNew);
            this.Controls.Add(this.labelCurrent);
            this.Name = "Changepassword";
            this.Text = "Change Password";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label labelCurrent;
        private System.Windows.Forms.Label labelNew;
        private System.Windows.Forms.Label labelVerify;
        private System.Windows.Forms.TextBox textBoxCurrent;
        private System.Windows.Forms.TextBox textBoxNew;
        private System.Windows.Forms.TextBox textBoxVerify;
        private System.Windows.Forms.Button buttonChange;
    }
}
