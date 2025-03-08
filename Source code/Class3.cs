using LibVLCSharp.Shared;
using System;
namespace Professional_calculator
{
    public class EqualizerManager
    {
        private readonly LibVLC libVLC;
        private readonly MediaPlayer mediaPlayer;
        public EqualizerManager(LibVLC libVLC, MediaPlayer mediaPlayer)
        {
            this.libVLC = libVLC ?? throw new ArgumentNullException(nameof(libVLC));
            this.mediaPlayer = mediaPlayer ?? throw new ArgumentNullException(nameof(mediaPlayer));
        }
        public void SetRockPreset()
        {
            try
            {
                using (var equalizer = new Equalizer())
                {
                    equalizer.SetPreamp(10.4f);
                    equalizer.SetAmp(18f, 0);
                    equalizer.SetAmp(12.0f, 1);
                    equalizer.SetAmp(0.0f, 2);
                    equalizer.SetAmp(0.0f, 3);
                    equalizer.SetAmp(1.0f, 4);
                    equalizer.SetAmp(0.0f, 5);
                    equalizer.SetAmp(0.0f, 6);
                    equalizer.SetAmp(-2.0f, 7);
                    equalizer.SetAmp(-5.0f, 8);
                    equalizer.SetAmp(-7.0f, 9);
                    if (!mediaPlayer.SetEqualizer(equalizer))
                    {
                        throw new Exception("تنظیم اکولایزر به حالت Rock با شکست مواجه شد!");
                    }
                    Console.WriteLine("اکولایزر با موفقیت به حالت Rock تنظیم شد.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("خطا در تنظیم اکولایزر: " + ex.Message);
                throw;
            }
        }
    }
}