using System;
using System.Windows.Forms;

namespace DoAnMonHocNT106
{
    public partial class Changepassword : Form
    {
        public Changepassword()
        {
            InitializeComponent();
        }

        private void buttonChange_Click(object sender, EventArgs e)
        {
            string current = textBoxCurrent.Text;
            string newPass = textBoxNew.Text;
            string verify = textBoxVerify.Text;

            // Giả lập kiểm tra mật khẩu cũ (ví dụ hardcoded là "admin")
            if (current != "admin")
            {
                MessageBox.Show("Current password is incorrect!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(newPass) || string.IsNullOrWhiteSpace(verify))
            {
                MessageBox.Show("Please enter all password fields!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (newPass != verify)
            {
                MessageBox.Show("New password and verify password do not match!", "Mismatch", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Thay đổi mật khẩu thành công (có thể thêm lưu vào file hoặc DB sau)
            MessageBox.Show("Password changed successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }
    }
}
