using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DoAnMonHocNT106
{
    public class SocketManager
    {
        private TcpListener tcpListener;
        private TcpClient tcpClient;
        private NetworkStream stream;
        private CancellationTokenSource cts;

        // Sự kiện nhận message
        public event Action<string> OnMessageReceived;

        // Bắt đầu server lắng nghe kết nối (host game)
        public async Task StartServer(int port)
        {
            cts = new CancellationTokenSource();
            tcpListener = new TcpListener(IPAddress.Any, port);
            tcpListener.Start();

            // Đợi client kết nối
            tcpClient = await tcpListener.AcceptTcpClientAsync();
            stream = tcpClient.GetStream();

            _ = Task.Run(() => ReceiveLoop(cts.Token));
        }

        // Kết nối đến server (join game)
        public async Task ConnectToServer(string ip, int port)
        {
            cts = new CancellationTokenSource();
            tcpClient = new TcpClient();
            await tcpClient.ConnectAsync(IPAddress.Parse(ip), port);
            stream = tcpClient.GetStream();

            _ = Task.Run(() => ReceiveLoop(cts.Token));
        }

        // Gửi message qua socket (chuỗi)
        public async Task SendMessage(string message)
        {
            if (stream == null) return;

            byte[] data = Encoding.UTF8.GetBytes(message + "\n");
            await stream.WriteAsync(data, 0, data.Length);
        }

        // Nhận message liên tục
        private async Task ReceiveLoop(CancellationToken token)
        {
            var buffer = new byte[1024];
            var sb = new StringBuilder();

            while (!token.IsCancellationRequested)
            {
                try
                {
                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length, token);
                    if (bytesRead == 0)
                    {
                        // Kết nối đóng
                        break;
                    }

                    string received = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    sb.Append(received);

                    // Kiểm tra nếu có dấu \n thì phân tách thành message
                    string s = sb.ToString();
                    int newlineIndex;
                    while ((newlineIndex = s.IndexOf('\n')) >= 0)
                    {
                        string msg = s.Substring(0, newlineIndex).Trim();
                        s = s.Substring(newlineIndex + 1);
                        OnMessageReceived?.Invoke(msg);
                    }
                    sb.Clear();
                    sb.Append(s);
                }
                catch (Exception)
                {
                    break;
                }
            }
        }
        public void Stop()
        {
            cts?.Cancel();
            stream?.Close();
            tcpClient?.Close();
            tcpListener?.Stop();
        }
    }
}
