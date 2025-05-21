using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DoAnMonHocNT106
{
    public partial class Menu : Form
    {
        public Menu()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 Form = new Form1();
            Form.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            PvE Form = new PvE();
            Form.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Login Form = new Login();
            Form.Show();
            this.Hide();
        }
    }
}
