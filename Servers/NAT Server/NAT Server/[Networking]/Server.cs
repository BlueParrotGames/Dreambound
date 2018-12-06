using System;
using System.Net;
using System.Threading;
using System.Diagnostics;
using System.Net.Sockets;
using System.Collections.Generic;

using BPG.Debugging;
using BPG.NATServer.Utility;
using BPG.NATServer.Sending;

namespace BPG.NATServer
{
    public class Server
    {
        public static Server Instance;

        private Socket _socket;

        //Packet Sending Instances
        private readonly NetworkSender _networkSender;
        private readonly NetworkSendingQueue _sendingQueue;
        private readonly ServerSendingLoop _sendingLoop;

        //Response Components
        private Queue<Socket> _responseQueue;
        private Thread _responseThread;
        private bool _isRunning;

        private ByteBuffer _buffer;

        private Stopwatch _stopwatch;

        public Server()
        {
            Instance = this;

            //Setup Data Components
            _sendingQueue = new NetworkSendingQueue();
            _networkSender = new NetworkSender(_sendingQueue);
            _sendingLoop = new ServerSendingLoop(_sendingQueue);

            //Setup Server Socket
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _socket.Bind(new IPEndPoint(IPAddress.Any, 4383));
            _socket.Listen(5);
            BeginAccept();

            _buffer = new ByteBuffer();
            _responseQueue = new Queue<Socket>();
            _isRunning = true;
            _responseThread = new Thread(ServerResponseLoop);
            _responseThread.Start();

            _sendingLoop.OnPacketSent += DisconnectSocket;

            //Diagnostics
            Logger.LogWarning("Server Online...\n");
            _stopwatch = new Stopwatch();
        }

        private void BeginAccept()
        {
            _socket.BeginAccept(new AsyncCallback(AcceptCallback), _socket);
        }
        private void AcceptCallback(IAsyncResult result)
        {
            Socket workSocket = _socket.EndAccept(result);
            _responseQueue.Enqueue(workSocket);
            _stopwatch.Start();

            Logger.Log(" -- ({0}) --", workSocket.RemoteEndPoint);
            Logger.Log("Connection astablished", 2); 

            BeginAccept();
        }

        private void DisconnectSocket(Socket socket)
        {
            socket.Close();
            Logger.Log("Connection Terminated", 2);
            _stopwatch.Stop();
            Logger.Log("Elapsed Time: {0} miliseconds\n", _stopwatch.Elapsed.Milliseconds, 2);
        }

        private void ServerResponseLoop()
        {
            while (_isRunning)
            {
                if(_responseQueue.Count > 0)
                {
                    Socket currentSocket = _responseQueue.Dequeue();

                    _buffer.Clear();
                    _buffer.WriteInt((int)PacketType.RemoteAddressResponse);
                    _buffer.WriteString(currentSocket.RemoteEndPoint.ToString());

                    NetworkSender.SendPacket(_buffer, currentSocket);

                    Logger.Log("Sending NAT response", 2);
                }
            }
        }
    }
}
