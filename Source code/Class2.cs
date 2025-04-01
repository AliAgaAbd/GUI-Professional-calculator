using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Professional_calculator;
using NAudio.Wave;
using OpenQA.Selenium.DevTools.V131.Debugger;
namespace Professional_calculator
{
    public class MusicPlayer : IDisposable
    {
        private List<string> playlist;
        private Random random;
        private bool isAutoPlayEnabled = true;
        private float currentVolume = 0.5f;
        private IWavePlayer waveOut;
        private AudioFileReader audioFileReader;
        public MusicPlayer()
        {
            try
            {
                random = new Random();
                playlist = new List<string>
                {
                    $"{AppDomain.CurrentDomain.BaseDirectory}\\music1.mp3",
                    $"{AppDomain.CurrentDomain.BaseDirectory}\\music2.mp3",
                    $"{AppDomain.CurrentDomain.BaseDirectory}\\music3.mp3",
                    $"{AppDomain.CurrentDomain.BaseDirectory}\\music4.mp3",
                    $"{AppDomain.CurrentDomain.BaseDirectory}\\music5.mp3",
                    $"{AppDomain.CurrentDomain.BaseDirectory}\\music6.mp3",
                    $"{AppDomain.CurrentDomain.BaseDirectory}\\music7.mp3"
                };
                foreach (var songPath in playlist)
                {
                    if (!File.Exists(songPath))
                    {
                        throw new FileNotFoundException($"فایل آهنگ پیدا نشد: {songPath}");
                    }
                }
                waveOut = new WaveOutEvent();
            }
            catch (Exception ex)
            {
                throw new Exception("خطا در مقداردهی اولیه MusicPlayer: " + ex.Message);
            }
        }
        public void PlayRandomSong(bool isManualPlay)
        {
            try
            {
                if (waveOut != null || waveOut.PlaybackState == PlaybackState.Playing || waveOut.PlaybackState == PlaybackState.Paused || waveOut.PlaybackState == PlaybackState.Stopped)
                {
                    waveOut.Stop();
                }
                if (!isManualPlay && !isAutoPlayEnabled)
                {
                    Console.WriteLine("پخش خودکار غیرفعاله");
                    return;
                }
                waveOut.PlaybackStopped -= WaveOut_PlaybackStopped;
                waveOut.PlaybackStopped += WaveOut_PlaybackStopped;
                if (waveOut.PlaybackState == PlaybackState.Paused || waveOut.PlaybackState == PlaybackState.Stopped || waveOut.PlaybackState == PlaybackState.Playing)
                {
                    if (isManualPlay == true)
                    {
                        WaveOutPlay(true);
                    }
                    else
                    {
                        WaveOutPlay(false);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("خطا در پخش آهنگ: " + ex.Message);
                MessageBox.Show("خطا در پخش آهنگ: " + ex.Message);
            }
        }
        private void WaveOut_PlaybackStopped(object sender, StoppedEventArgs e)
        {
            if (isAutoPlayEnabled == true)
            {
                WaveOutPlay(true);
            }
            else
            {
                WaveOutPlay(false);
            }
        }
        public void Stop()
        {
            if (waveOut != null && waveOut.PlaybackState == PlaybackState.Playing)
            {
                waveOut.Stop();
            }
            isAutoPlayEnabled = false;
        }
        public void SetVolume(int volume)
        {
            if (volume >= 0 && volume <= 100)
            {
                currentVolume = volume / 100f;
                if (waveOut != null)
                {
                    waveOut.Volume = currentVolume;
                }
            }
        }
        public int GetVolume()
        {
            return (int)(currentVolume * 100);
        }
        public void SetAutoPlayEnabled(bool enabled)
        {
            isAutoPlayEnabled = enabled;
        }
        public bool IsPlaying()
        {
            return waveOut != null && waveOut.PlaybackState == PlaybackState.Playing;
        }
        public void Dispose()
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
        }
        public void WaveOutPlay(bool isManualPlay)
        {
            if (!isManualPlay && !isAutoPlayEnabled)
            {
                Console.WriteLine("پخش خودکار غیرفعاله");
                return;
            }
            waveOut.Stop();
            int randomIndex = random.Next(0, playlist.Count);
            string songPath = playlist[randomIndex];
            Console.WriteLine($"در حال پخش: {songPath}");
            audioFileReader = new AudioFileReader(songPath);
            waveOut.Init(audioFileReader);
            waveOut.Volume = currentVolume;
            waveOut.Play();
        }
    }
}