using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoAnMonHocNT106
{
    public class FirebaseHelper
    {
        private static readonly string FirebaseURL = "https://nt106-7c9fe-default-rtdb.firebaseio.com/";
        private static FirebaseClient firebase = new FirebaseClient(FirebaseURL);

        // ===== USER ACCOUNT =====

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
                    LastOnline = DateTime.MinValue
                });
        }

        public static async Task<User> GetUserByUsername(string username)
        {
            try
            {
                var user = await firebase
                    .Child("Users")
                    .Child(username)
                    .OnceSingleAsync<User>();

                return user;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static async Task<string> GetEmailByUsername(string username)
        {
            var user = await GetUserByUsername(username);
            return user?.Email;
        }

        // Cập nhật trạng thái online/offline
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

        // Lấy danh sách user online
        public static async Task<List<User>> GetOnlineUsers()
        {
            var allUsers = await firebase.Child("Users").OnceAsync<User>();
            return allUsers
                .Where(u => u.Object.IsOnline)
                .Select(u => u.Object)
                .ToList();
        }


        // ===== CHAT =====

        // Lưu message chat riêng giữa 2 user (chatId = user1_user2 hoặc user2_user1 để tránh duplicate)
        public static async Task SaveChatMessage(string user1, string user2, ChatMessage message)
        {
            string chatId = GenerateChatId(user1, user2);
            await firebase
                .Child("Chats")
                .Child(chatId)
                .PostAsync(message);
        }

        // Lấy lịch sử chat giữa 2 user
        public static async Task<List<ChatMessage>> GetChatHistory(string user1, string user2)
        {
            string chatId = GenerateChatId(user1, user2);
            var messages = await firebase
                .Child("Chats")
                .Child(chatId)
                .OrderBy("Time")
                .OnceAsync<ChatMessage>();

            return messages.Select(m => m.Object).OrderBy(m => m.Time).ToList();
        }

        private static string GenerateChatId(string user1, string user2)
        {
            var arr = new[] { user1.ToLower(), user2.ToLower() };
            Array.Sort(arr);
            return $"{arr[0]}_{arr[1]}";
        }


        // ===== GAME RESULT =====

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

        public static async Task<(int Wins, int Losses, int Timeouts)> GetStats(string playerName)
        {
            var history = await GetGameHistory(playerName);
            int wins = history.Count(r => r.Result == "Win");
            int losses = history.Count(r => r.Result == "Lose");
            int timeouts = history.Count(r => r.Result == "Timeout");
            return (wins, losses, timeouts);
        }
    }

    // ===== MODEL =====

    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }  
        public string Email { get; set; }
        public bool IsOnline { get; set; }
        public DateTime LastOnline { get; set; }
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
        public string Result { get; set; } // "Win", "Lose", "Timeout"
        public DateTime Time { get; set; }
    }
}
