using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Threading.Tasks;
using System.Linq;

namespace DoAnMonHocNT106
{
    public class FirebaseHelper
    {
        // Tạo FirebaseClient để kết nối đến Realtime Database thông qua URL của database.
        private static readonly string FirebaseURL = "https://nt106-7c9fe-default-rtdb.firebaseio.com/";
        private static FirebaseClient firebase = new FirebaseClient(FirebaseURL);

        // Hàm thêm người dùng vào Firebase
        public static async Task AddUser(string username, string password, string email)
        {
            await firebase
                .Child("Users")
                .Child(username) // Username làm key
                .PutAsync(new User
                {
                    Username = username,
                    Password = password,
                    Email = email
                });
        }

        // Hàm lấy Email từ Username
        public static async Task<string> GetEmailByUsername(string username)
        {
            try
            {
                var user = await firebase
                    .Child("Users")
                    .Child(username)
                    .OnceSingleAsync<User>();

                return user?.Email; // Trả về email nếu tìm thấy
            }
            catch (Exception)
            {
                return null; // Trả về null nếu không tìm thấy
            }
        }
    }

    // Lớp User để lưu dữ liệu
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}
