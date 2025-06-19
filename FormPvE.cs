// FormPvE.cs
// Xử lý logic cho form chơi với máy (PvE):
// Bao gồm các chức năng khởi tạo bàn cờ, xử lý lượt chơi của người và bot,
// đếm ngược thời gian, chấm điểm AI, kiểm tra kết quả, lưu kết quả lên Firebase,
// và xử lý sự kiện đóng form cùng điều hướng về Lobby.

using System.Drawing;
using System.Media;
using System.Windows.Forms;
using System;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.IO;

namespace DoAnMonHocNT106
{
    public partial class FormPvE : Form
    {
        // Biến lưu tên người dùng hiện tại
        private string currentUser;

        // Kích thước bàn cờ: 20 hàng x 17 cột
        private const int Rows = 20;
        private const int Cols = 17;
        private Button[,] board = new Button[Rows, Cols];

        // Cờ hiệu lượt chơi của người chơi và trạng thái kết thúc game
        private bool isPlayerTurn = true;
        private bool gameOver = false;

        // Tên hiển thị mặc định cho người chơi
        private string playerName = "Người chơi";

        // Timer đếm ngược
        private Timer countdownTimer;
        private int countdown = 10;

        // Điểm tấn công và phòng thủ dùng cho AI
        private int[] AttackPoint = { 0, 1, 10, 100, 1000, 100000 };
        private int[] DefensePoint = { 0, 2, 20, 200, 2000, 200000 };

        // Biến phụ để xác định có phải đóng form bằng nút không
        private bool exitByButton = false;

        public FormPvE(string user)
        {
            InitializeComponent();
            currentUser = user;                     // Gán người dùng hiện tại
            InitializeTimer();                     // Khởi tạo timer
            InitializeBoard();                     // Khởi tạo bàn cờ
            this.FormClosing += FormPvE_FormClosing;  // Bắt sự kiện đóng form
        }

        // Khởi tạo bàn cờ với các nút Button
        private void InitializeBoard()
        {
            panelBoard.Controls.Clear();
            board = new Button[Rows, Cols];
            int size = 30;  // Kích thước mỗi ô

            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    Button btn = new Button
                    {
                        Width = size,
                        Height = size,
                        Location = new Point(j * size, i * size),
                        Font = new Font(FontFamily.GenericSansSerif, 12, FontStyle.Bold),
                        Tag = new Point(i, j)    // Dùng Tag để chứa tọa độ
                    };
                    btn.Click += PlayerMove;      // Bắt sự kiện click của người chơi
                    board[i, j] = btn;
                    panelBoard.Controls.Add(btn);
                }
            }

