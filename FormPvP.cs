using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DoAnMonHocNT106
{
    public partial class FormPvP : Form
    {
        private string currentUser, opponentUser, roomId;
        private FirebaseClient firebase;
        private Button[,] board;
        private const int Rows = 20, Cols = 17;
        private string mySymbol, opponentSymbol;
        private bool isMyTurn = false;
        private bool gameOver = false;
        private HashSet<string> processedKeys = new HashSet<string>();

        private Timer countdownTimer;
        private int countdown = 20;

        public FormPvP(string currentUser, string opponentUser, string roomId)
        {
            InitializeComponent();
            this.currentUser = currentUser;
            this.opponentUser = opponentUser;
            this.roomId = roomId;
            firebase = FirebaseHelper.GetFirebaseClient();
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
        }

        private void FormPvP_Load(object sender, EventArgs e)
        {
            if (string.Compare(currentUser, opponentUser) < 0)
            {
                mySymbol = "X";
                opponentSymbol = "O";
                isMyTurn = true;
            }
            else
            {
                mySymbol = "O";
                opponentSymbol = "X";
                isMyTurn = false;
            }

            lblYou.Text = $"Bạn ({currentUser}): {mySymbol}";
            lblOpponent.Text = $"Đối thủ ({opponentUser}): {opponentSymbol}";

            InitializeTimer();
            if (isMyTurn) StartCountdown();

            ListenToMoves();
        }

        private void InitializeTimer()
        {
            countdownTimer = new Timer();
            countdownTimer.Interval = 1000;
            countdownTimer.Tick += CountdownTick;
        }

        private void StartCountdown()
        {
            countdown = 20;
            lblCountdown.Text = $"Thời gian: {countdown}s";
            countdownTimer.Start();
        }

        private void StopCountdown()
        {
            countdownTimer.Stop();
            lblCountdown.Text = "";
        }

        private async void CountdownTick(object sender, EventArgs e)
        {
            countdown--;
            lblCountdown.Text = $"Thời gian: {countdown}s";
            if (countdown <= 0)
            {
                StopCountdown();
                gameOver = true;
                await FirebaseHelper.SavePvPGameResult(roomId, currentUser, opponentUser, "Timeout");
                MessageBox.Show("Bạn đã hết thời gian! Thua cuộc.");
                this.Close();
            }
        }

        private void ListenToMoves()
        {
            firebase.Child("Rooms").Child(roomId).Child("Moves")
                .AsObservable<Move>()
                .Subscribe(ev =>
                {
                    if (ev.Object != null && !processedKeys.Contains(ev.Key))
                    {
                        processedKeys.Add(ev.Key);
                        var move = ev.Object;

                        this.Invoke(new MethodInvoker(() =>
                        {
                            Button btn = board[move.row, move.col];
                            btn.Text = move.symbol;
                            btn.ForeColor = move.symbol == "X" ? Color.Blue : Color.Red;

                            if (move.user != currentUser)
                            {
                                isMyTurn = true;
                                StartCountdown();
                            }

                            if (CheckWin(move.row, move.col, move.symbol))
                            {
                                gameOver = true;
                                StopCountdown();
                                HighlightWinningLine(move.row, move.col, move.symbol);
                                string result = move.user == currentUser ? "Win" : "Lose";
                                FirebaseHelper.SavePvPGameResult(roomId, currentUser, opponentUser, result);
                                MessageBox.Show($"{move.user} thắng!");
                            }
                        }));
                    }
                });
        }

        private async void PlayerMove(object sender, EventArgs e)
        {
            if (gameOver || !isMyTurn) return;

            Button btn = sender as Button;
            if (btn == null || btn.Text != "") return;

            Point point = (Point)btn.Tag;

            btn.Text = mySymbol;
            btn.ForeColor = mySymbol == "X" ? Color.Blue : Color.Red;

            StopCountdown();

            var move = new Move
            {
                row = point.X,
                col = point.Y,
                user = currentUser,
                symbol = mySymbol,
                timestamp = DateTime.UtcNow.ToString("o")
            };

            await firebase.Child("Rooms").Child(roomId).Child("Moves").PostAsync(move);

            if (CheckWin(point.X, point.Y, mySymbol))
            {
                gameOver = true;
                HighlightWinningLine(point.X, point.Y, mySymbol);
                await FirebaseHelper.SavePvPGameResult(roomId, currentUser, opponentUser, "Win");
                MessageBox.Show("Bạn thắng!");
            }
            else
            {
                isMyTurn = false;
            }
        }

        private bool CheckWin(int x, int y, string symbol)
        {
            return CheckDirection(x, y, 1, 0, symbol) ||
                   CheckDirection(x, y, 0, 1, symbol) ||
                   CheckDirection(x, y, 1, 1, symbol) ||
                   CheckDirection(x, y, 1, -1, symbol);
        }

        private bool CheckDirection(int x, int y, int dx, int dy, string symbol)
        {
            int count = 1;
            for (int i = 1; i < 5; i++)
            {
                int nx = x + dx * i, ny = y + dy * i;
                if (nx < 0 || ny < 0 || nx >= Rows || ny >= Cols || board[nx, ny].Text != symbol) break;
                count++;
            }
            for (int i = 1; i < 5; i++)
            {
                int nx = x - dx * i, ny = y - dy * i;
                if (nx < 0 || ny < 0 || nx >= Rows || ny >= Cols || board[nx, ny].Text != symbol) break;
                count++;
            }
            return count >= 5;
        }

        private void HighlightWinningLine(int x, int y, string symbol)
        {
            if (CheckDirection(x, y, 1, 0, symbol)) HighlightDirection(x, y, 1, 0, symbol);
            else if (CheckDirection(x, y, 0, 1, symbol)) HighlightDirection(x, y, 0, 1, symbol);
            else if (CheckDirection(x, y, 1, 1, symbol)) HighlightDirection(x, y, 1, 1, symbol);
            else if (CheckDirection(x, y, 1, -1, symbol)) HighlightDirection(x, y, 1, -1, symbol);
        }

        private void HighlightDirection(int x, int y, int dx, int dy, string symbol)
        {
            board[x, y].BackColor = Color.Orange;

            for (int i = 1; i < 5; i++)
            {
                int nx = x + dx * i, ny = y + dy * i;
                if (nx < 0 || ny < 0 || nx >= Rows || ny >= Cols || board[nx, ny].Text != symbol) break;
                board[nx, ny].BackColor = Color.Orange;
            }

            for (int i = 1; i < 5; i++)
            {
                int nx = x - dx * i, ny = y - dy * i;
                if (nx < 0 || ny < 0 || nx >= Rows || ny >= Cols || board[nx, ny].Text != symbol) break;
                board[nx, ny].BackColor = Color.Orange;
            }
        }

        private async void btnRestart_Click(object sender, EventArgs e)
        {
            var confirm = MessageBox.Show("Bạn có chắc muốn chơi lại?", "Chơi lại", MessageBoxButtons.YesNo);
            if (confirm == DialogResult.Yes)
            {
                await firebase.Child("Rooms").Child(roomId).Child("Moves").DeleteAsync();
                gameOver = false;
                InitializeBoard();
                processedKeys.Clear();
                isMyTurn = mySymbol == "X";
                if (isMyTurn) StartCountdown(); else StopCountdown();
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
