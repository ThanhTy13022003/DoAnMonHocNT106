using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Dropbox.Api;
using Dropbox.Api.Files;
using Firebase.Database;
using Firebase.Database.Query;
using System.Net.Http;
using System.Threading.Tasks;
using Dropbox.Api.Sharing;
using System.Collections.Generic;
using System.Media;

namespace GameCaro_SettingInforPlayer
{
    public partial class Form : System.Windows.Forms.Form
    {
        // Replace these with your actual app key, app secret, and refresh token
        private const string AppKey = "youjg0q1oiqp7mg";
        private const string AppSecret = "fuvb1xb9wjbmp2b";
        private const string RefreshToken = "IXLCHAQQ4h0AAAAAAAAAAZ5IKZbHn4pQG4CjbuCwcgtvO_zZ5ryZLsPvLJctXaL8";

        private const string FirebaseDatabaseUrl = "https://nt106-7c9fe-default-rtdb.firebaseio.com/";
        private FirebaseClient firebaseClient;
        private bool isEditing = false;
        private System.Timers.Timer processCheckTimer;

        private static readonly HttpClient httpClient = new HttpClient();

        public Form()
        {
            InitializeComponent();
            this.btnChangeAvatar.Click += new System.EventHandler(this.btnChangeAvatar_Click);
            this.btnUpdateInfo.Click += new System.EventHandler(this.btnUpdateInfo_Click);
            this.btnChangePassword.Click += new System.EventHandler(this.btnChangePassword_Click);
            InitializeFirebase();

            // Lấy username từ tham số dòng lệnh
            string[] args = Environment.GetCommandLineArgs();
            string username = args.Length > 1 && !string.IsNullOrWhiteSpace(args[1]) ? args[1] : "baongdqu";

            // Tải dữ liệu người chơi với username
            LoadPlayerData(username);

            // Khởi tạo timer kiểm tra tiến trình
            processCheckTimer = new System.Timers.Timer(1000); // Kiểm tra mỗi 1 giây
            processCheckTimer.Elapsed += ProcessCheckTimer_Elapsed;
            processCheckTimer.AutoReset = true;
            processCheckTimer.Start();
        }

        private void ProcessCheckTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                var processes = System.Diagnostics.Process.GetProcessesByName("DoAnMonHocNT106");
                System.Diagnostics.Debug.WriteLine($"Found {processes.Length} process(es) named 'DoAnMonHocNT106'");
                if (processes.Length == 0)
                {
                    System.Diagnostics.Debug.WriteLine("No DoAnMonHocNT106 process found. Stopping timer and closing form.");
                    processCheckTimer.Stop();
                    this.Invoke((MethodInvoker)delegate
                    {
                        this.Close();
                    });
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("DoAnMonHocNT106 process is running.");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error checking process: {ex.Message}");
            }
        }

