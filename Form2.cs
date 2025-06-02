using GameCaro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NAudio.Wave; // Thêm để sử dụng NAudio

namespace GameCaro_Menu
{
    public partial class Form2 : Form
    {
        private string Username { get; set; } // Thuộc tính lưu tên người dùng
        private WaveOutEvent waveOut; // NAudio WaveOutEvent để phát nhạc nền
        private AudioFileReader audioFileReader; // NAudio AudioFileReader để đọc file âm thanh
        private SoundPlayer clickSoundPlayer; // SoundPlayer cho âm thanh click
        private bool isBackgroundMusicPlaying = false; // Trạng thái phát nhạc nền

        // Constructor mặc định
        public Form2()
        {
            InitializeComponent();
            InitializeSoundPlayers();
        }

        // Constructor với tham số username
        public Form2(string username)
        {
            InitializeComponent();
            Username = username;
            InitializeSoundPlayers();
        }

        // Khởi tạo các trình phát âm thanh
        private void InitializeSoundPlayers()
        {
            try
            {
                // Khởi tạo NAudio cho nhạc nền
                waveOut = new WaveOutEvent();
                audioFileReader = new AudioFileReader("BunnyGirl-VA-12414588.wav");
                waveOut.Init(audioFileReader);
                waveOut.PlaybackStopped += (sender, e) =>
                {
                    if (isBackgroundMusicPlaying)
                    {
                        audioFileReader.Position = 0; // Quay lại đầu khi kết thúc
                        waveOut.Play(); // Phát lại
                    }
                };

                // Khởi tạo SoundPlayer cho âm thanh click
                clickSoundPlayer = new SoundPlayer("click-button-140881.wav");
                clickSoundPlayer.Load();

                // Đặt văn bản ban đầu cho nút nhạc nền
                btnBackgroundMusic.Text = "Play Background Music";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khởi tạo âm thanh: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Xử lý sự kiện nút Player Info
        private void btnPlayerInfo_Click(object sender, EventArgs e)
        {
            try
            {
                PlayClickSound();

                // Sử dụng Username, mặc định là "Guest" nếu null
                string username = Username ?? "Guest";

                // Tìm thư mục cha để chạy file thực thi
                string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string parentDirectory = Directory.GetParent(currentDirectory)?.Parent?.Parent?.Parent?.FullName;

                if (parentDirectory == null)
                {
                    MessageBox.Show("Cannot navigate to the parent directory.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Tìm file GameCaro_SettingInforPlayer.exe
                string[] files = Directory.GetFiles(parentDirectory, "GameCaro_SettingInforPlayer.exe", SearchOption.AllDirectories);

                if (files.Length == 0)
                {
                    MessageBox.Show("GameCaro_SettingInforPlayer.exe not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Chạy file thực thi với tham số username và process ID
                Process.Start(files[0], $"{username} {Process.GetCurrentProcess().Id}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Xử lý sự kiện nút Back
        private void btnBack_Click(object sender, EventArgs e)
        {
            try
            {
                PlayClickSound();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi phát âm thanh: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            this.Close();
        }

        // Xử lý sự kiện nút Background Music
        private void btnBackgroundMusic_Click(object sender, EventArgs e)
        {
            try
            {
                PlayClickSound();

                if (isBackgroundMusicPlaying)
                {
                    waveOut.Stop();
                    isBackgroundMusicPlaying = false;
                    btnBackgroundMusic.Text = "Play Background Music";
                }
                else
                {
                    waveOut.Play();
                    isBackgroundMusicPlaying = true;
                    btnBackgroundMusic.Text = "Stop Background Music";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi phát nhạc nền: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Xử lý sự kiện nút Game Sound
        private void btnGameSound_Click(object sender, EventArgs e)
        {
            try
            {
                PlayClickSound();
                // Có thể thêm logic bổ sung cho âm thanh game tại đây
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi phát âm thanh: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Xử lý sự kiện nút Game Intro
        private void btnGameIntro_Click(object sender, EventArgs e)
        {
            try
            {
                PlayClickSound();
                // Có thể thêm logic bổ sung cho phần giới thiệu game tại đây
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi phát âm thanh: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Xử lý sự kiện nút Leaderboard
        private void btnLeaderboard_Click(object sender, EventArgs e)
        {
            try
            {
                PlayClickSound();
                // Có thể thêm logic bổ sung cho bảng xếp hạng tại đây
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi phát âm thanh: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Phương thức hỗ trợ phát âm thanh click
        private void PlayClickSound()
        {
            try
            {
                clickSoundPlayer.Play();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi phát âm thanh click: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Ghi đè sự kiện đóng Form để dừng và giải phóng âm thanh
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            try
            {
                if (waveOut != null)
                {
                    waveOut.Stop();
                    waveOut.Dispose();
                }
                if (audioFileReader != null)
                {
                    audioFileReader.Dispose();
                }
                if (clickSoundPlayer != null)
                {
                    clickSoundPlayer.Stop();
                    clickSoundPlayer.Dispose();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi dừng âm thanh: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}