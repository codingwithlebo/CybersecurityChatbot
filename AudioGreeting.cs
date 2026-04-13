
using System;
using System.IO;
using System.Media;

namespace CybersecurityChatbot
{

    public static class AudioGreeting
    {
        public static void PlayGreeting()
        {
            // Fixed: Looks for greeting.wav in the same folder as the .exe
            string audioPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "greeting.wav");

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