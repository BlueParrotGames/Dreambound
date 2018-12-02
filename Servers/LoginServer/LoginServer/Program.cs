using BPS.PipeLine;

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
