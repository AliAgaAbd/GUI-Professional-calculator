using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using LibVLCSharp.Shared;
namespace Professional_calculator
{
    public class MusicPlayer
    {
        private LibVLC libVLC;
        private MediaPlayer mediaPlayer;
        private List<string> playlist;
        private Random random;
        private bool isAutoPlayEnabled = true;
        private int currentVolume = 50;
        public MusicPlayer()
        {
            try
            {
                Core.Initialize();
                libVLC = new LibVLC();
                mediaPlayer = new MediaPlayer(libVLC);
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
                mediaPlayer.Volume = currentVolume;
                SetRockEqualizer(); 
            }
            catch (Exception ex)
            {
                throw new Exception("خطا در مقداردهی اولیه MusicPlayer: " + ex.Message);
            }
        }
        public void PlayRandomSong(bool isManualPlay = false)
        {
            try
            {
                if (mediaPlayer.IsPlaying)
                {
                    mediaPlayer.Stop();
                }
                if (!isManualPlay && !isAutoPlayEnabled)
                {
                    Console.WriteLine("پخش خودکار غیرفعاله");
                    return;
                }
                int randomIndex = random.Next(0, playlist.Count);
                string songPath = playlist[randomIndex];
                Console.WriteLine($"در حال پخش: {songPath}");
                using (var media = new Media(libVLC, songPath, FromType.FromPath))
                {
                    if (!mediaPlayer.Play(media))
                    {
                        throw new Exception("پخش آهنگ با شکست مواجه شد!");
                    }
                }
                mediaPlayer.EndReached += (s, e) =>
                {
                    if (isAutoPlayEnabled)
                    {
                        PlayRandomSong();
                    }
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine("خطا در پخش آهنگ: " + ex.Message);
                MessageBox.Show("خطا در پخش آهنگ: " + ex.Message);
            }
        }
        public void Stop()
        {
            if (mediaPlayer.IsPlaying)
            {
                mediaPlayer.Stop();
            }
            isAutoPlayEnabled = false;
        }
        public void SetVolume(int volume)
        {
            if (volume >= 0 && volume <= 100)
            {
                currentVolume = volume;
                mediaPlayer.Volume = volume;
            }
        }
        public int GetVolume()
        {
            return currentVolume;
        }
        public void SetAutoPlayEnabled(bool enabled)
        {
            isAutoPlayEnabled = enabled;
        }
        public bool IsPlaying()
        {
            return mediaPlayer.IsPlaying;
        }
        public void SetRockEqualizer()
        {
            try
            {
                using (var equalizer = new Equalizer())
                {
                    equalizer.SetAmp(18.0f, 0); 
                    equalizer.SetAmp(5.0f, 1);
                    equalizer.SetAmp(1.0f, 2);
                    equalizer.SetAmp(-5.0f, 3);
                    equalizer.SetAmp(-4.0f, 4);
                    equalizer.SetAmp(-5.0f, 5);
                    equalizer.SetAmp(1.0f, 6);
                    equalizer.SetAmp(2.0f, 7);
                    equalizer.SetAmp(3.0f, 8);
                    equalizer.SetAmp(5.0f, 9); 
                    mediaPlayer.SetEqualizer(equalizer);
                    Console.WriteLine("اکولایزر با موفقیت به حالت Rock تنظیم شد.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("خطا در تنظیم اکولایزر: " + ex.Message);
                MessageBox.Show("خطا در تنظیم اکولایزر: " + ex.Message);
            }
        }
        public void Dispose()
        {
            mediaPlayer.Dispose();
            libVLC.Dispose();
        }
    }
}