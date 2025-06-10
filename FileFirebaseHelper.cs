using Firebase.Database;
using Firebase.Database.Query;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoAnMonHocNT106
{
    public class FirebaseHelper
    {
        private static readonly string FirebaseURL = "https://nt106-7c9fe-default-rtdb.firebaseio.com/";
        public static FirebaseClient firebase = new FirebaseClient(FirebaseURL);

        public static async Task AddUser(string username, string password, string email)
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
                    Wins = 0, // Khởi tạo Wins
                    Losses = 0, // Khởi tạo Losses
                    Draws = 0 // Khởi tạo Draws
                });
        }

        public static async Task UpdateUserStats(string username, string result)
        {
            var user = await GetUserByUsername(username);
            if (user != null)
            {
                if (result == "Win")
                    user.Wins++;
                else if (result == "Lose")
                    user.Losses++;
                else if (result == "Draw")
                    user.Draws++;

                await firebase.Child("Users").Child(username).PutAsync(user);
            }
        }

        public static async Task UpdateUserCredentials(string oldUsername, string newUsername, string newPassword)
        {
            // 1. Lấy user cũ
            var user = await GetUserByUsername(oldUsername);
            if (user == null)
                throw new InvalidOperationException($"User '{oldUsername}' không tồn tại.");

            // 2. Cập nhật thông tin
            user.Username = newUsername;
            user.Password = newPassword;

            // 3. Ghi lên key mới
            await firebase
                .Child("Users")
                .Child(newUsername)
                .PutAsync(user);

            // 4. Xóa key cũ
            await firebase
                .Child("Users")
                .Child(oldUsername)
                .DeleteAsync();
        }


        public static async Task<User> GetUserByUsername(string username)
        {
            try
            {
                return await firebase
                    .Child("Users")
                    .Child(username)
                    .OnceSingleAsync<User>();
            }
            catch
            {
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
            if (user != null)
            {
                user.IsOnline = isOnline;
                user.LastOnline = DateTime.Now;
                await firebase.Child("Users").Child(username).PutAsync(user);
            }
        }

        public static async Task<List<User>> GetOnlineUsers()
        {
            var allUsers = await firebase.Child("Users").OnceAsync<User>();
            return allUsers.Where(u => u.Object.IsOnline).Select(u => u.Object).ToList();
        }

        public static async Task<List<User>> GetAllUsers()
        {
            var allUsers = await firebase.Child("Users").OnceAsync<User>();
            return allUsers.Select(x => x.Object).ToList();
        }

        public static async Task<string> GetLoggedInUsername()
        {
            return await Task.FromResult(CurrentUsername);
        }

        public static async Task SendChatMessage(string username, string message)
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

        public static async Task<List<ChatMessage>> GetPublicChatMessages()
        {
            var msgs = await firebase.Child("PublicChat").OrderByKey().OnceAsync<ChatMessage>();
            return msgs.Select(m => m.Object).ToList();
        }

        public static async Task SaveGameResult(string playerName, string result)
        {
            var gameResult = new GameResult
            {
                PlayerName = playerName,
                Result = result,
                Time = DateTime.Now
            };

            await firebase
                .Child("GameResults")
                .PostAsync(gameResult);

            // Cập nhật thống kê người dùng
            await UpdateUserStats(playerName, result);
        }

        public static async Task<List<GameResult>> GetGameHistory(string playerName)
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

        public static async Task EnsureUserStatsFields()
        {
            var users = await GetAllUsers();
            foreach (var user in users)
            {
                if (user.Wins == 0 && user.Losses == 0 && user.Draws == 0) // Kiểm tra nếu các trường chưa được khởi tạo
                {
                    user.Wins = 0;
                    user.Losses = 0;
                    user.Draws = 0;
                    await firebase.Child("Users").Child(user.Username).PutAsync(user);
                }
            }
        }

        public static async Task SavePvPGameResult(string roomId, string playerX, string playerO, string result)
        {
            var match = new
            {
                roomId = roomId,
                playerX = playerX,
                playerO = playerO,
                result = result,
                time = DateTime.UtcNow.ToString("o")
            };

            await firebase.Child("MatchHistory").PostAsync(match);

            // Cập nhật thống kê người dùng
            if (result == "X_Win")
            {
                await UpdateUserStats(playerX, "Win");
                await UpdateUserStats(playerO, "Lose");
            }
            else if (result == "O_Win")
            {
                await UpdateUserStats(playerX, "Lose");
                await UpdateUserStats(playerO, "Win");
            }
            else if (result == "Draw")
            {
                await UpdateUserStats(playerX, "Draw");
                await UpdateUserStats(playerO, "Draw");
            }
        }

        public static async Task<(int Wins, int Losses, int Timeouts)> GetStats(string playerName)
        {
            var history = await GetGameHistory(playerName);
            int wins = history.Count(r => r.Result == "Win");
            int losses = history.Count(r => r.Result == "Lose");
            int timeouts = history.Count(r => r.Result == "Timeout");
            return (wins, losses, timeouts);
        }
        public static string CurrentUsername { get; set; } = "Guest";

        public static async Task<List<(string Username, double WinRate, int TotalGames, int Wins, int Losses, int Draws)>> GetLeaderboard()
        {
            var allUsers = await GetAllUsers();
            var leaderboard = new List<(string Username, double WinRate, int TotalGames, int Wins, int Losses, int Draws)>();

            foreach (var user in allUsers)
            {
                int totalGames = user.Wins + user.Losses + user.Draws;
                double winRate = totalGames > 0 ? Math.Round((double)user.Wins / totalGames * 100, 2) : 0.0;

                leaderboard.Add((user.Username, winRate, totalGames, user.Wins, user.Losses, user.Draws));
            }

            // Sắp xếp theo tỉ lệ thắng giảm dần, nếu tỉ lệ thắng bằng nhau thì ưu tiên người chơi nhiều trận hơn
            return leaderboard
                .OrderByDescending(x => x.WinRate)
                .ThenByDescending(x => x.TotalGames)
                .Take(50) // Giới hạn top 50 người chơi
                .ToList();
        }
    }
        public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public bool IsOnline { get; set; }
        public DateTime LastOnline { get; set; }
        public int Wins { get; set; } // Thêm trường Wins
        public int Losses { get; set; } // Thêm trường Losses
        public int Draws { get; set; } // Thêm trường Draws
    }

    public class ChatMessage
    {
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
    public class UserInfo
    {
        public string Email { get; set; }
        public string DisplayName { get; set; }
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
