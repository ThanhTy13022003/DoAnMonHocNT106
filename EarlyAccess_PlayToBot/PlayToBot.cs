using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Media;
using System.IO;

namespace GameCaro_PlayToBot
{
    public partial class PlayToBot : Form
    {
        private const int BoardSize = 20;
        private const int CellSize = 30;
        private Button[,] board;
        private bool isPlayerTurn = true;
        private bool gameEnded = false;
        private Timer timer;
        private int timeLimit = 10; // Default time limit
        private int timeLeft = 10;
        private string difficulty = "Trung bình"; // Default difficulty
        private Stack<(Point pos, string player, bool wasPlayerTurn)> moveHistory = new Stack<(Point, string, bool)>();
        private System.Timers.Timer processCheckTimer;
        private string username = "Guest"; // Default username
        private bool isPaused = false;


        public PlayToBot()
        {
            InitializeComponent();

            // Retrieve username from command-line arguments
            string[] args = Environment.GetCommandLineArgs();
            if (args.Length > 1)
            {
                username = args[1]; // Get username from command-line argument
            }
            lblUsername.Text = $"Hi {username}!"; // Update the label with username

            InitializeGame();
            processCheckTimer = new System.Timers.Timer(1000); // Kiểm tra mỗi 1 giây
            processCheckTimer.Elapsed += ProcessCheckTimer_Elapsed;
            processCheckTimer.AutoReset = true;
            processCheckTimer.Start();
        }

        private void ProcessCheckTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                var processes = System.Diagnostics.Process.GetProcessesByName("GameCaro_Menu");
                System.Diagnostics.Debug.WriteLine($"Found {processes.Length} process(es) named 'GameCaro_Menu'");
                if (processes.Length == 0)
                {
                    System.Diagnostics.Debug.WriteLine("No GameCaro_Menu process found. Stopping timer and closing form.");
                    processCheckTimer.Stop();
                    this.Invoke((MethodInvoker)delegate
                    {
                        this.Close();
                    });
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("GameCaro_Menu process is running.");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error checking process: {ex.Message}");
            }
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            try
            {
                SoundPlayer player = new SoundPlayer("click-button-140881.wav"); // hoặc @"Resources\click.wav" nếu trong thư mục con
                player.Play(); // hoặc .PlaySync() nếu muốn đợi phát xong mới tiếp tục
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi phát âm thanh: " + ex.Message);
            }

            if (!gameEnded)
            {
                isPaused = !isPaused;
                btnPause.Text = isPaused ? "Resume" : "Pause";
                if (isPaused)
                {
                    timer.Stop();
                    foreach (Button btn in board)
                    {
                        btn.Enabled = false;
                    }
                    cbDifficulty.Enabled = true; // Hiển thị ComboBox độ khó
                    cbTimeLimit.Enabled = true; // Hiển thị ComboBox thời gian
                }
                else
                {
                    timer.Start();
                    foreach (Button btn in board)
                    {
                        if (btn.Text == "")
                            btn.Enabled = true;
                    }
                    cbDifficulty.Enabled = false; // Ẩn ComboBox độ khó
                    cbTimeLimit.Enabled = false; // Ẩn ComboBox thời gian
                }
                UpdateStatus();
            }
        }

        private void InitializeGame()
        {
            board = new Button[BoardSize, BoardSize];
            for (int i = 0; i < BoardSize; i++)
            {
                for (int j = 0; j < BoardSize; j++)
                {
                    Button btn = new Button
                    {
                        Size = new Size(CellSize, CellSize),
                        Location = new Point(i * CellSize, j * CellSize),
                        Tag = new Point(i, j),
                        Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                        Enabled = false // Disable buttons initially
                    };
                    btn.Click += Cell_Click;
                    panelBoard.Controls.Add(btn);
                    board[i, j] = btn;
                }
            }

            // Initialize ComboBox with default difficulty
            cbDifficulty.Items.AddRange(new string[] { "Very Easy", "Easy", "Normal", "Hard" });
            difficulty = "Very Easy";
            cbDifficulty.SelectedItem = difficulty; // Set default difficulty

            timer = new Timer
            {
                Interval = 1000 // 1 second
            };
            timer.Tick += Timer_Tick;

            // Vô hiệu hóa nút Pause và Undo khi khởi tạo game
            btnPause.Enabled = false;
            btnUndo.Enabled = false;

            UpdateStatus();
        }

