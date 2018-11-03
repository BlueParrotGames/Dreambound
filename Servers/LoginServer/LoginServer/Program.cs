using BPS.PipeLine;
using System;

namespace BPS.LoginServer
{
    class Program
    {
        static void Main(string[] args)
        {
            PipeClient client = new PipeClient();
            Server loginServer = new Server();
        }
    }
}
