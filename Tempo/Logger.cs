using System;

namespace Tempo
{
    public static class Logger
    {
        public static void Info(string text)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("[INFO] " + text);
            Console.ResetColor();
        }

        public static void Default(string text)
        {
            Console.WriteLine(text);
        }

        public static void Error(string text)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("[ERROR] " + text);
            Console.ResetColor();
        }

        public static void Warning(string text)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("[WARN] " + text);
            Console.ResetColor();
        }
    }
}