        private void InitializeFirebase()
        {
            try
            {
                firebaseClient = new FirebaseClient(FirebaseDatabaseUrl);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing Firebase: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task<bool> CheckUsernameExists(string username, string currentPlayerId)
        {
            try
            {
                var users = await firebaseClient
                    .Child("Users")
                    .OnceAsync<Player>();

                foreach (var user in users)
                {
                    // Skip checking the current player's username
                    if (user.Key != currentPlayerId && user.Object.Username == username)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error checking username: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return true; // Return true to prevent update in case of error
            }
        }

        private async void LoadPlayerData(string playerId)
        {
            try
            {
                var playerData = await firebaseClient
                    .Child("Users")
                    .Child(playerId)
                    .OnceSingleAsync<Player>();

                if (playerData != null)
                {
                    int draws = playerData.GamesPlayed - playerData.Wins - playerData.Losses;
                    string winRate = playerData.GamesPlayed > 0
                        ? $"{(playerData.Wins / (double)playerData.GamesPlayed * 100):0.00}%"
                        : "0%";

                    txtPlayerName.Text = playerData.Username ?? "Người chơi";
                    txtWins.Text = playerData.Wins.ToString() ?? "0";
                    txtLosses.Text = playerData.Losses.ToString() ?? "0";
                    txtDraws.Text = draws.ToString();
                    txtWinRate.Text = winRate;
                    txtStatus.Text = playerData.Status ?? "Hoạt động";
                    txtEmail.Text = playerData.Email ?? "player@example.com";
                    txtBirthDate.Text = playerData.DateOfBirth?.Replace("/", "") ?? "01012000";
                    cmbNationality.SelectedItem = playerData.Country ?? "Việt Nam";

                    if (!string.IsNullOrEmpty(playerData.AvatarLink))
                    {
                        await LoadAvatarImage(playerData.AvatarLink);
                    }
                }
                else
                {
                    MessageBox.Show($"Player {playerId} not found in the database.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading player data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task LoadAvatarImage(string avatarUrl)
        {
            try
            {
                string decodedUrl = Uri.UnescapeDataString(avatarUrl);

                if (!Uri.TryCreate(decodedUrl, UriKind.Absolute, out Uri uriResult) ||
                    (uriResult.Scheme != Uri.UriSchemeHttp && uriResult.Scheme != Uri.UriSchemeHttps))
                {
                    MessageBox.Show("The avatar URL is invalid or not an absolute URL.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using (var httpClient = new HttpClient())
                {
                    httpClient.Timeout = TimeSpan.FromSeconds(30);

                    var response = await httpClient.GetAsync(decodedUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        using (var stream = await response.Content.ReadAsStreamAsync())
                        {
                            pictureBoxAvatar.Image?.Dispose();
                            pictureBoxAvatar.Image = Image.FromStream(stream);
                        }
                    }
                    else
                    {
                        MessageBox.Show($"Failed to load avatar image. Status Code: {response.StatusCode}. The URL might have expired or is inaccessible.",
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading avatar image: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnChangeAvatar_Click(object sender, EventArgs e)
        {
            try
            {
                SoundPlayer player = new SoundPlayer("click-button-140881.wav");
                player.Play();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi phát âm thanh: " + ex.Message);
            }

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
                openFileDialog.Title = "Select an Avatar Image";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (var stream = File.Open(openFileDialog.FileName, FileMode.Open, FileAccess.Read))
                        {
                            Image selectedImage = new Bitmap(stream);
                            using (ImageEditForm editForm = new ImageEditForm(selectedImage))
                            {
                                if (editForm.ShowDialog() == DialogResult.OK)
                                {
                                    pictureBoxAvatar.Image?.Dispose();
                                    pictureBoxAvatar.Image = editForm.CroppedImage;

                                    string tempFilePath = Path.Combine(Path.GetTempPath(), "cropped_avatar.png");
                                    editForm.CroppedImage.Save(tempFilePath, System.Drawing.Imaging.ImageFormat.Png);

                                    string avatarUrl = await UploadToDropbox(tempFilePath);
                                    if (!string.IsNullOrEmpty(avatarUrl))
                                    {
                                        await SaveAvatarUrlToFirebase("baongdqu", avatarUrl);
                                        MessageBox.Show("Avatar uploaded to Dropbox and URL saved to Firebase successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }

                                    File.Delete(tempFilePath);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error uploading avatar: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private async Task<DropboxClient> GetDropboxClient()
        {
            try
            {
                var config = new DropboxClientConfig("GameCaro_SettingInforPlayer")
                {
                    HttpClient = httpClient
                };

                // Prepare the request to exchange the refresh token for an access token
                var content = new FormUrlEncodedContent(new[]
                {
            new KeyValuePair<string, string>("grant_type", "refresh_token"),
            new KeyValuePair<string, string>("refresh_token", RefreshToken),
            new KeyValuePair<string, string>("client_id", AppKey),
            new KeyValuePair<string, string>("client_secret", AppSecret)
        });

                var response = await httpClient.PostAsync("https://api.dropboxapi.com/oauth2/token", content);
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Failed to exchange refresh token: {response.StatusCode}, {errorContent}");
                }

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var tokenData = System.Text.Json.JsonSerializer.Deserialize<TokenResponse>(jsonResponse);
                string accessToken = tokenData.AccessToken;

                // Initialize DropboxClient with the access token
                return new DropboxClient(accessToken, config);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing Dropbox client: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        private async Task<string> UploadToDropbox(string filePath)
        {
            try
            {
                using (var dbx = await GetDropboxClient())
                {
                    if (dbx == null)
                    {
                        return null; // Error already shown in GetDropboxClient
                    }

                    using (var fileStream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        string fileName = Path.GetFileNameWithoutExtension(filePath);
                        string fileExtension = Path.GetExtension(filePath);
                        string timestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                        string uniqueFileName = $"{fileName}_{timestamp}{fileExtension}";
                        string dropboxPath = $"/Avatars/{uniqueFileName}";

                        var uploadResult = await dbx.Files.UploadAsync(
                            dropboxPath,
                            WriteMode.Overwrite.Instance,
                            body: fileStream
                        );

                        var sharedLinkSettings = new SharedLinkSettings();
                        var sharedLink = await dbx.Sharing.CreateSharedLinkWithSettingsAsync(dropboxPath, sharedLinkSettings);

                        string permanentLink = sharedLink.Url.Replace("www.dropbox.com", "dl.dropboxusercontent.com");
                        return permanentLink;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error uploading avatar: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        private async Task SaveAvatarUrlToFirebase(string playerId, string avatarUrl)
        {
            try
            {
                if (string.IsNullOrEmpty(avatarUrl))
                {
                    MessageBox.Show("Avatar URL is empty or null.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string encodedUrl = Uri.EscapeDataString(avatarUrl);

                await firebaseClient
                    .Child("Users")
                    .Child(playerId)
                    .PatchAsync(new { AvatarLink = encodedUrl });

                MessageBox.Show("Avatar URL saved to Firebase successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving avatar URL to Firebase: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnUpdateInfo_Click(object sender, EventArgs e)
        {
            try
            {
                SoundPlayer player = new SoundPlayer("click-button-140881.wav");
                player.Play();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi phát âm thanh: " + ex.Message);
            }

            if (!isEditing)
            {
                // Enable editing
                txtPlayerName.ReadOnly = false;
                txtBirthDate.ReadOnly = false;
                cmbNationality.Enabled = true;
                btnUpdateInfo.Text = "Lưu thông tin";
                isEditing = true;
            }
            else
            {
                // Validate inputs
                if (string.IsNullOrWhiteSpace(txtPlayerName.Text) ||
                    string.IsNullOrWhiteSpace(txtBirthDate.Text) ||
                    cmbNationality.SelectedItem == null)
                {
                    MessageBox.Show("Vui lòng điền đầy đủ thông tin!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Validate username
                string newUsername = txtPlayerName.Text.Trim();
                string playerId = "baongdqu";
                if (await CheckUsernameExists(newUsername, playerId))
                {
                    MessageBox.Show("Tên người dùng đã tồn tại! Vui lòng chọn tên khác.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Validate DateOfBirth
                if (!DateTime.TryParseExact(txtBirthDate.Text, "dd/MM/yyyy",
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None, out DateTime birthDate))
                {
                    MessageBox.Show("Ngày sinh không hợp lệ! Vui lòng nhập theo định dạng DD/MM/YYYY.",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Kiểm tra ngày sinh hợp lệ (không được là ngày trong tương lai)
                if (birthDate > DateTime.Now)
                {
                    MessageBox.Show("Ngày sinh không được là ngày trong tương lai!",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Save to Firebase
                try
                {
                    var updatedData = new
                    {
                        Username = newUsername,
                        DateOfBirth = txtBirthDate.Text,
                        Country = cmbNationality.SelectedItem.ToString()
                    };

                    // Update Firebase
                    await firebaseClient
                        .Child("Users")
                        .Child(playerId)
                        .PatchAsync(updatedData);

                    // Disable editing
                    txtPlayerName.ReadOnly = true;
                    txtBirthDate.ReadOnly = true;
                    cmbNationality.Enabled = false;
                    btnUpdateInfo.Text = "Sửa đổi thông tin";
                    isEditing = false;

                    MessageBox.Show("Thông tin đã được cập nhật thành công!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error updating player info: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnChangePassword_Click(object sender, EventArgs e)
        {
            try
            {
                SoundPlayer player = new SoundPlayer("click-button-140881.wav");
                player.Play();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi phát âm thanh: " + ex.Message);
            }

            using (ChangePasswordForm changePasswordForm = new ChangePasswordForm("baongdqu", firebaseClient))
            {
                if (changePasswordForm.ShowDialog() == DialogResult.OK)
                {
                    MessageBox.Show("Mật khẩu đã được thay đổi thành công!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }


    }

    public class Player
    {
        public string Username { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public int GamesPlayed { get; set; }
        public double WinRate { get; set; }
        public string Status { get; set; }
        public string Email { get; set; }
        public string DateOfBirth { get; set; }
        public string Country { get; set; }
        public string AvatarLink { get; set; }
        public string Password { get; set; }
    }

    public class TokenResponse
    {
        [System.Text.Json.Serialization.JsonPropertyName("access_token")]
        public string AccessToken { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("token_type")]
        public string TokenType { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("scope")]
        public string Scope { get; set; }
    }
}