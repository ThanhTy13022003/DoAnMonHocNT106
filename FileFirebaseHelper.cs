using Firebase.Database;
using Firebase.Database.Query;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reactive.Linq;
using System.Reactive.Disposables;

namespace DoAnMonHocNT106
{
    public class FirebaseHelper
    {
        private static readonly string FirebaseURL = "https://nt106-7c9fe-default-rtdb.firebaseio.com/";
        private static readonly FirebaseClient firebase = new FirebaseClient(FirebaseURL);

        public static async Task AddUser(string username, string password, string email)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Username, password, và email không được để trống.");

            try
            {
                await firebase
                    .Child("Users")
                    .Child(username)
                    .PutAsync(new User
                    {
                        Username = username,
                        Password = password,
                        Email = email,
                        IsOnline = false,
                        LastOnline = DateTime.MinValue,
                        Wins = 0,
                        Losses = 0,
                        Draws = 0,
                        WinRate = 0.0
                    });
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi thêm người dùng: {ex.Message}", ex);
            }
        }

        public static async Task UpdateUserStats(string username, string result)
        {
            var user = await GetUserByUsername(username);
            if (user == null)
                throw new InvalidOperationException($"Người dùng '{username}' không tồn tại.");

            try
            {
                if (result == "Win")
                    user.Wins++;
                else if (result == "Lose")
                    user.Losses++;
                else if (result == "Draw")
                    user.Draws++;

                user.WinRate = (user.Wins + user.Losses + user.Draws) > 0
                    ? Math.Round((double)user.Wins / (user.Wins + user.Losses + user.Draws) * 100, 2)
                    : 0.0;

                await firebase.Child("Users").Child(username).PutAsync(user);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi cập nhật thống kê: {ex.Message}", ex);
            }
        }

        public static async Task UpdateUserCredentials(string oldUsername, string newUsername, string newPassword)
        {
            if (string.IsNullOrWhiteSpace(oldUsername) || string.IsNullOrWhiteSpace(newUsername) || string.IsNullOrWhiteSpace(newPassword))
                throw new ArgumentException("Username và password không được để trống.");

            var user = await GetUserByUsername(oldUsername);
            if (user == null)
                throw new InvalidOperationException($"Người dùng '{oldUsername}' không tồn tại.");

            try
            {
                user.Username = newUsername;
                user.Password = newPassword;

                await firebase.Child("Users").Child(newUsername).PutAsync(user);
                if (oldUsername != newUsername)
                    await firebase.Child("Users").Child(oldUsername).DeleteAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi cập nhật thông tin người dùng: {ex.Message}", ex);
            }
        }

        public static async Task<User> GetUserByUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Username không được để trống.");

