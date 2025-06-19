// SignUp.cs
// Xử lý logic cho form đăng ký người dùng:
// Bao gồm các chức năng đăng ký bằng Email/Password, Google OAuth,
// kiểm tra tài khoản tồn tại, lưu dữ liệu người dùng vào Firebase Realtime Database,
// và chuyển hướng sang màn hình Lobby sau khi đăng ký thành công.

using System;
using System.Windows.Forms;
using Firebase.Auth;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Util.Store;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http.Json;

namespace DoAnMonHocNT106
{
    public partial class SignUp : Form
    {
        // Khóa API Firebase để xác thực người dùng
        private string apiKey = "AIzaSyAtbgnNBlNDVe4tlvlXFf8lRVCeus8Dong";

        // Đối tượng cung cấp dịch vụ xác thực từ Firebase
        private FirebaseAuthProvider auth;

        // Cờ đánh dấu khi đang chuyển hướng sang form Login, dùng để không mở lại Login khi đóng form
        private bool _navigatingToLogin = false;

        public SignUp()
        {
            InitializeComponent();

            // Khởi tạo provider xác thực từ Firebase
            auth = new FirebaseAuthProvider(new FirebaseConfig("AIzaSy..."));

            // Gán sự kiện khi đóng form
            this.FormClosing += SignUp_FormClosing;
        }

        private void SignUp_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Chỉ show Login khi người dùng bấm X, không phải khi SignUp thành công
            if (!_navigatingToLogin && e.CloseReason == CloseReason.UserClosing)
            {
                new Login().Show();
            }
        }

        // button1_Click (Sự kiện khi nhấn nút "Confirm" để đăng ký)
        private async void button1_Click(object sender, EventArgs e)
        {
            MusicPlayer.PlayClickSound();

            string Username = username.Text;
            string email = mail.Text;
            string password = pw.Text;

            // Kiểm tra người dùng đã nhập đầy đủ thông tin chưa
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
                return;
            }

            try
            {
                var authProvider = new FirebaseAuthProvider(new FirebaseConfig(apiKey));

                // Kiểm tra email đã tồn tại chưa
                var auth = await authProvider.CreateUserWithEmailAndPasswordAsync(email, password);

                // Lưu thông tin user vào Firebase Realtime Database (Không lưu mật khẩu!)
                await FirebaseHelper.AddUser(Username, password, email);

                MessageBox.Show("Đăng ký thành công!");

                _navigatingToLogin = true;    // Đánh dấu sắp chuyển sang Login

                Login form = new Login();
                form.Show();
                this.Close();
            }
            catch (FirebaseAuthException ex)
            {
                // Xử lý lỗi nếu email đã được đăng ký
                if (ex.ResponseData.Contains("EMAIL_EXISTS"))
                {
                    MessageBox.Show("Email đã được đăng ký!");
                }
                else
                {
                    MessageBox.Show("Lỗi đăng ký: " + ex.Message);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi đăng ký: " + ex.Message);
            }
        }

        // show_CheckedChanged (Hiển thị hoặc ẩn mật khẩu khi checkbox được thay đổi)
        private void show_CheckedChanged(object sender, EventArgs e)
        {
            pw.UseSystemPasswordChar = !show.Checked;
            repw.UseSystemPasswordChar = !show.Checked;
        }

        // gm_Click (Đăng ký/đăng nhập bằng tài khoản Google)
        private async void gm_Click(object sender, EventArgs e)
        {
            MusicPlayer.PlayClickSound();

            try
            {
                // Cấu hình client ID và secret cho Google OAuth
                string clientId = "150464310449-sbdgtn4a2vo3n1dbq51cc1o9ept5tokl.apps.googleusercontent.com";
                string clientSecret = "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";
                string firebaseApiKey = "AIzaSyAtbgnNBlNDVe4tlvlXFf8lRVCeus8Dong";
                string firebaseDbUrl = "https://nt106-7c9fe-default-rtdb.firebaseio.com/";

                // Bước 1: Đăng nhập Google bằng OAuth
                var secrets = new ClientSecrets
                {
                    ClientId = clientId,
                    ClientSecret = clientSecret
                };

                var credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    secrets,
                    new[] { "email", "profile", "openid" },
                    "user",
                    CancellationToken.None,
                    new FileDataStore("GoogleTokenStore")
                );

                // Kiểm tra ID token trả về từ Google
                string idToken = credential.Token.IdToken;
                if (string.IsNullOrEmpty(idToken))
                {
                    MessageBox.Show("Không lấy được ID Token từ Google.");
                    return;
                }

                // Bước 2: Gửi ID Token lên Firebase để xác thực
                using (var client = new HttpClient())
                {
                    var requestUri = $"https://identitytoolkit.googleapis.com/v1/accounts:signInWithIdp?key={firebaseApiKey}";

                    var content = new
                    {
                        postBody = $"id_token={idToken}&providerId=google.com",
                        requestUri = "http://localhost", // Phải có trong danh sách domain được cho phép trong Firebase
                        returnIdpCredential = true,
                        returnSecureToken = true
                    };

                    var response = await client.PostAsJsonAsync(requestUri, content);
                    var responseBody = await response.Content.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Lỗi Firebase: " + responseBody);
                        return;
                    }

                    var json = JObject.Parse(responseBody);
                    string firebaseIdToken = json["idToken"]?.ToString();
                    string email = json["email"]?.ToString();
                    string displayName = json["displayName"]?.ToString();
                    string localId = json["localId"]?.ToString();

                    // Kiểm tra thông tin trả về có đầy đủ không
                    if (string.IsNullOrEmpty(firebaseIdToken) || string.IsNullOrEmpty(localId))
                    {
                        MessageBox.Show("Thông tin người dùng từ Firebase không đầy đủ.");
                        return;
                    }

                    // Bước 3: Kiểm tra người dùng đã tồn tại trong Realtime Database chưa
                    bool userExists = await CheckIfUserExists(firebaseDbUrl, localId, firebaseIdToken);

                    if (!userExists)
                    {
                        // Nếu chưa có, tạo người dùng mới trong Database
                        await CreateUserInDatabase(firebaseDbUrl, localId, firebaseIdToken, displayName, email);
                    }

                    MessageBox.Show($"Đăng nhập Google thành công!\nChào {displayName ?? "bạn"}.");

                    // Mở màn hình Lobby
                    FormLobby lobby = new FormLobby(displayName);
                    lobby.Show();
                    this.Hide();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}");
            }
        }

        // CheckIfUserExists (Kiểm tra người dùng có tồn tại trong Firebase Realtime Database hay chưa)
        private async Task<bool> CheckIfUserExists(string firebaseDbUrl, string userId, string idToken)
        {
            using (var client = new HttpClient())
            {
                var requestUri = $"{firebaseDbUrl}/users/{userId}.json?auth={idToken}";

                var response = await client.GetAsync(requestUri);
                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    return responseBody != "null";
                }
                return false;
            }
        }

        // CreateUserInDatabase (Tạo mới một bản ghi người dùng trong Firebase Realtime Database)
        private async Task CreateUserInDatabase(string firebaseDbUrl, string userId, string idToken, string displayName, string email)
        {
            using (var client = new HttpClient())
            {
                var requestUri = $"{firebaseDbUrl}/users/{userId}.json?auth={idToken}";

                var newUser = new
                {
                    displayName,
                    email,
                    role = "user", // Gán quyền mặc định là "user"
                    createdAt = DateTime.UtcNow.ToString("o") // Lưu thời gian tạo
                };

                await client.PutAsJsonAsync(requestUri, newUser);
            }
        }
    }
}
