using System;

namespace BPG.Debugging
{
    public class Logger
    {
        //Log Normal
        public static void Log(object input, int tabCount = 0)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(InsertTabs(">> " + input.ToString(), tabCount));
            Console.ForegroundColor = ConsoleColor.White;
        }
        public static void Log(string format, object arg0, int tabCount = 0)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(InsertTabs(">> " + string.Format(format, arg0), tabCount));
            Console.ForegroundColor = ConsoleColor.White;
        }
        public static void Log(string format, object arg0, object arg1, int tabCount = 0)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(InsertTabs(">> " + string.Format(format, arg0, arg1), tabCount));
            Console.ForegroundColor = ConsoleColor.White;
        }
        public static void Log(string format, object arg0, object arg1, object arg2, int tabCount = 0)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(InsertTabs(">> " + string.Format(format, arg0, arg1, arg2), tabCount));
            Console.ForegroundColor = ConsoleColor.White;
        }
        public static void Log(string format, object[] args, int tabCount = 0)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(InsertTabs(">> " + string.Format(format, args), tabCount));
            Console.ForegroundColor = ConsoleColor.White;
        }

        //Log Warn
        public static void LogWarning(object input, int tabCount = 0)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(InsertTabs(">> " + input.ToString(), tabCount));
            Console.ForegroundColor = ConsoleColor.White;
        }
        public static void LogWarning(string format, object arg0, int tabCount = 0)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(InsertTabs(">> " + string.Format(format, arg0), tabCount));
            Console.ForegroundColor = ConsoleColor.White;
        }
        public static void LogWarning(string format, object arg0, object arg1, int tabCount = 0)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(InsertTabs(">> " + string.Format(format, arg0, arg1), tabCount));
            Console.ForegroundColor = ConsoleColor.White;
        }
        public static void LogWarning(string format, object arg0, object arg1, object arg2, int tabCount = 0)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(InsertTabs(">> " + string.Format(format, arg0, arg1, arg2), tabCount));
            Console.ForegroundColor = ConsoleColor.White;
        }
        public static void LogWarning(string format, object[] args, int tabCount = 0)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(InsertTabs(">> " + string.Format(format, args), tabCount));
            Console.ForegroundColor = ConsoleColor.Yellow;
        }

        //Log Error
        public static void LogError(object input, int tabCount = 0)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(InsertTabs(">> " + input.ToString(), tabCount));
            Console.ForegroundColor = ConsoleColor.White;
        }
        public static void LogError(string format, object arg0, int tabCount = 0)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(InsertTabs(">> " + string.Format(format, arg0), tabCount));
            Console.ForegroundColor = ConsoleColor.White;
        }
        public static void LogError(string format, object arg0, object arg1, int tabCount = 0)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(InsertTabs(">> " + string.Format(format, arg0, arg1), tabCount));
            Console.ForegroundColor = ConsoleColor.White;
        }
        public static void LogError(string format, object arg0, object arg1, object arg2, int tabCount = 0)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(InsertTabs(">> " + string.Format(format, arg0, arg1, arg2), tabCount));
            Console.ForegroundColor = ConsoleColor.White;
        }
        public static void LogError(string format, object[] args, int tabCount = 0)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(InsertTabs(">> " + string.Format(format, args), tabCount));
            Console.ForegroundColor = ConsoleColor.Yellow;
        }

        //Formatting
        private static string InsertTabs(string input, int count)
        {
            string tabString = "";

            for (int i = 0; i < count; i++)
            {
                tabString += "  ";
            }

            tabString += input;

            return tabString;
        }
    }
}
