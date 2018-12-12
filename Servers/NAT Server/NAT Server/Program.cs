using System;
using System.Net;
using System.Text;
using System.Net.Sockets;
using System.Threading.Tasks;


using BPG.NATServer;

class Program
{
    static void Main(string[] args)
    {
        //Test test = new Test();
        Server server = new Server();
        Console.ReadKey();
    }
}
