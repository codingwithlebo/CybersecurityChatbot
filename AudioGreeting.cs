
using System;
using System.IO;
using System.Media;

namespace CybersecurityChatbot
{
    
        public static class AudioGreeting
        {
            public static void PlayGreeting()
            {
            string audioPath = @"C:\Users\Student\source\repos\CybersecurityChatbot\bin\Debug\greeting.wav.wav";

            if (!File.Exists(audioPath))
            {
                Console.WriteLine("[Voice greeting not found — skipping]");
                return;
            }

            try
            {
                using (SoundPlayer player = new SoundPlayer(audioPath))
                {
                    player.PlaySync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("[Audio error: " + ex.Message + "]");
            }
        }
    }
}