        private void Cell_Click(object sender, EventArgs e)
        {
            if (!isPlayerTurn || gameEnded) return;

            Button btn = sender as Button;
            Point pos = (Point)btn.Tag;

            if (btn.Text == "") // Empty cell
            {
                btn.Text = "X";
                btn.ForeColor = Color.Blue;
                moveHistory.Push((pos, "X", isPlayerTurn));
                timeLeft = timeLimit;
                if (CheckWin(pos.X, pos.Y, "X"))
                {
                    gameEnded = true;
                    try
                    {
                        SoundPlayer player = new SoundPlayer("victorymale-version-230553.wav"); // Âm thanh khi người chơi thắng
                        player.Play();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi phát âm thanh: " + ex.Message);
                    }
                    MessageBox.Show("Player wins!", "Result");
                    timer.Stop();
                    return;
                }
                isPlayerTurn = false;
                UpdateStatus();
                BotMove();
            }
        }

        private void BotMove()
        {
            if (gameEnded) return;

            Point? bestMove = FindBestMove();
            if (bestMove.HasValue)
            {
                board[bestMove.Value.X, bestMove.Value.Y].Text = "O";
                board[bestMove.Value.X, bestMove.Value.Y].ForeColor = Color.Red;
                moveHistory.Push((bestMove.Value, "O", isPlayerTurn));
                timeLeft = timeLimit;
                if (CheckWin(bestMove.Value.X, bestMove.Value.Y, "O"))
                {
                    gameEnded = true;
                    try
                    {
                        SoundPlayer player = new SoundPlayer("you-lose-game-sound-230514.wav"); // Âm thanh khi bot thắng
                        player.Play();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi phát âm thanh: " + ex.Message);
                    }
                    MessageBox.Show("Bot wins!", "Result"); 
                    timer.Stop();
                    return;
                }
                isPlayerTurn = true;
                UpdateStatus();
            }
        }

        private Point? FindBestMove()
        {
            List<Point> candidates = GetCandidateMoves();
            if (candidates.Count == 0) return null;

            if (cbDifficulty.SelectedItem?.ToString() == "Cực dễ")
            {
                Random rand = new Random();
                return candidates[rand.Next(candidates.Count)];
            }

            int bestScore = int.MinValue;
            Point? bestMove = null;
            Random randTie = new Random();

            foreach (Point pos in candidates)
            {
                int score = EvaluateMove(pos.X, pos.Y);
                if (score > bestScore)
                {
                    bestScore = score;
                    bestMove = pos;
                }
                else if (score == bestScore && randTie.Next(2) == 0)
                {
                    bestMove = pos;
                }
            }

            return bestMove;
        }

        private List<Point> GetCandidateMoves()
        {
            List<Point> candidates = new List<Point>();
            HashSet<Point> added = new HashSet<Point>();
            // Use difficulty field as fallback if cbDifficulty.SelectedItem is null
            int radius = (cbDifficulty.SelectedItem?.ToString() == "Dễ") ? 1 : 2;

            for (int i = 0; i < BoardSize; i++)
            {
                for (int j = 0; j < BoardSize; j++)
                {
                    if (board[i, j].Text != "")
                    {
                        for (int di = -radius; di <= radius; di++)
                        {
                            for (int dj = -radius; dj <= radius; dj++)
                            {
                                int ni = i + di;
                                int nj = j + dj;
                                if (ni >= 0 && ni < BoardSize && nj >= 0 && nj < BoardSize &&
                                    board[ni, nj].Text == "" && !added.Contains(new Point(ni, nj)))
                                {
                                    candidates.Add(new Point(ni, nj));
                                    added.Add(new Point(ni, nj));
                                }
                            }
                        }
                    }
                }
            }

            if (candidates.Count == 0)
            {
                for (int i = 0; i < BoardSize; i++)
                {
                    for (int j = 0; j < BoardSize; j++)
                    {
                        if (board[i, j].Text == "")
                            candidates.Add(new Point(i, j));
                    }
                }
            }

            return candidates;
        }

