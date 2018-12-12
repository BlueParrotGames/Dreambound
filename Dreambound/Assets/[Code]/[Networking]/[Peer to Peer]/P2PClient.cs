using System;
using System.Net;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Threading.Tasks;

using UnityEngine;

using Dreambound.Networking.Data;
using Dreambound.Networking.Data.Sending;
using Dreambound.Networking.Data.Handlers;
using Dreambound.Networking.Utility;

namespace Dreambound.Networking.P2P
{
    public class P2PClient
    {
        private UdpClient _client;
        private IPEndPoint _serverEndpoint;

        //Sending Instances
        private NetworkSender _networkSender;
        private NetworkSendingQueue _sendingQueue;
        private NetworkSendingLoop _sendingLoop;

        //Handling Instances
        private PackageHandling _packetHandler;
        private DataHandler _dataHandler;

        //Handling Variables
        private bool _isRunning;
        private Thread _packetHandlingThread;

        public P2PClient(IPEndPoint endPoint)
        {
            _client = new UdpClient();
            _serverEndpoint = endPoint;

            //Sending Instances
            _sendingQueue = new NetworkSendingQueue();
            _networkSender = new NetworkSender(_sendingQueue);
            _sendingLoop = new NetworkSendingLoop(_sendingQueue, _client);

            //Handling Instances
            _packetHandler = new PackageHandling();
            _dataHandler = new DataHandler();

            //Handling Variables
            _isRunning = true;
            _packetHandlingThread = new Thread(PacketHandlingLoop);
            _packetHandlingThread.Start();

            StartReceiveLoop();
        }

        private void StartReceiveLoop()
        {
            Task.Factory.StartNew(async () =>
            {
                while (_isRunning)
                {
                    //Get and queue the received data for handling
                    Received receivedData = await Receive();
                    _packetHandler.QueuePackage(receivedData.Data, receivedData.Sender);
                }
            });
        }
        private async Task<Received> Receive()
        {
            UdpReceiveResult result = await _client.ReceiveAsync();

            return new Received()
            {
                Sender = result.RemoteEndPoint,
                Data = result.Buffer
            };
        }

        private void PacketHandlingLoop()
        {
            while (_isRunning)
            {
                if (_packetHandler.HasPackets())
                {
                    ClientNetworkPackage package = _packetHandler.PackageQueue.Dequeue();

                    if (_dataHandler.Packets.TryGetValue((int)package.PacketType, out DataHandler.Packet packet))
                    {
                        packet?.Invoke(package);
                    }
                }
            }
        }

        public void Send(string text)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteInt((int)PacketType.Message);
            buffer.WriteBytes(Encoding.UTF8.GetBytes(text));

            NetworkSender.SendPacket(buffer.ReadBytes(buffer.Length()), _serverEndpoint);
        }
    }
    public struct Received
    {
        public IPEndPoint Sender;
        public byte[] Data;
    }
}
