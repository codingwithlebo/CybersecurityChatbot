using System;
using System.Threading;

namespace CybersecurityChatbot
{
    

    public static class ConsoleUI
    {
        // Writes text in a specific colour then resets
        public static void WriteColour(string text, ConsoleColor colour)
        {
            Console.ForegroundColor = colour;
            Console.WriteLine(text);
            Console.ResetColor();
        }

        // Simulates a typing effect — conversational feel
        public static void TypeWrite(string text, ConsoleColor colour = ConsoleColor.White, int delay = 18)
        {
            Console.ForegroundColor = colour;
            foreach (char c in text)
            {
                Console.Write(c);
                Thread.Sleep(delay);
            }
            Console.ResetColor();
            Console.WriteLine();
        }

        public static void Divider()
        {
            WriteColour("──────────────────────────────────────────────", ConsoleColor.DarkCyan);
        }

        public static void BotSay(string message)
        {
            Console.Write("  🤖 ");
            TypeWrite(message, ConsoleColor.Green);
        }

        public static string UserPrompt(string name)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($"  [{name}] > ");
            Console.ResetColor();
            return Console.ReadLine();
        }
    }
}
