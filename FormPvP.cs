// FormPvP.cs
// Xử lý logic và giao diện cho chế độ chơi PvP (người với người):
// Quản lý bàn cờ, nước đi của người chơi, đồng bộ hóa với Firebase,
// kiểm tra thắng/thua/hòa, lắng nghe trạng thái đối thủ, và hỗ trợ chơi lại hoặc quay về Lobby.

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
        // Thông tin người chơi hiện tại, đối thủ và ID phòng chơi
        private string currentUser, opponentUser, roomId;
        // Kết nối tới Firebase Realtime Database
        private FirebaseClient firebase;
        // Mảng lưu bàn cờ
        private Button[,] board;
        // Kích thước bàn cờ: 20 hàng x 17 cột
        private const int Rows = 20, Cols = 17;
        // Ký hiệu của người chơi hiện tại và đối thủ
        private string mySymbol, opponentSymbol;
        // Trạng thái lượt chơi
        private bool isMyTurn = false;
        // Trạng thái kết thúc trò chơi
        private bool gameOver = false;
        // Lưu trữ các key nước đi đã xử lý để tránh lặp
        private HashSet<string> processedKeys = new HashSet<string>();
        // Bộ đếm ngược thời gian
        private Timer countdownTimer;
        // Thời gian mặc định cho mỗi lượt
        private int countdown = 20;
        // Cờ kiểm tra khi quay lại Lobby
        private bool isReturningToLobby = false;

        // Khởi tạo form với thông tin người chơi và phòng
        public FormPvP(string currentUser, string opponentUser, string roomId)
        {
            InitializeComponent();
            this.currentUser = currentUser;
            this.opponentUser = opponentUser;
            this.roomId = roomId;

            try
            {
                // Kết nối tới Firebase
                this.firebase = new FirebaseClient("https://nt106-7c9fe-default-rtdb.firebaseio.com/");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi kết nối Firebase: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            InitializeBoard();
            this.FormClosing += FormPvP_FormClosing;
        }

        // Khởi tạo bàn cờ với các ô là Button 
        private void InitializeBoard()
        {
            panelBoard.Controls.Clear();
            board = new Button[Rows, Cols];
            int size = 30; // Kích thước mỗi ô

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
                        Tag = new Point(i, j) // Lưu tọa độ ô
                    };
                    btn.Click += PlayerMove; // Gán sự kiện click cho nước đi
                    board[i, j] = btn;
                    panelBoard.Controls.Add(btn);
                }
            }
        }

        // Xử lý khi form được tải
        private async void FormPvP_Load(object sender, EventArgs e)
        {
            // Cập nhật trạng thái online
            await FirebaseHelper.SetUserOnlineStatus(currentUser, true);

            // Phân định ký hiệu và lượt đi
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

            // Cập nhật nhãn thông tin người chơi
            lblYou.Text = $"Bạn ({currentUser}): {mySymbol}";
            lblOpponent.Text = $"Đối thủ ({opponentUser}): {opponentSymbol}";

            // Khởi tạo bộ đếm ngược
            InitializeTimer();
            if (isMyTurn) StartCountdown();

            // Đợi 1 giây trước khi khởi tạo trò chơi
            await Task.Delay(1000);
            _ = InitializeGameAsync();
        }

        // Khởi tạo trò chơi bất đồng bộ
        private async Task InitializeGameAsync()
        {
            // Lắng nghe nước đi và trạng thái đối thủ
            ListenToMoves();
            ListenToOpponentStatus();
            await Task.CompletedTask;
        }

        // Thiết lập bộ đếm ngược
        private void InitializeTimer()
        {
            countdownTimer = new Timer();
            countdownTimer.Interval = 1000; // Cập nhật mỗi giây
            countdownTimer.Tick += CountdownTick;
        }

        // Bắt đầu đếm ngược cho lượt chơi
        private void StartCountdown()
        {
            countdown = 20;
            lblCountdown.Text = $"Thời gian: {countdown}s";
            countdownTimer.Start();
        }

        // Dừng bộ đếm ngược
        private void StopCountdown()
        {
            countdownTimer.Stop();
            lblCountdown.Text = "";
        }

        // Xử lý mỗi giây đếm ngược
        private async void CountdownTick(object sender, EventArgs e)
        {
            if (gameOver) return;
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

        // Xử lý nước đi của người chơi
        private async void PlayerMove(object sender, EventArgs e)
        {
            if (gameOver || !isMyTurn) return;

            Button btn = sender as Button;
            if (btn == null || btn.Text != "") return;

            Point point = (Point)btn.Tag;

            try
            {
                // Hiển thị nước đi trên giao diện
                btn.Text = mySymbol;
                btn.ForeColor = mySymbol == "X" ? Color.Blue : Color.Red;

                StopCountdown();

                // Lưu nước đi vào Firebase
                var move = new Move
                {
                    row = point.X,
                    col = point.Y,
                    user = currentUser,
                    symbol = mySymbol,
                    timestamp = DateTime.UtcNow.ToString("o")
                };

                await firebase.Child("Rooms").Child(roomId).Child("Moves").PostAsync(move);

                // Kiểm tra thắng
                if (CheckWin(point.X, point.Y, mySymbol))
                {
                    gameOver = true;
                    HighlightWinningLine(point.X, point.Y, mySymbol);
                    await FirebaseHelper.SavePvPGameResult(roomId, currentUser, opponentUser, "Win");
                    MessageBox.Show("Bạn thắng!");
                }
                // Kiểm tra hòa
                else if (CheckDraw())
                {
                    gameOver = true;
                    await FirebaseHelper.SavePvPGameResult(roomId, currentUser, opponentUser, "Draw");
                    MessageBox.Show("Trò chơi hòa!");
                }
                else
                {
                    isMyTurn = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thực hiện nước đi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Lắng nghe nước đi từ Firebase
        private void ListenToMoves()
        {
            firebase.Child("Rooms").Child(roomId).Child("Moves")
                .AsObservable<Move>()
                .Subscribe(ev =>
                {
                    if (gameOver || ev.Object == null || processedKeys.Contains(ev.Key)) return;

                    processedKeys.Add(ev.Key);
                    var move = ev.Object;

                    this.Invoke(new MethodInvoker(() =>
                    {
                        // Cập nhật nước đi lên bàn cờ
                        Button btn = board[move.row, move.col];
                        btn.Text = move.symbol;
                        btn.ForeColor = move.symbol == "X" ? Color.Blue : Color.Red;

                        // Chuyển lượt nếu là nước đi của đối thủ
                        if (move.user != currentUser)
                        {
                            isMyTurn = true;
                            StartCountdown();
                        }

                        // Kiểm tra thắng
                        if (CheckWin(move.row, move.col, move.symbol))
                        {
                            gameOver = true;
                            StopCountdown();
                            HighlightWinningLine(move.row, move.col, move.symbol);
                            string result = move.user == currentUser ? "Win" : "Lose";
                            FirebaseHelper.SavePvPGameResult(roomId, currentUser, opponentUser, result);
                            MessageBox.Show($"{move.user} thắng!");
                        }
                        // Kiểm tra hòa
                        else if (CheckDraw())
                        {
                            gameOver = true;
                            StopCountdown();
                            FirebaseHelper.SavePvPGameResult(roomId, currentUser, opponentUser, "Draw");
                            MessageBox.Show("Trò chơi hòa!");
                        }
                    }));
                });
        }

        // Lắng nghe trạng thái online của đối thủ
        private void ListenToOpponentStatus()
        {
            firebase.Child("Users").Child(opponentUser).Child("online")
                .AsObservable<bool>()
                .Subscribe(status =>
                {
                    if (status.Object == null) return;

                    if (!status.Object && !gameOver)
                    {
                        this.Invoke(new MethodInvoker(() =>
                        {
                            gameOver = true;
                            StopCountdown();
                            MessageBox.Show("Đối thủ đã thoát khỏi trò chơi!", "Thông báo");
                            this.Close();
                        }));
                    }
                });
        }

        // Kiểm tra điều kiện thắng trên 4 hướng
        private bool CheckWin(int x, int y, string symbol)
        {
            return CheckDirection(x, y, 1, 0, symbol) || // Hàng ngang
                   CheckDirection(x, y, 0, 1, symbol) || // Cột dọc
                   CheckDirection(x, y, 1, 1, symbol) || // Đường chéo chính
                   CheckDirection(x, y, 1, -1, symbol); // Đường chéo phụ
        }

        // Kiểm tra bàn cờ đã đầy (hòa)
        private bool CheckDraw()
        {
            for (int i = 0; i < Rows; i++)
                for (int j = 0; j < Cols; j++)
                    if (board[i, j].Text == "") return false;
            return true;
        }

        // Kiểm tra có đủ 5 ô liên tiếp trên một hướng
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

        // Tô sáng đường thắng
        private void HighlightWinningLine(int x, int y, string symbol)
        {
            if (CheckDirection(x, y, 1, 0, symbol)) HighlightDirection(x, y, 1, 0, symbol);
            else if (CheckDirection(x, y, 0, 1, symbol)) HighlightDirection(x, y, 0, 1, symbol);
            else if (CheckDirection(x, y, 1, 1, symbol)) HighlightDirection(x, y, 1, 1, symbol);
            else if (CheckDirection(x, y, 1, -1, symbol)) HighlightDirection(x, y, 1, -1, symbol);
        }

        // Xử lý sự kiện vẽ lại panel bàn cờ (hiện không sử dụng)
        private void panelBoard_Paint(object sender, PaintEventArgs e)
        {
        }

        // Tô màu cam cho các ô thuộc đường thắng
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

        // Xử lý nút chơi lại
        private async void btnRestart_Click(object sender, EventArgs e)
        {
            MusicPlayer.PlayClickSound();
            var confirm = MessageBox.Show("Bạn có chắc muốn chơi lại?", "Chơi lại", MessageBoxButtons.YesNo);
            if (confirm == DialogResult.Yes)
            {
                // Xóa nước đi cũ và khởi tạo lại bàn cờ
                await firebase.Child("Rooms").Child(roomId).Child("Moves").DeleteAsync();
                gameOver = false;
                InitializeBoard();
                isMyTurn = mySymbol == "X";
                if (isMyTurn) StartCountdown();
                else StopCountdown();
            }
        }

        // Xử lý nút quay về Lobby
        private void btnBack_Click(object sender, EventArgs e)
        {
            MusicPlayer.PlayClickSound();
            gameOver = true;
            isReturningToLobby = true;
            StopCountdown();
            this.Close();
        }

        // Xử lý sự kiện khi đóng form
        private async void FormPvP_FormClosing(object sender, FormClosingEventArgs e)
        {
            gameOver = true;
            StopCountdown();
            if (!isReturningToLobby)
            {
                // Cập nhật trạng thái offline nếu không quay về Lobby
                await FirebaseHelper.SetUserOnlineStatus(currentUser, false); // đã xem
            }
        }
    }
}