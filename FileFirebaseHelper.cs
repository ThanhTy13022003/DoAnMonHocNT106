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
                    LastOnline = DateTime.MinValue
                });
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
        }
        public static FirebaseClient GetFirebaseClient()
        {
            return firebase;
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

    }

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