            isPlayerTurn = true;
            gameOver = false;
            StartCountdown();   // Bắt đầu đếm ngược cho lượt đầu
        }

        // Thiết lập timer cho đếm ngược
        private void InitializeTimer()
        {
            countdownTimer = new Timer { Interval = 1000 };  // 1 giây
            countdownTimer.Tick += CountdownTick;
        }

        // Bắt đầu lại thời gian đếm ngược
        private void StartCountdown()
        {
            countdown = 10;
            lblTimer.Text = $"Thời gian: {countdown}s";
            countdownTimer.Start();
        }

        // Dừng timer
        private void StopCountdown()
        {
            countdownTimer.Stop();
        }

        // Xử lý mỗi tick của timer
        private async void CountdownTick(object sender, EventArgs e)
        {
            if (gameOver) return;

            countdown--;
            lblTimer.Text = $"Thời gian: {countdown}s";

            if (countdown <= 0)
            {
                StopCountdown();
                if (!gameOver)
                {
                    gameOver = true;
                    await FirebaseHelper.SaveGameResult(playerName, "Timeout");  // Lưu kết quả
                    MessageBox.Show($"{playerName} đã hết thời gian! Bạn thua.");
                    PlaySound("lose.wav");  // Phát nhạc thua
                }
            }
        }

        // Xử lý nước đi của người chơi
        private async void PlayerMove(object sender, EventArgs e)
        {
            if (gameOver || !isPlayerTurn) return;

            Button btn = sender as Button;
            if (btn.Text != "") return;  // Nếu đã có ký tự thì không đặt tiếp

            btn.Text = "X";
            btn.ForeColor = Color.Blue;
            Point point = (Point)btn.Tag;

            StopCountdown();  // Dừng đếm ngược khi đã đi

            // Kiểm tra thắng
            if (CheckWin(point.X, point.Y, "X"))
            {
                gameOver = true;
                HighlightWinningLine(point.X, point.Y, "X");
                PlaySound("win.wav");
                await FirebaseHelper.SaveGameResult(playerName, "Win");
                MessageBox.Show($"{playerName} thắng!");
                return;
            }
            // Kiểm tra hòa
            if (IsBoardFull())
            {
                gameOver = true;
                await FirebaseHelper.SaveGameResult(playerName, "Draw");
                MessageBox.Show("Hòa! Bàn cờ đã đầy.");
                return;
            }

            isPlayerTurn = false;
            BotMove();  // Đến lượt bot
        }

        // Nước đi của bot
        private async void BotMove()
        {
            Point move = GetSmartBotMove();
            if (board[move.X, move.Y].Text != "") return; // tránh lỗi nếu đã có

            board[move.X, move.Y].Text = "O";
            board[move.X, move.Y].ForeColor = Color.Red;

            // Kiểm tra bot thắng
            if (CheckWin(move.X, move.Y, "O"))
            {
                gameOver = true;
                HighlightWinningLine(move.X, move.Y, "O");
                PlaySound("lose.wav");
                await FirebaseHelper.SaveGameResult(playerName, "Lose");
                MessageBox.Show("Bot thắng!");
                return;
            }
            // Kiểm tra hòa
            if (IsBoardFull())
            {
                gameOver = true;
                await FirebaseHelper.SaveGameResult(playerName, "Draw");
                MessageBox.Show("Hòa! Bàn cờ đã đầy.");
                return;
            }

            isPlayerTurn = true;
            StartCountdown();  // Bật lại đếm ngược cho người chơi
        }

        // Tìm nước đi tốt nhất cho bot bằng thuật toán chấm điểm
        private Point GetSmartBotMove()
        {
            int maxScore = -1;
            Point bestMove = new Point(0, 0);

            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    if (board[i, j].Text != "") continue;

                    int attack = EvaluatePoint(i, j, "O");
                    int defense = EvaluatePoint(i, j, "X");
                    int total = attack + defense;

                    if (total > maxScore)
                    {
                        maxScore = total;
                        bestMove = new Point(i, j);
                    }
                }
            }

            return bestMove;
        }

        // Tính điểm cho một ô theo cả tấn công và phòng thủ
        private int EvaluatePoint(int x, int y, string player)
        {
            return EvaluateDirection(x, y, 1, 0, player) +
                   EvaluateDirection(x, y, 0, 1, player) +
                   EvaluateDirection(x, y, 1, 1, player) +
                   EvaluateDirection(x, y, 1, -1, player);
        }

        // Tính điểm trên một hướng nhất định
        private int EvaluateDirection(int x, int y, int dx, int dy, string player)
        {
            int count = 0, block = 0;

            // Duyệt về phía trước
            for (int i = 1; i <= 4; i++)
            {
                int nx = x + dx * i;
                int ny = y + dy * i;
                if (nx < 0 || ny < 0 || nx >= Rows || ny >= Cols) { block++; break; }
                string cell = board[nx, ny].Text;
                if (cell == player) count++;
                else if (cell != "") { block++; break; } else break;
            }

            // Duyệt về phía sau
            for (int i = 1; i <= 4; i++)
            {
                int nx = x - dx * i;
                int ny = y - dy * i;
                if (nx < 0 || ny < 0 || nx >= Rows || ny >= Cols) { block++; break; }
                string cell = board[nx, ny].Text;
                if (cell == player) count++;
                else if (cell != "") { block++; break; } else break;
            }

            // Trả về điểm theo loại người chơi
            return player == "O"
                ? (block == 2 ? 0 : AttackPoint[count])
                : (block == 2 ? 0 : DefensePoint[count]);
        }

        // Kiểm tra thắng theo 4 hướng
        private bool CheckWin(int x, int y, string player)
        {
            return CheckDirection(x, y, 1, 0, player) ||
                   CheckDirection(x, y, 0, 1, player) ||
                   CheckDirection(x, y, 1, 1, player) ||
                   CheckDirection(x, y, 1, -1, player);
        }

        // Kiểm tra bàn cờ đã đầy
        private bool IsBoardFull()
        {
            for (int i = 0; i < Rows; i++)
                for (int j = 0; j < Cols; j++)
                    if (string.IsNullOrEmpty(board[i, j].Text))
                        return false;
            return true;
        }

        // Kiểm tra có đủ 5 ô liên tiếp theo hướng dx, dy
        private bool CheckDirection(int x, int y, int dx, int dy, string player)
        {
            int count = 1;
            for (int i = 1; i < 5; i++)
            {
                int nx = x + dx * i, ny = y + dy * i;
                if (nx < 0 || ny < 0 || nx >= Rows || ny >= Cols || board[nx, ny].Text != player) break;
                count++;
            }
            for (int i = 1; i < 5; i++)
            {
                int nx = x - dx * i, ny = y - dy * i;
                if (nx < 0 || ny < 0 || nx >= Rows || ny >= Cols || board[nx, ny].Text != player) break;
                count++;
            }
            return count >= 5;
        }

        // Đánh dấu đường thắng và tô màu cam cho các ô
        private void HighlightWinningLine(int x, int y, string player)
        {
            if (CheckDirection(x, y, 1, 0, player)) HighlightWinningLine(x, y, 1, 0, player);
            else if (CheckDirection(x, y, 0, 1, player)) HighlightWinningLine(x, y, 0, 1, player);
            else if (CheckDirection(x, y, 1, 1, player)) HighlightWinningLine(x, y, 1, 1, player);
            else if (CheckDirection(x, y, 1, -1, player)) HighlightWinningLine(x, y, 1, -1, player);
        }

        // Phương thức phụ để tô màu cho đường thắng
        private void HighlightWinningLine(int x, int y, int dx, int dy, string player)
        {
            int count = 1;
            board[x, y].BackColor = Color.Orange;

            for (int i = 1; i < 5; i++)
            {
                int nx = x + dx * i, ny = y + dy * i;
                if (nx < 0 || ny < 0 || nx >= Rows || ny >= Cols || board[nx, ny].Text != player) break;
                board[nx, ny].BackColor = Color.Orange;
                count++;
            }

            for (int i = 1; i < 5; i++)
            {
                int nx = x - dx * i, ny = y - dy * i;
                if (nx < 0 || ny < 0 || nx >= Rows || ny >= Cols || board[nx, ny].Text != player) break;
                board[nx, ny].BackColor = Color.Orange;
                count++;
            }

            // Nếu chưa đủ 5 ô thì bỏ tô
            if (count < 5)
            {
                for (int i = 0; i < Rows; i++)
                    for (int j = 0; j < Cols; j++)
                        if (board[i, j].BackColor == Color.Orange)
                            board[i, j].BackColor = SystemColors.Control;
            }
        }

        // Phát âm thanh từ thư mục Resources/Music
        private void PlaySound(string fileName)
        {
            try
            {
                string soundDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Music");
                string soundFile = Path.Combine(soundDir, fileName);
                SoundPlayer player = new SoundPlayer(soundFile);
                player.Play();
            }
            catch { }
        }

        // Xử lý sự kiện khi bấm nút Quay về Lobby
        private void button1_Click(object sender, EventArgs e)
        {
            MusicPlayer.PlayClickSound();
            gameOver = true;
            StopCountdown();

            // đánh dấu đây là exit qua nút
            exitByButton = true;

            // chỉ mở Form1 ở đây
            new Form1().Show();
            this.Close();
        }

        // Xử lý sự kiện bấm nút Restart để chơi lại
        private void btnRestart_Click(object sender, EventArgs e)
        {
            MusicPlayer.PlayClickSound();
            InitializeBoard();  // Khởi tạo lại bàn cờ
        }

        // Xử lý trước khi form đóng
        private async void FormPvE_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Nếu đây là do bấm X (UserClosing) và chưa bấm nút button1
            if (e.CloseReason == CloseReason.UserClosing && !exitByButton)
            {
                e.Cancel = true;            // Hủy đóng form tạm
                btnBack.PerformClick();    // Giả lập bấm nút Thoát
            }
            // Ngược lại, để form đóng bình thường

            await FirebaseHelper.SetUserOnlineStatus(currentUser, false);  // Cập nhật trạng thái offline
        }
    }
}
