using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace DoAnMonHocNT106
{
    public static class MusicPlayer
    {
        private static IWavePlayer waveOutDevice;
        private static AudioFileReader audioFileReader;
        private static List<string> playlist = new List<string>();
        private static int currentIndex = 0;
        private static bool isPlaying = false;
        private static bool isSoundEnabled = true; // Biến để bật/tắt âm thanh game
        private static float soundVolume = 0.15f; // Âm lượng âm thanh game

        public static void SetSoundEnabled(bool enabled)
        {
            isSoundEnabled = enabled;
        }

        public static bool IsSoundEnabled()
        {
            return isSoundEnabled;
        }

        public static void SetSoundVolume(float volume)
        {
            soundVolume = volume;
        }

        public static float GetSoundVolume()
        {
            return soundVolume;
        }

        public static void StartBackgroundMusic()
        {
            try
            {
                if (isPlaying) return;

                LoadPlaylist();

                if (playlist.Count == 0)
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

        public static bool IsMusicPlaying()
        {
            return isPlaying;
        }

        private static void LoadPlaylist()
        {
            // Thư mục Music nằm trong Resources của project (đi lên 2 cấp rồi vào Resources/Music)
            string musicDir = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "Resources", "Music"));
            if (!Directory.Exists(musicDir))
            {
                MessageBox.Show($"Thư mục nhạc không tồn tại: {musicDir}");
                return;
            }

            var mp3Files = Directory.GetFiles(musicDir, "*.mp3").ToList();
            if (mp3Files.Count == 0)
            {
                MessageBox.Show($"Không tìm thấy file .mp3 nào trong thư mục: {musicDir}");
                return;
            }

            playlist = mp3Files;
            currentIndex = 0;
        }

        public static void DisposeAll()
        {
            StopCurrent();
            playlist.Clear();
            isPlaying = false;
        }

        private static void PlayCurrent()
        {
            if (playlist.Count == 0) return;

            StopCurrent();

            // Chọn ngẫu nhiên một file từ playlist
            Random rand = new Random();
            currentIndex = rand.Next(playlist.Count);

            audioFileReader = new AudioFileReader(playlist[currentIndex]);
            waveOutDevice = new DirectSoundOut();
            waveOutDevice.Init(audioFileReader);
            waveOutDevice.PlaybackStopped += OnPlaybackStopped;
            waveOutDevice.Play();
        }

        private static void OnPlaybackStopped(object sender, StoppedEventArgs e)
        {
            if (!isPlaying) return;

            currentIndex = (currentIndex + 1) % playlist.Count; // chuyển bài
            PlayCurrent();
        }

        private static void StopCurrent()
        {
            waveOutDevice?.Stop();
            waveOutDevice?.Dispose();
            waveOutDevice = null;

            audioFileReader?.Dispose();
            audioFileReader = null;
        }

        public static void StopBackgroundMusic()
        {
            try
            {
                if (waveOutDevice != null && isPlaying)
                {
                    waveOutDevice.Stop();
                    waveOutDevice.Dispose();
                    waveOutDevice = null;
                }

                if (audioFileReader != null)
                {
                    audioFileReader.Dispose();
                    audioFileReader = null;
                }

                isPlaying = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi dừng nhạc nền: " + ex.Message);
            }
        }

        public static void ToggleMusic()
        {
            if (isPlaying)
                StopBackgroundMusic();
            else
                StartBackgroundMusic();
        }

        public static float GetVolume()
        {
            return audioFileReader != null ? audioFileReader.Volume : 1.0f;
        }

        public static void NextTrack()
        {
            if (!isPlaying || playlist.Count == 0) return;

            currentIndex = (currentIndex + 1) % playlist.Count;
            PlayCurrent();
        }

        public static void PreviousTrack()
        {
            if (!isPlaying || playlist.Count == 0) return;

            currentIndex = (currentIndex - 1 + playlist.Count) % playlist.Count;
            PlayCurrent();
        }

        public static void SetVolume(float volume) 
        {
            if (audioFileReader != null)
                audioFileReader.Volume = volume;
        }

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
