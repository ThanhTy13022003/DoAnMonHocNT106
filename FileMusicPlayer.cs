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
            string musicDir = AppDomain.CurrentDomain.BaseDirectory;
            string musicFile = Path.Combine(musicDir, "BunnyGirl-VA-12414588.wav");

            if (File.Exists(musicFile))
            {
                playlist = new List<string> { musicFile };
                currentIndex = 0;
            }
            else
            {
                MessageBox.Show("Không tìm thấy file nhạc 'BunnyGirl-VA-12414588.wav' trong thư mục.");
            }
        }

        public static void DisposeAll()
        {
            StopCurrent();
            playlist.Clear();
            isPlaying = false;
        }

        private static void PlayCurrent()
        {
            if (currentIndex < 0 || currentIndex >= playlist.Count) return;

            StopCurrent();

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
                string soundDir = AppDomain.CurrentDomain.BaseDirectory;
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
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi phát âm thanh click: " + ex.Message);
            }
        }
    }
}
