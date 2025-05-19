using GameCaro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace DoAnMonHocNT106
{
    public partial class Form1: Form
    {
        #region Properties
        ChessBoardManager ChessBoard;
        SocketManager socket;
        private string chatHistory = ""; // Lưu lịch sử chat
        private bool isChatEnabled = false; // Trạng thái khung chat
        #endregion

        public Form1()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
            ChessBoard = new ChessBoardManager(pnlChessBoard,txbPlayerName, pctbMark);
            ChessBoard.EndedGame += ChessBoard_EndedGame;
            ChessBoard.PlayerMarked += ChessBoard_PlayerMarked;
            prcbCoolDown.Step = Cons.COOL_DOWN_STEP;
            prcbCoolDown.Maximum = Cons.COOL_DOWN_TIME;
            prcbCoolDown.Value = 0;

            tmCoolDown.Interval = Cons.COOL_DOWN_INTERVAL;
            socket = new SocketManager();

            NewGame();
            InitializeChat(); // Khởi tạo khung chat
        }

        #region Methods
        private void InitializeChat()
        {
            // Vô hiệu hóa khung chat khi khởi tạo
            SetChatEnabled(false);
            // Thêm sự kiện KeyDown cho textBoxChatInput
            textBoxChatInput.KeyDown += TextBoxChatInput_KeyDown;
        }

        private void TextBoxChatInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !e.Shift && isChatEnabled)
            {
                e.SuppressKeyPress = true; // Ngăn Enter tạo dòng mới
                buttonSendChat_Click(sender, EventArgs.Empty); // Gọi logic gửi tin nhắn
            }
        }

        private void SetChatEnabled(bool enabled)
        {
            isChatEnabled = enabled;
            textBoxChatInput.Enabled = enabled;
            buttonSendChat.Enabled = enabled;
            if (!enabled)
            {
                textBoxChatInput.Text = "";
            }
        }

        private void buttonSendChat_Click(object sender, EventArgs e)
        {
            if (!isChatEnabled) return;

            string message = textBoxChatInput.Text.Trim();
            if (!string.IsNullOrEmpty(message))
            {
                string formattedMessage = $"[{txbPlayerName.Text}]: {message}\n";
                chatHistory += formattedMessage;
                richTextBoxChat.Text = chatHistory;
                textBoxChatInput.Clear();

                // Gửi tin nhắn qua socket
                socket.Send(new SocketData((int)SocketCommand.CHAT, formattedMessage, new Point()));
            }
        }

        void EndGame()
        {
            tmCoolDown.Stop();
            pnlChessBoard.Enabled = false;
            undoToolStripMenuItem.Enabled = false;
            //MessageBox.Show("Kết thúc");
            SetChatEnabled(true); // Kích hoạt chat khi kết thúc
        }
        void NewGame()
        {
            prcbCoolDown.Value = 0;
            tmCoolDown.Stop();
            undoToolStripMenuItem.Enabled = true;
            ChessBoard.DrawChessBoard();
            chatHistory = "";
            richTextBoxChat.Text = "";
            SetChatEnabled(true); // Kích hoạt chat khi bắt đầu
        }

        void Quit()
        {
            
            Application.Exit();
        }

        void Undo()
        {
            if (ChessBoard.Undo())
            {
                socket.Send(new SocketData((int)SocketCommand.UNDO, "", new Point()));
                prcbCoolDown.Value = 0;
            }
        }

        private void ChessBoard_PlayerMarked(object sender, ButtonClickEvent e)
        {

            tmCoolDown.Start();
            pnlChessBoard.Enabled = false;
            prcbCoolDown.Value = 0;
            SetChatEnabled(true); // Tắt chat khi đang chơi
            socket.Send(new SocketData((int)SocketCommand.SEND_POINT, "", e.ClickedPoint));
            undoToolStripMenuItem.Enabled = false;

            Listen();
        }

        private void ChessBoard_EndedGame(object sender, EventArgs e)
        {
            EndGame();
            socket.Send(new SocketData((int)SocketCommand.END_GAME, "", new Point()));

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void tmCoolDown_Tick(object sender, EventArgs e)
        {
            prcbCoolDown.PerformStep();

            if (prcbCoolDown.Value >= prcbCoolDown.Maximum)
            {
                EndGame();
                socket.Send(new SocketData((int)SocketCommand.TIME_OUT, "", new Point()));
            }

        }

        private void prcbCoolDown_Click(object sender, EventArgs e)
        {

        }

        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewGame();
            socket.Send(new SocketData((int)SocketCommand.NEW_GAME, "", new Point()));
            pnlChessBoard.Enabled = true;
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Undo();
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Quit();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {

            if (MessageBox.Show("Bạn có chắc muốn thoát", "Thông báo", MessageBoxButtons.OKCancel) != System.Windows.Forms.DialogResult.OK)
            {
                e.Cancel = true;
            }
            else
            {
                try
                {
                    socket.Send(new SocketData((int)SocketCommand.QUIT, "", new Point()));
                }
                catch { }
            }
        }

        private void btnHostGame_Click(object sender, EventArgs e)
        {
            socket.IP = txbIP.Text;
            socket.isServer = true;
            socket.CreateServer();
            pnlChessBoard.Enabled = true;
            MessageBox.Show($"Đang chờ kết nối tại IP: {txbIP.Text}", "Server", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnJoinGame_Click(object sender, EventArgs e)
        {
            socket.IP = txbIP.Text;
            if (socket.ConnectServer())
            {
                socket.isServer = false;
                pnlChessBoard.Enabled = false;
                socket.Send(new SocketData((int)SocketCommand.READY, "Client đã sẵn sàng", new Point()));
                Listen();
                MessageBox.Show("Đã kết nối đến server!", "Client", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Không thể kết nối đến server!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            txbIP.Text = socket.GetLocalIPv4(NetworkInterfaceType.Wireless80211);

            if (string.IsNullOrEmpty(txbIP.Text))
            {
                txbIP.Text = socket.GetLocalIPv4(NetworkInterfaceType.Ethernet);
            }
        }
        void Listen()
        {
            Thread listenThread = new Thread(() =>
            {
                try
                {
                    SocketData data = (SocketData)socket.Receive();
                    ProcessData(data);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi mạng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            });
            listenThread.IsBackground = true;
            listenThread.Start();
        }
        private void ProcessData(SocketData data)
        {
            switch (data.Command)
            {
                case (int)SocketCommand.NOTIFY:
                    MessageBox.Show(data.Message);
                    break;
                case (int)SocketCommand.NEW_GAME:
                    this.Invoke((MethodInvoker)(() =>
                    {
                        NewGame();
                        pnlChessBoard.Enabled = false;
                    }));
                    break;
                case (int)SocketCommand.SEND_POINT:
                    this.Invoke((MethodInvoker)(() =>
                    {
                        prcbCoolDown.Value = 0;
                        pnlChessBoard.Enabled = true;
                        tmCoolDown.Start();
                        ChessBoard.OtherPlayerMark(data.Point);
                        undoToolStripMenuItem.Enabled = true;
                        SetChatEnabled(true); // Tắt chat khi đối thủ đánh
                    }));
                    break;
                case (int)SocketCommand.UNDO:
                    Undo();
                    prcbCoolDown.Value = 0;
                    break;
                case (int)SocketCommand.END_GAME:
                    MessageBox.Show("Đã 5 con trên 1 hàng");
                    SetChatEnabled(true); // Kích hoạt chat khi kết thúc
                    break;
                case (int)SocketCommand.TIME_OUT:
                    MessageBox.Show("Hết giờ");
                    SetChatEnabled(true); // Kích hoạt chat khi hết giờ
                    break;
                case (int)SocketCommand.QUIT:
                    tmCoolDown.Stop();
                    MessageBox.Show("Người chơi đã thoát");
                    SetChatEnabled(true); // Kích hoạt chat khi đối thủ thoát
                    break;
                case (int)SocketCommand.READY:
                    MessageBox.Show(data.Message);
                    if (socket.isServer)
                    {
                        socket.Send(new SocketData((int)SocketCommand.READY, "Server đã sẵn sàng", new Point()));
                        pnlChessBoard.Enabled = true;
                        SetChatEnabled(true); // Kích hoạt chat khi sẵn sàng
                    }
                    else
                    {
                        pnlChessBoard.Enabled = false;
                        SetChatEnabled(true); // Kích hoạt chat khi sẵn sàng
                    }
                    break;
                case (int)SocketCommand.CHAT:
                    this.Invoke((MethodInvoker)(() =>
                    {
                        chatHistory += data.Message;
                        richTextBoxChat.Text = chatHistory;
                    }));
                    break;
                default:
                    break;
            }

            Listen();
        }
        #endregion

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void pctbMark_Click(object sender, EventArgs e)
        {

        }
    }
    
}
