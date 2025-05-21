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
                    Email = email
                });
        }

        public static async Task<string> GetEmailByUsername(string username)
        {
            try
            {
                var user = await firebase
                    .Child("Users")
                    .Child(username)
                    .OnceSingleAsync<User>();

                return user?.Email;
            }
            catch (Exception)
            {
                return null;
            }
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

    // Lớp người dùng
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }

    // Lớp kết quả game
    public class GameResult
    {
        public string PlayerName { get; set; }
        public string Result { get; set; } // "Win", "Lose", "Timeout"
        public DateTime Time { get; set; }
    }
}