        private int EvaluateMove(int x, int y)
        {
            int score = 0;
            int[][] directions = new int[][] {
                new int[] { 1, 0 },  // Horizontal
                new int[] { 0, 1 },  // Vertical
                new int[] { 1, 1 },  // Diagonal \
                new int[] { 1, -1 }  // Diagonal /
            };

            board[x, y].Text = "O";
            foreach (var dir in directions)
            {
                score += EvaluateDirection(x, y, dir[0], dir[1], "O");
                score += EvaluateDirection(x, y, dir[0], dir[1], "X") * 2;
                if (cbDifficulty.SelectedItem?.ToString() == "Khó")
                    score += EvaluateTwoWayThreat(x, y);
            }
            board[x, y].Text = "";

            return score;
        }

        private int EvaluateDirection(int x, int y, int dx, int dy, string player)
        {
            int score = 0;
            int count = 1;
            int openEnds = 0;

            for (int step = -1; step <= 1; step += 2)
            {
                int consecutive = 0;
                bool isOpen = true;
                for (int i = 1; i <= 4; i++)
                {
                    int nx = x + i * dx * step;
                    int ny = y + i * dy * step;
                    if (nx < 0 || nx >= BoardSize || ny < 0 || ny >= BoardSize)
                    {
                        isOpen = false;
                        break;
                    }
                    if (board[nx, ny].Text == player)
                        consecutive++;
                    else if (board[nx, ny].Text == "")
                    {
                        break;
                    }
                    else
                    {
                        isOpen = false;
                        break;
                    }
                }
                count += consecutive;
                if (isOpen) openEnds++;
            }

            if (count >= 5) score += 100000;
            else if (count == 4 && openEnds >= 1) score += 10000;
            else if (count == 3 && openEnds >= 1) score += 1000;
            else if (count == 2 && openEnds >= 1) score += 100;
            else if (count == 1 && openEnds >= 1) score += 10;

            return score;
        }

        private int EvaluateTwoWayThreat(int x, int y)
        {
            int score = 0;
            int[][] directions = new int[][] {
                new int[] { 1, 0 },  // Horizontal
                new int[] { 0, 1 },  // Vertical
                new int[] { 1, 1 },  // Diagonal \
                new int[] { 1, -1 }  // Diagonal /
            };

            int threatCount = 0;
            foreach (var dir in directions)
            {
                int count = 1;
                int openEnds = 0;
                for (int step = -1; step <= 1; step += 2)
                {
                    int consecutive = 0;
                    bool isOpen = true;
                    for (int i = 1; i <= 3; i++)
                    {
                        int nx = x + i * dir[0] * step;
                        int ny = y + i * dir[1] * step;
                        if (nx < 0 || nx >= BoardSize || ny < 0 || ny >= BoardSize)
                        {
                            isOpen = false;
                            break;
                        }
                        if (board[nx, ny].Text == "O")
                            consecutive++;
                        else if (board[nx, ny].Text == "")
                        {
                            break;
                        }
                        else
                        {
                            isOpen = false;
                            break;
                        }
                    }
                    count += consecutive;
                    if (isOpen) openEnds++;
                }
                if (count >= 3 && openEnds >= 1) threatCount++;
            }

            if (threatCount >= 2) score += 5000;
            return score;
        }

        private bool CheckWin(int x, int y, string player)
        {
            int[][] directions = new int[][] {
                new int[] { 1, 0 },  // Horizontal
                new int[] { 0, 1 },  // Vertical
                new int[] { 1, 1 },  // Diagonal \
                new int[] { 1, -1 }  // Diagonal /
            };

            foreach (var dir in directions)
            {
                int count = 1;
                for (int step = -1; step <= 1; step += 2)
                {
                    for (int i = 1; i < 5; i++)
                    {
                        int newX = x + i * dir[0] * step;
                        int newY = y + i * dir[1] * step;
                        if (newX >= 0 && newX < BoardSize && newY >= 0 && newY < BoardSize &&
                            board[newX, newY].Text == player)
                            count++;
                        else
                            break;
                    }
                }
                if (count >= 5) return true;
            }
            return false;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            timeLeft--;
            lblTimer.Text = $"Thời gian: {timeLeft}s";
            if (timeLeft <= 0)
            {
                timer.Stop();
                gameEnded = true;
                MessageBox.Show(isPlayerTurn ? "Bot wins! Time's up!" : "Player wins! Time's up!", "Result");
            }
        }

        private void UpdateStatus()
        {
            lblStatus.Text = gameEnded ? "Game Over" : $"Turn: {(isPlayerTurn ? "Player" : "Bot")}";
            lblTimer.Text = timer.Enabled ? $"Time: {timeLeft}s" : "Time: Not Started";
        }

