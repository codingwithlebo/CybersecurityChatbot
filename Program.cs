using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CybersecurityChatbot
{
    
        public class Program
        {
            static void Main(string[] args)
            {
            AudioGreeting.PlayGreeting();
            AsciiArt.Display();
            ChatBot.Start();
        }
    }
}