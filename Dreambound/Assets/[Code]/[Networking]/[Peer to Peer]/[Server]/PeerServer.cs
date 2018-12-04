using System;
using System.Net;
using System.Threading;
using System.Net.Sockets;
using System.Collections.Generic;

using Dreambound.Networking.DataHandling;
using Dreambound.Networking.PeerToPeer.Server.Security;

namespace Dreambound.Networking.PeerToPeer.Server
{
    public class PeerServer
    {
        public static PeerServer Instance { get; internal set; }

        private Socket _socket;

        //Data handling instances
        private PackageHandling _packetHandling;
        private readonly DataHandler _dataHandler;
        public PackageHandling PacketHandler { get => _packetHandling; set => _packetHandling = value; }

        //Data sending instances
        private readonly NetworkSender _networkSender;
        private readonly NetworkSendingQueue _sendingQueue;
        private readonly NetworkSendingLoop _sendingLoop;

        private readonly VerificationLogic _verification;

        private Thread _packetHandlingThread;

        private List<string> _onlineUsers;
        private Dictionary<Socket, PeerClient> _connectedClients;
        public List<string> OnlineUser { get => _onlineUsers; set => _onlineUsers = value; }

        private bool _isRunning;

        public PeerServer()
        {
            Instance = this;

            //Setup the server socket
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _socket.Bind(new IPEndPoint(IPAddress.Any, 4382));
            _socket.Listen(4);
            WaitForConnection();

            //Data handling instances
            _packetHandling = new PackageHandling();
            _dataHandler = new DataHandler();

            //Data sending instances
            _sendingQueue = new NetworkSendingQueue();
            _networkSender = new NetworkSender(_sendingQueue);
            _sendingLoop = new NetworkSendingLoop(_sendingQueue);

            //Security instances
            _verification = new VerificationLogic();

            //Initialize the packet handling loop
            _packetHandlingThread = new Thread(ServerPacketHandlingLoop);
            _packetHandlingThread.Start();
            _isRunning = true;

            _onlineUsers = new List<string>();
            _connectedClients = new Dictionary<Socket, PeerClient>();

        }

        private void WaitForConnection()
        {
            _socket.BeginAccept(new AsyncCallback(AcceptConnection), null);
        }
        private void AcceptConnection(IAsyncResult result)
        {
            Socket clientSocket = _socket.EndAccept(result);
            PeerClient client = new PeerClient(clientSocket, this);

            _connectedClients.Add(clientSocket, client);
            WaitForConnection();
            _verification.SetClientHash(client);
        }

        public void DisconnectSocket(Socket socket)
        {
            _connectedClients.Remove(socket);
        }

        private void ServerPacketHandlingLoop()
        {
            while (_isRunning)
            {
                if (_packetHandling.HasPackets())
                {
                    ClientNetworkPackage package = _packetHandling.PackageQueue.Dequeue();

                    if(_dataHandler.Packets.TryGetValue((int)package.Packet, out DataHandler.Packet packet))
                        packet?.Invoke(package);
                }

                Thread.Sleep(10);
            }
        }
    }
}
