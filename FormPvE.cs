using System.Drawing;
using System.Media;
using System.Windows.Forms;
using System;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace DoAnMonHocNT106
{
    public partial class FormPvE : Form
    {
        private string currentUser;

        private const int Rows = 20;
        private const int Cols = 17;
        private Button[,] board = new Button[Rows, Cols];
        private bool isPlayerTurn = true;
        private bool gameOver = false;

        private string playerName = "Người chơi";
        private Timer countdownTimer;
        private int countdown = 10;

        private int[] AttackPoint = { 0, 1, 10, 100, 1000, 100000 };
        private int[] DefensePoint = { 0, 2, 20, 200, 2000, 200000 };

        public FormPvE(string user)
        {
            InitializeComponent();
            currentUser = user;
            InitializeTimer();
            InitializeBoard();
        }

        private void InitializeBoard()
        {
            panelBoard.Controls.Clear();
            board = new Button[Rows, Cols];
            int size = 30;

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
                        Tag = new Point(i, j)
                    };
                    btn.Click += PlayerMove;
                    board[i, j] = btn;
                    panelBoard.Controls.Add(btn);
                }
            }

            isPlayerTurn = true;
            gameOver = false;
            StartCountdown();
        }

        private void InitializeTimer()
        {
            countdownTimer = new Timer { Interval = 1000 };
            countdownTimer.Tick += CountdownTick;
        }

        private void StartCountdown()
        {
            countdown = 10;
            lblTimer.Text = $"Thời gian: {countdown}s";
            countdownTimer.Start();
        }

        private void StopCountdown()
        {
            countdownTimer.Stop();
        }
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
                    await FirebaseHelper.SaveGameResult(playerName, "Timeout");
                    MessageBox.Show($"{playerName} đã hết thời gian! Bạn thua.");
                    PlaySound("lose.wav");
                }
            }
        }
        private async void PlayerMove(object sender, EventArgs e)
        {
            if (gameOver || !isPlayerTurn) return;

            Button btn = sender as Button;
            if (btn.Text != "") return;

            btn.Text = "X";
            btn.ForeColor = Color.Blue;
            Point point = (Point)btn.Tag;

            StopCountdown();

            if (CheckWin(point.X, point.Y, "X"))
            {
                gameOver = true;
                HighlightWinningLine(point.X, point.Y, "X");
                PlaySound("win.wav");
                await FirebaseHelper.SaveGameResult(playerName, "Win");
                MessageBox.Show($"{playerName} thắng!");
                return;
            }

            isPlayerTurn = false;
            BotMove();
        }

        private async void BotMove()
        {
            Point move = GetSmartBotMove();
            if (board[move.X, move.Y].Text != "") return; // tránh lỗi nếu trùng

            board[move.X, move.Y].Text = "O";
            board[move.X, move.Y].ForeColor = Color.Red;

            if (CheckWin(move.X, move.Y, "O"))
            {
                gameOver = true;
                HighlightWinningLine(move.X, move.Y, "O");
                PlaySound("lose.wav");
                await FirebaseHelper.SaveGameResult(playerName, "Lose");
                MessageBox.Show("Bot thắng!");
                return;
            }

            isPlayerTurn = true;
            StartCountdown();
        }

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

        private int EvaluatePoint(int x, int y, string player)
        {
            return EvaluateDirection(x, y, 1, 0, player) +
                   EvaluateDirection(x, y, 0, 1, player) +
                   EvaluateDirection(x, y, 1, 1, player) +
                   EvaluateDirection(x, y, 1, -1, player);
        }

        private int EvaluateDirection(int x, int y, int dx, int dy, string player)
        {
            int count = 0, block = 0;

            for (int i = 1; i <= 4; i++)
            {
                int nx = x + dx * i;
                int ny = y + dy * i;
                if (nx < 0 || ny < 0 || nx >= Rows || ny >= Cols) { block++; break; }
                string cell = board[nx, ny].Text;
                if (cell == player) count++;
                else if (cell != "") { block++; break; } else break;
            }

            for (int i = 1; i <= 4; i++)
            {
                int nx = x - dx * i;
                int ny = y - dy * i;
                if (nx < 0 || ny < 0 || nx >= Rows || ny >= Cols) { block++; break; }
                string cell = board[nx, ny].Text;
                if (cell == player) count++;
                else if (cell != "") { block++; break; } else break;
            }

            return player == "O" ? (block == 2 ? 0 : AttackPoint[count]) : (block == 2 ? 0 : DefensePoint[count]);
        }

        private bool CheckWin(int x, int y, string player)
        {
            return CheckDirection(x, y, 1, 0, player) ||
                   CheckDirection(x, y, 0, 1, player) ||
                   CheckDirection(x, y, 1, 1, player) ||
                   CheckDirection(x, y, 1, -1, player);
        }

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

        private void HighlightWinningLine(int x, int y, string player)
        {
            if (CheckDirection(x, y, 1, 0, player)) HighlightWinningLine(x, y, 1, 0, player);
            else if (CheckDirection(x, y, 0, 1, player)) HighlightWinningLine(x, y, 0, 1, player);
            else if (CheckDirection(x, y, 1, 1, player)) HighlightWinningLine(x, y, 1, 1, player);
            else if (CheckDirection(x, y, 1, -1, player)) HighlightWinningLine(x, y, 1, -1, player);
        }

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

            if (count < 5)
            {
                for (int i = 0; i < Rows; i++)
                    for (int j = 0; j < Cols; j++)
                        if (board[i, j].BackColor == Color.Orange)
                            board[i, j].BackColor = SystemColors.Control;
            }
        }

        private void PlaySound(string fileName)
        {
            try
            {
                SoundPlayer player = new SoundPlayer(fileName);
                player.Play();
            }
            catch { }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            MusicPlayer.PlayClickSound();
            gameOver = true;
            StopCountdown();
            new Form1().Show();
            this.Close(); 
        }
        private void btnRestart_Click(object sender, EventArgs e)
        {
            MusicPlayer.PlayClickSound();
            InitializeBoard();
        }
        private void FormPvE_FormClosing(object sender, FormClosingEventArgs e)
        {
            gameOver = true;
            StopCountdown();
        }
    }
}
