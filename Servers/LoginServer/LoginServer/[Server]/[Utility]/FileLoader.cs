using System.Collections.Generic;
using System.Reflection;
using System.IO;

namespace BPS.LoginServer.Utility
{
    class FileLoader
    {
        private static readonly string parentDirectory = Directory.GetParent(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)).ToString();
        private static readonly string fileName = "OnlineUsers.txt";

        public static string[] ReadTxtFile()
        {
            string path = parentDirectory + "/" + fileName;

            if (!Exists(path))
                return null;

            List<string> result = new List<string>();
            using (StreamReader sr = new StreamReader(path))
            {
                while (sr.Peek() >= 0)
                {
                    result.Add(sr.ReadLine());
                }
            }
            return result.ToArray();
        }

        private static bool Exists(string path)
        {
            return (File.Exists(path));
        }
    }
}