        private void btnRestart_Click(object sender, EventArgs e)
        {
            try
            {
                SoundPlayer player = new SoundPlayer("click-button-140881.wav");
                player.Play();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi phát âm thanh: " + ex.Message);
            }

            try
            {
                SoundPlayer player = new SoundPlayer("3 2 1 go Green screen footage.wav");
                player.PlaySync(); // Chờ âm thanh phát xong trước khi tiếp tục
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi phát âm thanh: " + ex.Message);
            }

            foreach (Button btn in board)
            {
                btn.Text = "";
                btn.Enabled = true;
            }
            isPlayerTurn = new Random().Next(2) == 0;
            gameEnded = false;
            timeLeft = timeLimit;
            moveHistory.Clear();
            timer.Start();
            cbDifficulty.Enabled = false;
            cbTimeLimit.Enabled = false;
            // Kích hoạt nút Pause và Undo khi bắt đầu game mới
            btnPause.Enabled = true;
            btnUndo.Enabled = true;
            UpdateStatus();
            if (!isPlayerTurn)
            {

                // Bot makes a random first move
                List<Point> emptyCells = new List<Point>();
                for (int i = 0; i < BoardSize; i++)
                {
                    for (int j = 0; j < BoardSize; j++)
                    {
                        if (board[i, j].Text == "")
                            emptyCells.Add(new Point(i, j));
                    }
                }
                if (emptyCells.Count > 0)
                {
                    Random rand = new Random();
                    Point firstMove = emptyCells[rand.Next(emptyCells.Count)];
                    board[firstMove.X, firstMove.Y].Text = "O";
                    board[firstMove.X, firstMove.Y].ForeColor = Color.Red;
                    moveHistory.Push((firstMove, "O", isPlayerTurn));
                    timeLeft = timeLimit;
                    if (CheckWin(firstMove.X, firstMove.Y, "O"))
                    {
                        gameEnded = true;
                        MessageBox.Show("Bot wins!", "Result"); 
                        timer.Stop();
                        return;
                    }
                    isPlayerTurn = true;
                    UpdateStatus();
                }
            }
            if (isPaused)
                lblStatus.Text = "Game Paused";
            else
                lblStatus.Text = gameEnded ? "Game Over" : $"Turn: {(isPlayerTurn ? "Player" : "Bot")}";
            lblTimer.Text = timer.Enabled ? $"Time: {timeLeft}s" : isPaused ? "Paused" : "Time: Not Started";
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            try
            {
                SoundPlayer player = new SoundPlayer("click-button-140881.wav"); // hoặc @"Resources\click.wav" nếu trong thư mục con
                player.Play(); // hoặc .PlaySync() nếu muốn đợi phát xong mới tiếp tục
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi phát âm thanh: " + ex.Message);
            }
            Application.Exit();
        }

        private void btnUndo_Click(object sender, EventArgs e)
        {
            try
            {
                SoundPlayer player = new SoundPlayer("click-button-140881.wav"); // hoặc @"Resources\click.wav" nếu trong thư mục con
                player.Play(); // hoặc .PlaySync() nếu muốn đợi phát xong mới tiếp tục
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi phát âm thanh: " + ex.Message);
            }

            if (gameEnded || moveHistory.Count == 0) return;

            // Undo the last move
            var lastMove = moveHistory.Pop();
            board[lastMove.pos.X, lastMove.pos.Y].Text = "";
            isPlayerTurn = lastMove.wasPlayerTurn;
            timeLeft = timeLimit;
            timer.Start();
            UpdateStatus();
        }

        private void cbTimeLimit_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selected = cbTimeLimit.SelectedItem.ToString();
            timeLimit = int.Parse(selected.Split(' ')[0]);
            timeLeft = timeLimit;
            UpdateStatus();
        }

        private void cbDifficulty_SelectedIndexChanged(object sender, EventArgs e)
        {
            difficulty = cbDifficulty.SelectedItem?.ToString() ?? "Normal";
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Clean up resources
            timer?.Stop();
            timer?.Dispose();
            Application.Exit();
        }

        private void lblTimer_Click(object sender, EventArgs e)
        {

        }

        private void lblDifficulty_Click(object sender, EventArgs e)
        {

        }
    }
}