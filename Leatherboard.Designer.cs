using System;
using System.Drawing;
using System.Windows.Forms;

namespace DoAnMonHocNT106
{
    public partial class Leatherboard : Form
    {
        private PictureBox pictureBoxMedals;
        private ListView listViewLeaderboard;

        private void Leatherboard_Load(object sender, EventArgs e)
        {
            LoadLeaderboardData();
        }

        private void LoadLeaderboardData()
        {
            string[,] data = {
                { "1", "Under Primeea", "Ciorra Claix", "1" },
                { "2", "Enart Sitlone", "Sorrie Sates", "1" },
                { "3", "Benve Flieble", "Brann Broahard", "1" },
                { "4", "Motpr Homritaton", "Eronkte", "1" },
                { "5", "Boroh Qandle", "MaslWileais", "1" },
                { "6", "Phyvie Vaokettie", "Nirchena", "1" },
                { "7", "Vlire Seera", "Moolleils", "1" },
                { "8", "Torlay Morke", "Siay tatie", "1" },
                { "9", "Mathy L.Srrart", "Beomcke", "1" },
                { "10", "Bonne Souten", "Claperie Gallas", "1" },
            };

            for (int i = 0; i < data.GetLength(0); i++)
            {
                ListViewItem item = new ListViewItem(data[i, 0]);
                for (int j = 1; j < data.GetLength(1); j++)
                {
                    item.SubItems.Add(data[i, j]);
                }
                listViewLeaderboard.Items.Add(item);
            }
        }

        private void InitializeComponent()
        {
            this.pictureBoxMedals = new PictureBox();
            this.listViewLeaderboard = new ListView();

            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMedals)).BeginInit();
            this.SuspendLayout();

            // 
            // pictureBoxMedals
            // 
            // this.pictureBoxMedals.Image = Image.FromFile("medals.png"); // hoặc đường dẫn tuyệt đối nếu cần
            this.pictureBoxMedals.Image = null;
            this.pictureBoxMedals.Location = new Point(100, 10);
            this.pictureBoxMedals.Size = new Size(600, 120);
            this.pictureBoxMedals.SizeMode = PictureBoxSizeMode.StretchImage;

            // 
            // listViewLeaderboard
            // 
            this.listViewLeaderboard.Location = new Point(50, 140);
            this.listViewLeaderboard.Size = new Size(700, 260);
            this.listViewLeaderboard.View = View.Details;
            this.listViewLeaderboard.FullRowSelect = true;
            this.listViewLeaderboard.GridLines = true;

            this.listViewLeaderboard.Columns.Add("STT", 50);
            this.listViewLeaderboard.Columns.Add("Người chơi 1", 200);
            this.listViewLeaderboard.Columns.Add("Người chơi 2", 200);
            this.listViewLeaderboard.Columns.Add("Kết quả", 100);

            // 
            // Leatherboard (Form)
            // 
            this.ClientSize = new Size(800, 450);
            this.Controls.Add(this.pictureBoxMedals);
            this.Controls.Add(this.listViewLeaderboard);
            this.Name = "Leatherboard";
            this.Text = "Leatherboard";

            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMedals)).EndInit();
            this.ResumeLayout(false);
        }
    }
}
