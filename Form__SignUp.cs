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
        private string apiKey = "AIzaSyAtbgnNBlNDVe4tlvlXFf8lRVCeus8Dong";
        private FirebaseAuthProvider auth;
        private bool _navigatingToLogin = false;

        public SignUp()
        {
            InitializeComponent();
            auth = new FirebaseAuthProvider(new FirebaseConfig("AIzaSy..."));
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

        private async void button1_Click(object sender, EventArgs e)
        {
            MusicPlayer.PlayClickSound();
            string Username = username.Text;
            string email = mail.Text;
            string password = pw.Text;

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
        private void show_CheckedChanged(object sender, EventArgs e)
        {
            pw.UseSystemPasswordChar = !show.Checked;
            repw.UseSystemPasswordChar = !show.Checked;
        }
        private async void gm_Click(object sender, EventArgs e)
        {
            MusicPlayer.PlayClickSound();
            try
            {
                string clientId = "150464310449-sbdgtn4a2vo3n1dbq51cc1o9ept5tokl.apps.googleusercontent.com";
                string clientSecret = "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";
                string firebaseApiKey = "AIzaSyAtbgnNBlNDVe4tlvlXFf8lRVCeus8Dong";
                string firebaseDbUrl = "https://nt106-7c9fe-default-rtdb.firebaseio.com/";

                // Step 1: Google OAuth
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

                // Kiểm tra ID token
                string idToken = credential.Token.IdToken;
                if (string.IsNullOrEmpty(idToken))
                {
                    MessageBox.Show("Không lấy được ID Token từ Google.");
                    return;
                }

                // Step 2: Firebase Auth with Google Token
                using (var client = new HttpClient())
                {
                    var requestUri = $"https://identitytoolkit.googleapis.com/v1/accounts:signInWithIdp?key={firebaseApiKey}";

                    var content = new
                    {
                        postBody = $"id_token={idToken}&providerId=google.com",
                        requestUri = "http://localhost", // Đảm bảo domain này có trong Firebase Auth > Sign-in method > Authorized domains
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

                    if (string.IsNullOrEmpty(firebaseIdToken) || string.IsNullOrEmpty(localId))
                    {
                        MessageBox.Show("Thông tin người dùng từ Firebase không đầy đủ.");
                        return;
                    }

                    // Step 3: Check if user exists in Realtime Database
                    bool userExists = await CheckIfUserExists(firebaseDbUrl, localId, firebaseIdToken);

                    if (!userExists)
                    {
                        await CreateUserInDatabase(firebaseDbUrl, localId, firebaseIdToken, displayName, email);
                    }

                    MessageBox.Show($"Đăng nhập Google thành công!\nChào {displayName ?? "bạn"}.");

                    // Mở Lobby
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
        private async Task CreateUserInDatabase(string firebaseDbUrl, string userId, string idToken, string displayName, string email)
        {
            using (var client = new HttpClient())
            {
                var requestUri = $"{firebaseDbUrl}/users/{userId}.json?auth={idToken}";

                var newUser = new
                {
                    displayName,
                    email,
                    role = "user", // Optional: default role
                    createdAt = DateTime.UtcNow.ToString("o")
                };

                await client.PutAsJsonAsync(requestUri, newUser);
            }
        }
    }
}