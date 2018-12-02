using BPS.PipeLine;

namespace BPS.InGameServer
{
    class Program
    {
        static void Main(string[] args)
        {
            PipeServer server = new PipeServer();
            Server InGameServer = new Server();
        }
    }
}
