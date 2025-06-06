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
            string musicDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Music");

            if (Directory.Exists(musicDir))
            {
                playlist = Directory.GetFiles(musicDir, "*.mp3").ToList();
                currentIndex = 0;
            }
        }

        private static void PlayCurrent()
        {
            if (currentIndex < 0 || currentIndex >= playlist.Count) return;

            StopCurrent();

            audioFileReader = new AudioFileReader(playlist[currentIndex]);
            waveOutDevice = new WaveOutEvent();
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
            isPlaying = false;
            StopCurrent();
        }

        public static void ToggleMusic()
        {
            if (isPlaying)
                StopBackgroundMusic();
            else
                StartBackgroundMusic();
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
    }
}
