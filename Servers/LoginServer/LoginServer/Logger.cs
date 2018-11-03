using System;

namespace BPS.Debugging
{
    public class Logger
    {
        public static void Log(object input)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(">> " + input.ToString());
            Console.ForegroundColor = ConsoleColor.White;
        }
        public static void Warn(object input)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(">> " + input.ToString());
            Console.ForegroundColor = ConsoleColor.White;
        }
        public static void LogError(object input)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(">> " + input.ToString());
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
