using System;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Collections.Generic;
using Firebase.Database;
using Firebase.Database.Query;
using System.Threading.Tasks;
using System.Linq;
using System.Media;


namespace DoAnMonHocNT106
{
    public partial class Form1 : Form
    {
        private const int GridSize = 20;
        private const int CellSize = 30;
        private Button[,] buttons;
        private bool isPlayerX = true;
        private bool isConnected = false;
        private bool isServer = false;
        private TcpListener server;
        private TcpClient client;
        private NetworkStream stream;
        private List<Point> moveHistory = new List<Point>();
        private int timeLimit = 30;
        private int timeRemaining;
        private System.Windows.Forms.Timer timer;
        private bool gameStarted = false; // Reintroduced: Track if game has started
        private System.Timers.Timer processCheckTimer;
        private FirebaseClient firebaseClient;
        private string currentUsername; // Store the current player's username (baongdqu1 or baongdqu2)
        private bool isNewGameRequested = false; // Trạng thái yêu cầu trận mới
        private bool isWaitingForOpponentConfirmation = false; // Đang chờ xác nhận từ đối thủ
        private string opponentUsername; // Lưu tên đối thủ
        private string receiveBuffer = "";
        private bool isGameOver = false; // Thêm cờ isGameOver


        public Form1()
        {
            InitializeComponent();
            setTimeComboBox.SelectedItem = $"{timeLimit}s"; // Hiển thị giá trị mặc định trong setTimeComboBox
            timeLabel.Text = $"Time: {timeLimit}s";         // Cập nhật timeLabel cho nhất quán
            InitializeGameBoard();
            InitializeFirebase();
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 1000;
            timer.Tick += Timer_Tick;
            setTimeComboBox.Visible = false;
            UpdateUI();

            processCheckTimer = new System.Timers.Timer(1000);
            processCheckTimer.Elapsed += ProcessCheckTimer_Elapsed;
            processCheckTimer.AutoReset = true;
            processCheckTimer.Start();

            // Danh sách tên người dùng hợp lệ
            List<string> validUsernames = new List<string> { "baongdqu1", "baongdqu2", "baongdqu3", "baongdqu4", "baongdqu5" };

            // Retrieve username from command-line arguments
            string[] args = Environment.GetCommandLineArgs();
            currentUsername = args.Length > 1 ? args[1] : null;

            // If no username is provided via command-line, prompt for it
            if (string.IsNullOrEmpty(currentUsername))
            {
                currentUsername = Prompt.ShowDialog("Nhập tên người dùng (baongdqu1, baongdqu2, baongdqu3, baongdqu4, baongdqu5):", "Nhập tên người dùng");
                while (!validUsernames.Contains(currentUsername, StringComparer.OrdinalIgnoreCase))
                {
                    MessageBox.Show("Tên người dùng không hợp lệ. Vui lòng nhập một trong các tên: baongdqu1, baongdqu2, baongdqu3, baongdqu4, baongdqu5.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    currentUsername = Prompt.ShowDialog("Nhập tên người dùng (baongdqu1, baongdqu2, baongdqu3, baongdqu4, baongdqu5):", "Nhập tên người dùng");
                    if (string.IsNullOrEmpty(currentUsername))
                    {
                        MessageBox.Show("Tên người dùng không được để trống.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Application.Exit();
                        return;
                    }
                }
            }
            else if (!validUsernames.Contains(currentUsername, StringComparer.OrdinalIgnoreCase))
            {
                MessageBox.Show("Tên người dùng không hợp lệ. Phải là một trong: baongdqu1, baongdqu2, baongdqu3, baongdqu4, baongdqu5.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
                return;
            }

            // Cập nhật giao diện với tên người dùng
            yourNameLabel.Text = currentUsername;
            yourDetailsButton.Enabled = true;
        }

        private async Task UpdatePlayerStatsAsync(bool isWin, bool isDraw = false)
        {
            try
            {
                // Validate username
                if (string.IsNullOrEmpty(currentUsername))
                {
                    MessageBox.Show("Tên người dùng không được để trống.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Fetch current player data
                var player = await firebaseClient
                    .Child("Users")
                    .Child(currentUsername)
                    .OnceSingleAsync<PlayerData>();

                if (player == null)
                {
                    MessageBox.Show($"User {currentUsername} not found in database.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Update stats
                player.GamesPlayed += 1;
                if (isDraw)
                {
                    // No change to Wins or Losses for a draw
                }
                else if (isWin)
                {
                    player.Wins += 1;
                }
                else
                {
                    player.Losses += 1;
                }

                // Calculate WinRate
                player.WinRate = player.GamesPlayed > 0 ? (double)player.Wins / player.GamesPlayed * 100 : 0;

                // Update Firebase with existing data
                await firebaseClient
                    .Child("Users")
                    .Child(currentUsername)
                    .PutAsync(player);

                // Debug output
                System.Diagnostics.Debug.WriteLine($"Updated stats for {currentUsername}: GamesPlayed={player.GamesPlayed}, Wins={player.Wins}, Losses={player.Losses}, WinRate={player.WinRate:F2}%");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error updating player stats: {ex.Message}");
                MessageBox.Show($"Error updating stats: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitializeFirebase()
        {
            // Initialize Firebase client
            firebaseClient = new FirebaseClient("https://nt106-7c9fe-default-rtdb.firebaseio.com/");
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

        private void UpdateUI()
        {
            setTimeComboBox.Visible = isConnected;
            setTimeComboBox.Enabled = isConnected && isServer && !gameStarted && !isWaitingForOpponentConfirmation;
            newGameButton.Enabled = isConnected && !isWaitingForOpponentConfirmation;
            resetButton.Enabled = isConnected;
            undoButton.Enabled = isConnected && gameStarted && moveHistory.Count > 0;
            chatBox.Enabled = isConnected;
            sendButton.Enabled = isConnected;
            yourDetailsButton.Enabled = !string.IsNullOrEmpty(currentUsername);
            opponentDetailsButton.Enabled = isConnected && !string.IsNullOrEmpty(opponentUsername);
            yourNameLabel.Text = !string.IsNullOrEmpty(currentUsername) ? currentUsername : "";
            opponentNameLabel.Text = isConnected && !string.IsNullOrEmpty(opponentUsername) ? opponentUsername : "";
        }

        private void InitializeGameBoard()
        {
            buttons = new Button[GridSize, GridSize];
            for (int i = 0; i < GridSize; i++)
            {
                for (int j = 0; j < GridSize; j++)
                {
                    Button btn = new Button
                    {
                        Size = new Size(CellSize - 2, CellSize - 2),
                        Location = new Point(j * CellSize, i * CellSize),
                        Name = $"button{i}_{j}",
                        Tag = new Point(i, j),
                        Font = new Font("Arial", 12, FontStyle.Bold)
                    };
                    btn.Click += Button_Click;
                    gamePanel.Controls.Add(btn);
                    buttons[i, j] = btn;
                }
            }
        }

        private void Button_Click(object sender, EventArgs e)
        {
            if (!isConnected || !gameStarted || (isServer && !isPlayerX) || (!isServer && isPlayerX)) return;

            Button btn = sender as Button;
            Point pos = (Point)btn.Tag;

            if (btn.Text != "") return;

            string symbol = isPlayerX ? "X" : "O";
            btn.Text = symbol;
            if (symbol == "X") btn.ForeColor = Color.Red;
            else btn.ForeColor = Color.Blue;

            moveHistory.Add(pos);

            if (isConnected)
            {
                string moveData = $"{pos.X},{pos.Y},{symbol}";
                SendNetworkData(moveData);
            }

            if (CheckWin(pos.X, pos.Y, symbol) && !isGameOver)
            {
                isGameOver = true;
                timer.Stop();
                MessageBox.Show("You win!", "Game Over", MessageBoxButtons.OK, MessageBoxIcon.Information);
                statusLabel.Text = "You win!";
                DisableBoard();
                gameStarted = false;
                UpdateUI();
                Task.Run(() => UpdatePlayerStatsAsync(true));
                try
                {
                    SoundPlayer player = new SoundPlayer("victorymale-version-230553.wav");
                    player.Play();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi phát âm thanh: " + ex.Message);
                }
            }
            else if (moveHistory.Count == GridSize * GridSize)
            {
                isGameOver = true;
                timer.Stop();
                MessageBox.Show("Game ends in a draw!", "Game Over", MessageBoxButtons.OK, MessageBoxIcon.Information);
                statusLabel.Text = "Game ends in a draw!";
                DisableBoard();
                gameStarted = false;
                UpdateUI();
                Task.Run(() => UpdatePlayerStatsAsync(false, true));
            }
            else
            {
                timer.Stop();
                isPlayerX = !isPlayerX;
                UpdateStatusLabel();
                StartTimer();
            }
        }

        private void StartTimer()
        {
            timeRemaining = timeLimit;
            timeLabel.Text = $"Time: {timeRemaining}s";
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            timeRemaining--;
            timeLabel.Text = $"Time: {timeRemaining}s";
            if (timeRemaining <= 0)
            {
                timer.Stop();

                // Kiểm tra xem người chơi có đang ở lượt đi của mình hay không
                bool isYourTurn = (isServer && isPlayerX) || (!isServer && !isPlayerX);

                if (isYourTurn)
                {
                    // Nếu là lượt của người chơi hiện tại, thực hiện nước đi ngẫu nhiên
                    //MessageBox.Show("Hết thời gian! Đánh ngẫu nhiên cho bạn.", "Hết giờ", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    List<Button> emptyButtons = new List<Button>();
                    for (int i = 0; i < GridSize; i++)
                    {
                        for (int j = 0; j < GridSize; j++)
                        {
                            if (buttons[i, j].Text == "")
                            {
                                emptyButtons.Add(buttons[i, j]);
                            }
                        }
                    }

                    if (emptyButtons.Count > 0)
                    {
                        Random rand = new Random();
                        int index = rand.Next(emptyButtons.Count);
                        Button randomButton = emptyButtons[index];
                        randomButton.PerformClick(); // Gọi sự kiện Button_Click để xử lý nước đi
                    }
                    else
                    {
                        MessageBox.Show("Không còn ô trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                // Nếu không phải lượt của người chơi, không làm gì cả
            }
        }

        private void SetTimeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (setTimeComboBox.SelectedItem != null && isServer && !gameStarted) // Only server, before game starts
            {
                string selectedTime = setTimeComboBox.SelectedItem.ToString();
                timeLimit = int.Parse(selectedTime.Replace("s", ""));
                timeLabel.Text = $"Time: {selectedTime}";
                timeRemaining = timeLimit;
                if (isConnected)
                {
                    SendNetworkData($"TIMELIMIT:{timeLimit}"); // Sync with client
                }
            }
        }

        private async void HostGame_Click(object sender, EventArgs e)
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
                server = new TcpListener(IPAddress.Any, 12345);
                server.Start();
                isServer = true;
                isConnected = true;
                youLabel.Text = "You: X (Red)";
                opponentLabel.Text = "Opponent: O (Blue)";
                gameStarted = false;

                // Lấy địa chỉ IP của máy chủ
                string hostName = Dns.GetHostName();
                IPAddress[] addresses = Dns.GetHostAddresses(hostName);
                string ipAddress = "Unknown";
                foreach (IPAddress address in addresses)
                {
                    if (address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        ipAddress = address.ToString();
                        break;
                    }
                }

                // Đăng ký host lên Firebase
                await RegisterHostAsync(ipAddress);

                statusLabel.Text = $"Đang tạo server tại IP: {ipAddress}, Port: 12345. Chờ đối thủ...";
                UpdateUI();

                Thread listenThread = new Thread(ListenForClient);
                listenThread.IsBackground = true;
                listenThread.Start();
            }
            catch (Exception ex)
            {
                statusLabel.Text = $"Lỗi khi tạo server: {ex.Message}";
            }
        }

        private void JoinGame_Click(object sender, EventArgs e)
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

            string ip = Prompt.ShowDialog("Nhập IP của server:", "Tham gia trò chơi");
            if (string.IsNullOrEmpty(ip)) return;

            try
            {
                client = new TcpClient(ip, 12345);
                stream = client.GetStream();
                isConnected = true;
                isServer = false;
                youLabel.Text = "You: O (Blue)";
                opponentLabel.Text = "Opponent: X (Red)";
                gameStarted = false;

                // Gửi tên người chơi đến server
                SendNetworkData($"USERNAME:{currentUsername}");

                statusLabel.Text = "Đã kết nối đến server. Chờ bắt đầu.";
                UpdateUI();

                Thread receiveThread = new Thread(ReceiveNetworkData);
                receiveThread.IsBackground = true;
                receiveThread.Start();
            }
            catch (Exception ex)
            {
                statusLabel.Text = $"Lỗi khi tham gia: {ex.Message}";
                UpdateUI();
            }
        }

        private async void FindRandom_Click(object sender, EventArgs e)
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

            statusLabel.Text = "Đang tìm kiếm trận đấu ngẫu nhiên...";
            try
            {
                // Lấy danh sách host từ Firebase
                var hosts = await firebaseClient
                    .Child("Hosts")
                    .OnceAsync<object>();

                if (hosts.Count == 0)
                {
                    statusLabel.Text = "Không có host nào đang mở.";
                    return;
                }

                // Chọn một host ngẫu nhiên
                var randomHost = hosts.ElementAt(new Random().Next(hosts.Count));
                dynamic hostData = randomHost.Object;
                string ipAddress = hostData.IP.ToString();
                int port = int.Parse(hostData.Port.ToString());

                // Kết nối đến host ngẫu nhiên
                client = new TcpClient(ipAddress, port);
                stream = client.GetStream();
                isConnected = true;
                isServer = false;
                youLabel.Text = "You: O (Blue)";
                opponentLabel.Text = "Opponent: X (Red)";
                gameStarted = false;
                UpdateUI();
                statusLabel.Text = "Đã kết nối đến trận đấu ngẫu nhiên. Chờ bắt đầu.";

                // Gửi tên người chơi đến server
                SendNetworkData($"USERNAME:{currentUsername}");

                Thread receiveThread = new Thread(ReceiveNetworkData);
                receiveThread.IsBackground = true;
                receiveThread.Start();
            }
            catch (Exception ex)
            {
                statusLabel.Text = $"Lỗi khi tìm kiếm trận đấu: {ex.Message}";
            }
        }

        private async Task RegisterHostAsync(string ipAddress)
        {
            try
            {
                var hostData = new { IP = ipAddress, Port = 12345, Username = currentUsername };
                await firebaseClient
                    .Child("Hosts")
                    .Child(currentUsername)
                    .PutAsync(hostData);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi đăng ký host: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task UnregisterHostAsync()
        {
            try
            {
                await firebaseClient
                    .Child("Hosts")
                    .Child(currentUsername)
                    .DeleteAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi khi hủy đăng ký host: {ex.Message}");
            }
        }

        private void NewGameButton_Click(object sender, EventArgs e)
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

            if (!isConnected)
            {
                MessageBox.Show("Chưa kết nối. Vui lòng tạo hoặc tham gia trò chơi!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!isServer)
            {
                MessageBox.Show("Chỉ máy chủ có thể khởi tạo trận mới.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (isWaitingForOpponentConfirmation)
            {
                MessageBox.Show("Đang chờ đối thủ xác nhận trận mới.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult confirmResult = MessageBox.Show("Bạn có muốn bắt đầu trận mới?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmResult == DialogResult.Yes)
            {
                isNewGameRequested = true;
                isWaitingForOpponentConfirmation = true;
                SendNetworkData("NEWGAME_REQUEST");
                statusLabel.Text = "Đang chờ đối thủ xác nhận trận mới...";
                UpdateUI();
            }
        }


        private void ResetButton_Click(object sender, EventArgs e)
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

            DialogResult confirmResult = MessageBox.Show("Bạn có chắc chắn muốn thoát trận đấu?", "Xác nhận thoát", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmResult == DialogResult.Yes)
            {
                if (isConnected)
                {
                    SendNetworkData("RESET");
                }
                CloseConnection();
                ResetGame();
                gameStarted = false;
                youLabel.Text = "";
                opponentLabel.Text = "";
                opponentUsername = null;
                statusLabel.Text = "Đã ngắt kết nối. Sẵn sàng tìm trận mới.";
                UpdateUI();
            }
        }

        private void UndoButton_Click(object sender, EventArgs e)
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

            if (moveHistory.Count == 0 || !isConnected || !gameStarted) return;

            Point lastMove = moveHistory[moveHistory.Count - 1];
            buttons[lastMove.X, lastMove.Y].Text = "";
            moveHistory.RemoveAt(moveHistory.Count - 1);
            isPlayerX = !isPlayerX;
            if (isConnected) SendNetworkData($"UNDO,{lastMove.X},{lastMove.Y}");
            UpdateStatusLabel();
        }

        private void ExitButton_Click(object sender, EventArgs e)
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
                CloseConnection();
                processCheckTimer?.Stop();
                processCheckTimer?.Dispose();
                Application.Exit();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi khi thoát: {ex.Message}");
            }
        }

        private void SendButton_Click(object sender, EventArgs e)
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

            if (!isConnected || string.IsNullOrEmpty(chatBox.Text)) return;

            string message = chatBox.Text;
            chatHistory.AppendText($"You: {message}\r\n");
            SendNetworkData($"CHAT:{message}");
            chatBox.Clear();
        }

        private void ListenForClient()
        {
            try
            {
                client = server.AcceptTcpClient();
                stream = client.GetStream();
                isConnected = true;
                gameStarted = false;

                // Gửi tín hiệu kết nối và tên của server
                SendNetworkData("CONNECTED");
                SendNetworkData($"USERNAME:{currentUsername}");

                this.Invoke((MethodInvoker)delegate
                {
                    statusLabel.Text = "Đã kết nối với đối thủ. Chờ bắt đầu.";
                    UpdateUI();
                });

                Thread receiveThread = new Thread(ReceiveNetworkData);
                receiveThread.IsBackground = true;
                receiveThread.Start();
            }
            catch (Exception ex)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    statusLabel.Text = $"Lỗi kết nối: {ex.Message}";
                    CloseConnection();
                    UpdateUI();
                });
            }
        }


        private void SendNetworkData(string data)
        {
            if (stream == null || !isConnected)
            {
                System.Diagnostics.Debug.WriteLine("Không thể gửi dữ liệu: Không có kết nối.");
                return;
            }
            try
            {
                System.Diagnostics.Debug.WriteLine("Sent: " + data);
                byte[] buffer = Encoding.UTF8.GetBytes(data + "\n");
                stream.Write(buffer, 0, buffer.Length);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi gửi dữ liệu mạng: {ex.Message}");
                statusLabel.Text = $"Lỗi kết nối: {ex.Message}";
                CloseConnection();
                UpdateUI();
            }
        }

        private void ReceiveNetworkData()
        {
            try
            {
                byte[] buffer = new byte[1024];
                while (isConnected)
                {
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    if (bytesRead == 0)
                    {
                        throw new Exception("Kết nối bị đóng bởi đối thủ.");
                    }
                    string receivedData = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    receiveBuffer += receivedData;

                    // Xử lý từng thông điệp hoàn chỉnh
                    while (receiveBuffer.Contains("\n"))
                    {
                        int index = receiveBuffer.IndexOf("\n");
                        string message = receiveBuffer.Substring(0, index).Trim();
                        receiveBuffer = receiveBuffer.Substring(index + 1);

                        this.Invoke((MethodInvoker)delegate
                        {
                            if (message.StartsWith("TIMELIMIT:"))
                            {
                                string[] parts = message.Split(':');
                                if (parts.Length == 2 && int.TryParse(parts[1], out int newTimeLimit))
                                {
                                    timeLimit = newTimeLimit;
                                    timeLabel.Text = $"Time: {timeLimit}s";
                                    timeRemaining = timeLimit;
                                    setTimeComboBox.SelectedIndex = setTimeComboBox.Items.IndexOf($"{timeLimit}s");
                                }
                            }
                            else
                            {
                                ProcessNetworkData(message);
                            }
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi ReceiveNetworkData: {ex.Message}");
                this.Invoke((MethodInvoker)delegate
                {
                    statusLabel.Text = $"Mất kết nối: {ex.Message}";
                    CloseConnection();
                    gameStarted = false;
                    UpdateUI();
                });
            }
        }

        private void ProcessNetworkData(string data)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("Received: " + data);

                if (data == "NEWGAME_REQUEST" && !isWaitingForOpponentConfirmation)
                {
                    DialogResult result = MessageBox.Show("Đối thủ muốn bắt đầu trận mới. Bạn có đồng ý?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        SendNetworkData("NEWGAME_ACCEPT");
                        gameStarted = true;
                        isNewGameRequested = false;
                        isWaitingForOpponentConfirmation = false;
                        ResetGame();
                        UpdateUI();
                        statusLabel.Text = isServer ? "Đến lượt bạn" : "Đến lượt đối thủ";
                        if (isServer) SendNetworkData($"TIMELIMIT:{timeLimit}");
                        StartTimer();
                    }
                    else
                    {
                        SendNetworkData("NEWGAME_DECLINE");
                        statusLabel.Text = "Bạn đã từ chối bắt đầu trận mới.";
                        UpdateUI();
                    }
                    return;
                }
                else if (data == "NEWGAME_ACCEPT")
                {
                    if (isWaitingForOpponentConfirmation)
                    {
                        isNewGameRequested = false;
                        isWaitingForOpponentConfirmation = false;
                        gameStarted = true;
                        ResetGame();
                        UpdateUI();
                        statusLabel.Text = isServer ? "Đến lượt bạn" : "Đến lượt đối thủ";
                        if (isServer) SendNetworkData($"TIMELIMIT:{timeLimit}");
                        StartTimer();
                        MessageBox.Show("Đối thủ đã đồng ý. Trò chơi bắt đầu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        try
                        {
                            SoundPlayer player = new SoundPlayer("3 2 1 go Green screen footage.wav");
                            player.Play();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Lỗi phát âm thanh: " + ex.Message);
                        }
                    }
                    return;
                }
                else if (data == "NEWGAME_DECLINE")
                {
                    if (isWaitingForOpponentConfirmation)
                    {
                        isNewGameRequested = false;
                        isWaitingForOpponentConfirmation = false;
                        statusLabel.Text = "Đối thủ từ chối bắt đầu trận mới.";
                        MessageBox.Show("Đối thủ từ chối bắt đầu trận mới.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        UpdateUI();
                    }
                    return;
                }
                if (data.StartsWith("CHAT:"))
                {
                    string message = data.Substring(5);
                    chatHistory.AppendText($"Opponent: {message}\r\n");
                    return;
                }
                if (data == "CONNECTED")
                {
                    statusLabel.Text = "Đã kết nối với đối thủ.";
                    return;
                }
                if (data == "RESET")
                {
                    MessageBox.Show("Đối thủ đã thoát trận đấu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CloseConnection();
                    ResetGame();
                    gameStarted = false;
                    youLabel.Text = "";
                    opponentLabel.Text = "";
                    opponentUsername = null;
                    statusLabel.Text = "Đối thủ đã thoát. Sẵn sàng tìm trận mới.";
                    UpdateUI();
                    return;
                }
                if (data.StartsWith("USERNAME:"))
                {
                    string usernameData = data.Substring(9).Trim();
                    if (usernameData.EndsWith("CONNECTED"))
                    {
                        usernameData = usernameData.Replace("CONNECTED", "").Trim();
                    }
                    if (!string.IsNullOrEmpty(usernameData))
                    {
                        opponentUsername = usernameData;
                        opponentNameLabel.Text = opponentUsername;
                        opponentDetailsButton.Enabled = true;
                        UpdateUI();
                        statusLabel.Text = "Đã nhận thông tin đối thủ.";
                    }
                    else
                    {
                        statusLabel.Text = "Tên đối thủ không hợp lệ.";
                    }
                    return;
                }
                if (!gameStarted) return;

                if (data.StartsWith("UNDO,"))
                {
                    string[] parts = data.Split(',');
                    if (parts.Length != 3) return;
                    if (!int.TryParse(parts[1], out int x) || !int.TryParse(parts[2], out int y)) return;
                    buttons[x, y].Text = "";
                    moveHistory.RemoveAt(moveHistory.Count - 1);
                    isPlayerX = !isPlayerX;
                    UpdateStatusLabel();
                }
                else if (data == "TIMEOUT")
                {
                    timer.Stop();
                    MessageBox.Show("Đối thủ hết thời gian! Đến lượt bạn.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    isPlayerX = !isPlayerX;
                    UpdateStatusLabel();
                    StartTimer();
                }
                else
                {
                    string[] parts = data.Split(',');
                    if (parts.Length != 3) return;
                    if (!int.TryParse(parts[0], out int x) || !int.TryParse(parts[1], out int y))
                    {
                        System.Diagnostics.Debug.WriteLine("Lỗi định dạng nước đi: " + data);
                        return;
                    }
                    string symbol = parts[2];
                    buttons[x, y].Text = symbol;
                    if (symbol == "X") buttons[x, y].ForeColor = Color.Red;
                    else buttons[x, y].ForeColor = Color.Blue;
                    moveHistory.Add(new Point(x, y));
                    if (CheckWin(x, y, symbol) && !isGameOver)
                    {
                        isGameOver = true;
                        timer.Stop();
                        // Chỉ hiển thị thông báo thua nếu client không phải người thắng
                        if (!isServer && symbol == "X" || isServer && symbol == "O")
                        {
                            MessageBox.Show("Bạn đã thua!", "Kết thúc trò chơi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            statusLabel.Text = "Bạn đã thua!";
                            Task.Run(() => UpdatePlayerStatsAsync(false));
                            try
                            {
                                SoundPlayer player = new SoundPlayer("you-lose-game-sound-230514.wav");
                                player.Play();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Lỗi phát âm thanh: " + ex.Message);
                            }
                        }
                        DisableBoard();
                        gameStarted = false;
                        UpdateUI();
                    }
                    else if (moveHistory.Count == GridSize * GridSize)
                    {
                        isGameOver = true;
                        timer.Stop();
                        MessageBox.Show("Trò chơi kết thúc hòa!", "Kết thúc trò chơi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        statusLabel.Text = "Trò chơi kết thúc hòa!";
                        DisableBoard();
                        gameStarted = false;
                        UpdateUI();
                        Task.Run(() => UpdatePlayerStatsAsync(false, true));
                    }
                    else
                    {
                        timer.Stop();
                        isPlayerX = !isPlayerX;
                        UpdateStatusLabel();
                        StartTimer();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi xử lý dữ liệu mạng: {ex.Message}");
                statusLabel.Text = $"Lỗi kết nối: {ex.Message}";
                CloseConnection();
                UpdateUI();
            }
        }

        private async void YourDetailsButton_Click(object sender, EventArgs e)
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
                var player = await firebaseClient
                    .Child("Users")
                    .Child(currentUsername)
                    .OnceSingleAsync<PlayerData>();

                if (player == null)
                {
                    MessageBox.Show($"Không tìm thấy thông tin cho {currentUsername}.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string details = $"Username: {player.Username}\n" +
                                $"Avatar: {player.AvatarLink}\n" +
                                $"Country: {player.Country}\n" +
                                $"Date of Birth: {player.DateOfBirth}\n" +
                                $"Games Played: {player.GamesPlayed}\n" +
                                $"Wins: {player.Wins}\n" +
                                $"Losses: {player.Losses}\n" +
                                $"Win Rate: {player.WinRate:F2}%\n" +
                                $"Status: {player.Status}";
                MessageBox.Show(details, $"Thông tin của {currentUsername}", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lấy thông tin: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void OpponentDetailsButton_Click(object sender, EventArgs e)
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
            if (string.IsNullOrEmpty(opponentUsername))
            {
                MessageBox.Show("Không có thông tin đối thủ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                var player = await firebaseClient
                    .Child("Users")
                    .Child(opponentUsername)
                    .OnceSingleAsync<PlayerData>();

                if (player == null)
                {
                    MessageBox.Show($"Không tìm thấy thông tin cho {opponentUsername}.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string details = $"Username: {player.Username}\n" +
                                $"Avatar: {player.AvatarLink}\n" +
                                $"Country: {player.Country}\n" +
                                $"Date of Birth: {player.DateOfBirth}\n" +
                                $"Games Played: {player.GamesPlayed}\n" +
                                $"Wins: {player.Wins}\n" +
                                $"Losses: {player.Losses}\n" +
                                $"Win Rate: {player.WinRate:F2}%\n" +
                                $"Status: {player.Status}";
                MessageBox.Show(details, $"Thông tin của {opponentUsername}", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lấy thông tin: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateStatusLabel()
        {
            if (!isConnected)
            {
                statusLabel.Text = "Not connected.";
            }
            else if (!gameStarted)
            {
                statusLabel.Text = "Waiting to start the game.";
            }
            else if (isServer)
            {
                statusLabel.Text = isPlayerX ? "Your turn" : "Opponent's turn";
            }
            else
            {
                statusLabel.Text = isPlayerX ? "Opponent's turn" : "Your turn";
            }
        }

        private bool CheckWin(int x, int y, string symbol)
        {
            int[][] directions = new int[][] {
                new int[] { 0, 1 },
                new int[] { 1, 0 },
                new int[] { 1, 1 },
                new int[] { 1, -1 }
            };

            foreach (var dir in directions)
            {
                int count = 1;
                for (int i = -4; i <= 4; i++)
                {
                    if (i == 0) continue;
                    int newX = x + i * dir[0];
                    int newY = y + i * dir[1];
                    if (newX >= 0 && newX < GridSize && newY >= 0 && newY < GridSize &&
                        buttons[newX, newY].Text == symbol)
                    {
                        count++;
                    }
                    else if (count >= 5)
                    {
                        break;
                    }
                    else
                    {
                        count = 1;
                    }
                }
                if (count >= 5) return true;
            }
            return false;
        }

        private void ResetGame()
        {
            timer.Stop();
            foreach (Button btn in buttons)
            {
                btn.Text = "";
                btn.ForeColor = SystemColors.ControlText;
                btn.Enabled = true;
            }
            moveHistory.Clear();
            isPlayerX = true;
            timeLabel.Text = $"Time: {timeLimit}s";
            timeRemaining = timeLimit;
        }

        private void DisableBoard()
        {
            foreach (Button btn in buttons)
            {
                btn.Enabled = false;
            }
        }

        private async void CloseConnection()
        {
            try
            {
                timer?.Stop();
                isConnected = false;
                gameStarted = false;
                opponentUsername = null;
                opponentNameLabel.Text = "";
                opponentDetailsButton.Enabled = false;

                if (stream != null)
                {
                    stream.Close();
                    stream.Dispose();
                    stream = null;
                }

                if (client != null)
                {
                    client.Close();
                    client.Dispose();
                    client = null;
                }

                if (server != null)
                {
                    server.Stop();
                    server = null;
                }

                // Hủy đăng ký host khỏi Firebase nếu là server
                if (isServer)
                {
                    await UnregisterHostAsync();
                }

                UpdateUI();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi khi đóng kết nối: {ex.Message}");
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            try
            {
                // Dừng timer kiểm tra tiến trình
                processCheckTimer?.Stop();
                processCheckTimer?.Dispose();

                // Đóng tất cả kết nối mạng
                CloseConnection();

                base.OnFormClosing(e);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi khi đóng form: {ex.Message}");
            }
        }

        private void gamePanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void timeLabel_Click(object sender, EventArgs e)
        {

        }

        private void chatBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void ruleLabel_Click(object sender, EventArgs e)
        {

        }

        private void statusLabel_TextChanged(object sender, EventArgs e)
        {

        }

        private void youLabel_Click(object sender, EventArgs e)
        {

        }

        private void opponentLabel_Click(object sender, EventArgs e)
        {

        }

        private void chatHistory_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }

    public static class Prompt
    {
        public static string ShowDialog(string text, string caption)
        {
            Form prompt = new Form()
            {
                Width = 300,
                Height = 150,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen
            };
            Label textLabel = new Label() { Left = 50, Top = 20, Text = text };
            TextBox textBox = new TextBox() { Left = 50, Top = 50, Width = 200 };
            Button confirmation = new Button() { Text = "OK", Left = 150, Width = 100, Top = 80, DialogResult = DialogResult.OK };
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.AcceptButton = confirmation;

            return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : "";
        }
    }

    public class PlayerData
    {
        public string AvatarLink { get; set; }
        public string Country { get; set; }
        public string DateOfBirth { get; set; }
        public string Email { get; set; }
        public int GamesPlayed { get; set; }
        public int Losses { get; set; }
        public string Password { get; set; }
        public string Status { get; set; }
        public string Username { get; set; }
        public double WinRate { get; set; }
        public int Wins { get; set; }
    }


}