            try
            {
                return await firebase
                    .Child("Users")
                    .Child(username)
                    .OnceSingleAsync<User>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi lấy thông tin người dùng '{username}': {ex.Message}");
                return null;
            }
        }

        public static async Task<string> GetEmailByUsername(string username)
        {
            var user = await GetUserByUsername(username);
            return user?.Email;
        }

        public static async Task SetUserOnlineStatus(string username, bool isOnline)
        {
            var user = await GetUserByUsername(username);
            if (user == null) return;

            try
            {
                user.IsOnline = isOnline;
                user.LastOnline = DateTime.Now;
                await firebase.Child("Users").Child(username).PutAsync(user);
                await firebase.Child("Users").Child(username).Child("online").PutAsync(isOnline);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi cập nhật trạng thái online cho '{username}': {ex.Message}");
            }
        }

        public static async Task<List<User>> GetOnlineUsers()
        {
            try
            {
                var allUsers = await firebase.Child("Users").OnceAsync<User>();
                return allUsers.Where(u => u.Object.IsOnline).Select(u => u.Object).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi lấy danh sách người dùng online: {ex.Message}");
                return new List<User>();
            }
        }

        public static async Task<List<User>> GetAllUsers()
        {
            try
            {
                var allUsers = await firebase.Child("Users").OnceAsync<User>();
                return allUsers.Select(x => x.Object).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi lấy danh sách người dùng: {ex.Message}");
                return new List<User>();
            }
        }

        public static async Task SendChatMessage(string username, string message)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(message))
                throw new ArgumentException("Username và tin nhắn không được để trống.");

            try
            {
                var chatMsg = new ChatMessage
                {
                    FromUser = username,
                    ToUser = "all",
                    Message = message,
                    Time = DateTime.UtcNow
                };

                await firebase.Child("PublicChat").PostAsync(chatMsg);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi gửi tin nhắn: {ex.Message}", ex);
            }
        }

        public static async Task<List<ChatMessage>> GetPublicChatMessages()
        {
            try
            {
                var msgs = await firebase.Child("PublicChat").OrderByKey().OnceAsync<ChatMessage>();
                return msgs.Select(m => m.Object).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi lấy tin nhắn công khai: {ex.Message}");
                return new List<ChatMessage>();
            }
        }

        public static async Task SaveGameResult(string playerName, string result)
        {
            try
            {
                var gameResult = new GameResult
                {
                    PlayerName = playerName,
                    Result = result,
                    Time = DateTime.Now
                };

                await firebase.Child("GameResults").PostAsync(gameResult);
                await UpdateUserStats(playerName, result);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lưu kết quả trò chơi: {ex.Message}", ex);
            }
        }

        public static async Task<List<GameResult>> GetGameHistory(string playerName)
        {
            try
            {
                var allResults = await firebase
                    .Child("GameResults")
                    .OrderBy("Time")
                    .OnceAsync<GameResult>();

                return allResults
                    .Select(item => item.Object)
                    .Where(gr => gr.PlayerName == playerName)
                    .OrderByDescending(gr => gr.Time)
                    .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi lấy lịch sử trò chơi: {ex.Message}");
                return new List<GameResult>();
            }
        }

        public static async Task EnsureUserStatsFields()
        {
            try
            {
                var users = await GetAllUsers();
                foreach (var user in users)
                {
                    if (user.Wins == 0 && user.Losses == 0 && user.Draws == 0)
                    {
                        user.Wins = 0;
                        user.Losses = 0;
                        user.Draws = 0;
                        user.WinRate = 0.0;
                        await firebase.Child("Users").Child(user.Username).PutAsync(user);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi đảm bảo các trường thống kê: {ex.Message}");
            }
        }

        public static async Task SavePvPGameResult(string roomId, string playerX, string playerO, string result)
        {
            if (string.IsNullOrWhiteSpace(roomId) || string.IsNullOrWhiteSpace(playerX) || string.IsNullOrWhiteSpace(playerO))
                throw new ArgumentException("Thông tin phòng và người chơi không được để trống.");

            try
            {
                var match = new
                {
                    roomId = roomId,
                    playerX = playerX,
                    playerO = playerO,
                    result = result,
                    time = DateTime.UtcNow.ToString("o")
                };

                string resX = result == "X_Win" ? "Win"
                            : result == "O_Win" ? "Lose"
                            : result == "Draw" ? "Draw" : "Lose";
                string resO = result == "O_Win" ? "Win"
                            : result == "X_Win" ? "Lose"
                            : result == "Draw" ? "Draw" : "Lose";

                var recordX = new { opponent = playerO, result = resX, time = match.time };
                var recordO = new { opponent = playerX, result = resO, time = match.time };

                await Task.WhenAll(
                    firebase.Child("MatchHistory").PostAsync(match),
                    firebase.Child("Users").Child(playerX).Child("MatchHistory").PostAsync(recordX),
                    firebase.Child("Users").Child(playerO).Child("MatchHistory").PostAsync(recordO),
                    UpdateUserStats(playerX, resX),
                    UpdateUserStats(playerO, resO)
                );
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lưu kết quả trận đấu PvP: {ex.Message}", ex);
            }
        }

        public static async Task<(int Wins, int Losses, int Timeouts)> GetStats(string playerName)
        {
            try
            {
                var history = await GetGameHistory(playerName);
                int wins = history.Count(r => r.Result == "Win");
                int losses = history.Count(r => r.Result == "Lose");
                int timeouts = history.Count(r => r.Result == "Timeout");
                return (wins, losses, timeouts);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi lấy thống kê: {ex.Message}");
                return (0, 0, 0);
            }
        }

        public static string CurrentUsername { get; set; } = "Guest";

        public static async Task<List<(string Username, double WinRate, int TotalGames, int Wins, int Losses, int Draws)>> GetLeaderboard()
        {
            try
            {
                var allUsers = await GetAllUsers();
                var leaderboard = new List<(string Username, double WinRate, int TotalGames, int Wins, int Losses, int Draws)>();

                foreach (var user in allUsers)
                {
                    int totalGames = user.Wins + user.Losses + user.Draws;
                    leaderboard.Add((user.Username, user.WinRate, totalGames, user.Wins, user.Losses, user.Draws));
                }

                return leaderboard
                    .OrderByDescending(x => x.WinRate)
                    .ThenByDescending(x => x.TotalGames)
                    .Take(50)
                    .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi lấy bảng xếp hạng: {ex.Message}");
                return new List<(string Username, double WinRate, int TotalGames, int Wins, int Losses, int Draws)>();
            }
        }

        public static IDisposable SubscribeUserChanges(string username, Action<User> onUserChanged)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Username không được để trống.");

            return firebase
                .Child("Users")
                .Child(username)
                .AsObservable<User>()
                .Subscribe(ev =>
                {
                    if (ev.Object != null)
                        onUserChanged?.Invoke(ev.Object);
                }, ex =>
                {
                    Console.WriteLine($"Lỗi khi lắng nghe thay đổi người dùng: {ex.Message}");
                });
        }
    }

    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public bool IsOnline { get; set; }
        public DateTime LastOnline { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public int Draws { get; set; }
        public double WinRate { get; set; }
    }

    public class ChatMessage
    {
        public string Id { get; set; }
        public string FromUser { get; set; }
        public string ToUser { get; set; }
        public string Message { get; set; }
        public DateTime Time { get; set; }
    }

    public class GameResult
    {
        public string PlayerName { get; set; }
        public string Result { get; set; }
        public DateTime Time { get; set; }
    }

    public class Invite
    {
        public string from { get; set; }
        public string to { get; set; }
        public string roomId { get; set; }
        public string timestamp { get; set; }
    }

    public class Move
    {
        public int row { get; set; }
        public int col { get; set; }
        public string user { get; set; }
        public string symbol { get; set; }
        public string timestamp { get; set; }
    }
}