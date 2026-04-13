using CybersecurityChatbot;
using System;
using System.Xml.Linq;

namespace CybersecurityChatbot
{


    public static class ChatBot
    {
        public static void Start() {
           // must be static void Start

       
            ConsoleUI.Divider();
            ConsoleUI.WriteColour("  Welcome to the Cybersecurity Awareness Bot!", ConsoleColor.Cyan);
            ConsoleUI.Divider();
            Console.WriteLine();

            ConsoleUI.WriteColour("  What is your name?", ConsoleColor.White);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("  > ");
            Console.ResetColor();
            string name = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(name))
                name = "User";

            name = name.Trim();

            Console.WriteLine();
            ConsoleUI.BotSay("Nice to meet you, " + name + "! I am CyberBot.");
            ConsoleUI.BotSay("Ask me about passwords, phishing, safe browsing, 2FA, or malware.");
            ConsoleUI.BotSay("Type exit to quit.");
            ConsoleUI.Divider();

            while (true)
            {
                string input = ConsoleUI.UserPrompt(name);

                if (string.IsNullOrWhiteSpace(input))
                {
                    ConsoleUI.BotSay("I did not receive any input. Please type a question!");
                    continue;
                }

                string response = ResponseHandler.GetResponse(input);

                if (response == "QUIT")
                {
                    Console.WriteLine();
                    ConsoleUI.BotSay("Stay safe out there, " + name + "! Goodbye.");
                    ConsoleUI.Divider();
                    Console.WriteLine("Press any key to exit...");
                    Console.ReadKey(); // keeps window open
                    break;
                }

Console.WriteLine();
ConsoleUI.BotSay(response);
ConsoleUI.Divider();
            }
        }
    }
}
