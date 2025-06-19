// MusicPlayer.cs
// Lớp quản lý phát nhạc nền và âm thanh sự kiện trong game sử dụng NAudio
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace DoAnMonHocNT106
{
    /// <summary>
    /// Lớp MusicPlayer cung cấp các chức năng phát nhạc nền,
    /// chuyển bài, dừng nhạc và phát âm thanh hiệu ứng (click)
    /// </summary>
    public static class MusicPlayer
    {
        // Thiết bị phát âm thanh
        private static IWavePlayer waveOutDevice;
        // Đối tượng đọc file âm thanh
        private static AudioFileReader audioFileReader;
        // Danh sách đường dẫn các file nhạc nền
        private static List<string> playlist = new List<string>();
        // Vị trí bài hát hiện tại trong playlist
        private static int currentIndex = 0;
        // Cờ trạng thái đang phát nhạc
        private static bool isPlaying = false;
        // Cờ bật/tắt âm thanh hiệu ứng
        private static bool isSoundEnabled = true;
        // Âm lượng phát âm thanh hiệu ứng (0.0f - 1.0f)
        private static float soundVolume = 0.15f;

        /// <summary>
        /// Bật hoặc tắt âm thanh hiệu ứng click
        /// </summary>
        public static void SetSoundEnabled(bool enabled)
        {
            isSoundEnabled = enabled;
        }

        /// <summary>
        /// Kiểm tra trạng thái âm thanh hiệu ứng có được bật hay không
        /// </summary>
        public static bool IsSoundEnabled() => isSoundEnabled;

        /// <summary>
        /// Thiết lập âm lượng phát hiệu ứng click
        /// </summary>
        public static void SetSoundVolume(float volume) => soundVolume = volume;

        /// <summary>
        /// Lấy giá trị âm lượng hiệu ứng click hiện tại
        /// </summary>
        public static float GetSoundVolume() => soundVolume;

        /// <summary>
        /// Bắt đầu phát nhạc nền theo playlist
        /// </summary>
        public static void StartBackgroundMusic()
        {
            try
            {
                if (isPlaying) return;
                LoadPlaylist();
                if (!playlist.Any())
                {
                    MessageBox.Show("Không tìm thấy file nhạc trong thư mục Music.");
                    return;
                }
                PlayCurrent();
                isPlaying = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi phát nhạc: " + ex.Message);
            }
        }

        /// <summary>
        /// Kiểm tra có đang phát nhạc nền không
        /// </summary>
        public static bool IsMusicPlaying() => isPlaying;

        /// <summary>
        /// Tải danh sách file nhạc từ thư mục Resources/Music
        /// </summary>
        private static void LoadPlaylist()
        {
            string musicDir = Path.GetFullPath(
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "Resources", "Music")
            );
            if (!Directory.Exists(musicDir))
            {
                MessageBox.Show($"Thư mục nhạc không tồn tại: {musicDir}");
                return;
            }
            playlist = Directory.GetFiles(musicDir, "*.mp3").ToList();
            currentIndex = 0;
        }

        /// <summary>
        /// Dừng phát và giải phóng tài nguyên
        /// </summary>
        public static void DisposeAll()
        {
            StopCurrent();
            playlist.Clear();
            isPlaying = false;
        }

        /// <summary>
        /// Phát bài hát hiện tại trong playlist, chọn ngẫu nhiên
        /// </summary>
        private static void PlayCurrent()
        {
            if (!playlist.Any()) return;
            StopCurrent();
            // Chọn bài ngẫu nhiên
            currentIndex = new Random().Next(playlist.Count);
            audioFileReader = new AudioFileReader(playlist[currentIndex]) { Volume = GetSoundVolume() };
            waveOutDevice = new WaveOutEvent();
            waveOutDevice.Init(audioFileReader);
            waveOutDevice.PlaybackStopped += OnPlaybackStopped;
            waveOutDevice.Play();
        }

        /// <summary>
        /// Xử lý sự kiện khi một bài hát kết thúc, tự động chuyển bài
        /// </summary>
        private static void OnPlaybackStopped(object sender, StoppedEventArgs e)
        {
            if (!isPlaying) return;
            currentIndex = (currentIndex + 1) % playlist.Count;
            PlayCurrent();
        }

        /// <summary>
        /// Dừng phát bài hiện tại và giải phóng tài nguyên liên quan
        /// </summary>
        private static void StopCurrent()
        {
            waveOutDevice?.Stop();
            waveOutDevice?.Dispose();
            waveOutDevice = null;
            audioFileReader?.Dispose();
            audioFileReader = null;
        }

        /// <summary>
        /// Dừng nhạc nền
        /// </summary>
        public static void StopBackgroundMusic()
        {
            try
            {
                StopCurrent();
                isPlaying = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi dừng nhạc nền: " + ex.Message);
            }
        }

        /// <summary>
        /// Bật/tắt nhạc nền dựa trên trạng thái hiện tại
        /// </summary>
        public static void ToggleMusic()
        {
            if (isPlaying) StopBackgroundMusic(); else StartBackgroundMusic();
        }

        /// <summary>
        /// Lấy âm lượng hiện tại của trình đọc file
        /// </summary>
        public static float GetVolume() => audioFileReader?.Volume ?? 1.0f;

        /// <summary>
        /// Chuyển sang bài kế tiếp
        /// </summary>
        public static void NextTrack()
        {
            if (!isPlaying || !playlist.Any()) return;
            currentIndex = (currentIndex + 1) % playlist.Count;
            PlayCurrent();
        }

        /// <summary>
        /// Quay về bài trước đó
        /// </summary>
        public static void PreviousTrack()
        {
            if (!isPlaying || !playlist.Any()) return;
            currentIndex = (currentIndex - 1 + playlist.Count) % playlist.Count;
            PlayCurrent();
        }

        /// <summary>
        /// Thiết lập âm lượng nhạc nền
        /// </summary>
        public static void SetVolume(float volume)
        {
            if (audioFileReader != null)
                audioFileReader.Volume = volume;
        }

        /// <summary>
        /// Phát hiệu ứng âm thanh click nếu được bật
        /// </summary>
        public static void PlayClickSound()
        {
            if (!isSoundEnabled) return;
            try
            {
                string soundDir = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "Resources", "Music"));
                string soundFile = Path.Combine(soundDir, "click-button-140881.wav");

                if (File.Exists(soundFile))
                {
                    var audioFile = new AudioFileReader(soundFile);
                    var outputDevice = new WaveOutEvent();
                    audioFile.Volume = soundVolume;
                    outputDevice.Init(audioFile);
                    outputDevice.PlaybackStopped += (s, e) =>
                    {
                        audioFile.Dispose();
                        outputDevice.Dispose();
                    };
                    outputDevice.Play();
                }
                else
                {
                    MessageBox.Show($"Không tìm thấy file âm thanh 'click-button-140881.wav' trong thư mục: {soundDir}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi phát âm thanh click: " + ex.Message);
            }
        }
    }
}
