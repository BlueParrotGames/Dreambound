using System;

namespace BPS.Debugging
{
    public class Logger
    {
        public static void Log(string input)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(">> " + input);
            Console.ForegroundColor = ConsoleColor.White;
        }
        public static void Warn(string input)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(">> " + input);
            Console.ForegroundColor = ConsoleColor.White;
        }
        public static void LogError(string input)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(">> " + input);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
