using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading;

using BPS.Debugging;
using BPS.LoginServer.Sending;
using BPS.LoginServer.Security;
using BPS.LoginServer.DataHandling;

namespace BPS.LoginServer
{
    public class Server : IDisposable
    {
        public static Server Instance;

        private Socket _socket;

        private PackageHandling _packetHandler;
        private DataHandler _dataHandler;

        //Sending instances
        private readonly NetworkSender _networkSender;
        private readonly NetworkSendingQueue _sendingQueue;
        private readonly ServerSendingLoop _sendingLoop;

        private VerificationLogic _verification;

        private Dictionary<Socket, Client> _connectedClients;
        private Dictionary<string, int> _connectedUsers;
        private Thread _packetHandlingThread;

        private bool _isRunning;

        public PackageHandling PacketHandler { get => _packetHandler; set => _packetHandler = value; }
        internal Dictionary<Socket, Client> ConnectedClients { get => _connectedClients; set => _connectedClients = value; }
        internal Dictionary<string, int> ConnectedUsers { get => _connectedUsers; set => _connectedUsers = value; }

        public Server()
        {
            Instance = this;
            ConnectedClients = new Dictionary<Socket, Client>();
            ConnectedUsers = new Dictionary<string, int>();

            //Setup all the data components
            PacketHandler = new PackageHandling();
            _dataHandler = new DataHandler();
            _verification = new VerificationLogic();
            _sendingQueue = new NetworkSendingQueue();
            _networkSender = new NetworkSender(_sendingQueue);
            _sendingLoop = new ServerSendingLoop(this, _sendingQueue);

            //Setup the server socket
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _socket.Bind(new IPEndPoint(IPAddress.Any, 4381));
            _socket.Listen(20);
            _socket.BeginAccept(new AsyncCallback(AcceptConnection), null);

            _isRunning = true;

            _packetHandlingThread = new Thread(ServerPacketHandlingLoop);
            _packetHandlingThread.Start();

            Logger.Log("Server started\n");
        }

        private void AcceptConnection(IAsyncResult result)
        {
            Socket clientSocket = _socket.EndAccept(result);
            Client client = new Client(clientSocket, this);

            ConnectedClients.Add(clientSocket, client);
            _socket.BeginAccept(new AsyncCallback(AcceptConnection), null);
            _verification.SetClientHash(client);

            Logger.Warn("A client has connected (IP: " + clientSocket.RemoteEndPoint + ")");
        }
        public void DisconnectPlayer(Socket socket)
        {
            ConnectedClients.Remove(socket);
            ConnectedUsers.Remove(ConnectedClients[socket].Username);
        }

        private void ServerPacketHandlingLoop()
        {
            while (_isRunning)
            {
                if (_packetHandler.HasPackets())
                {
                    ClientNetworkPackage package = _packetHandler.PackageQueue.Dequeue();

                    if(_dataHandler.Packets.TryGetValue((int)package.Packet, out DataHandler.Packet packet))
                    {
                        packet?.Invoke(package);
                    }
                }

                Thread.Sleep(10);
            }
        }

        public void Dispose()
        {
            //_socket.Close();
            //_packetHandlingThread.Abort();
        }

        public bool IsUserAlreadyOnline(string username)
        {
            return ConnectedUsers.ContainsKey(username);
        }
    }
}
