﻿using System;
using System.Net;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Threading.Tasks;

using BPG.Debugging;
using BPG.NATServer.Data;
using BPG.NATServer.Data.Sending;
using BPG.NATServer.Data.Handlers;

namespace BPG.NATServer
{
    public class Server : IDisposable
    {
        private UdpClient _client;

        //Sending Instances
        private readonly NetworkSender _networkSender;
        private readonly NetworkSendingQueue _sendingQueue;
        private readonly NetworkSendingLoop _sendingLoop;

        //Handling Instances
        private PacketHandler _packetHandler;
        private DataHandler _dataHandler;

        //Handling Variables
        private bool _isRunning;
        private Thread _packetHandlingThread;

        public Server() : this(new IPEndPoint(IPAddress.Any, 4383)) { }
        public Server(IPEndPoint endPoint)
        {
            _client = new UdpClient(endPoint);

            //Sending Instances
            _sendingQueue = new NetworkSendingQueue();
            _networkSender = new NetworkSender(_sendingQueue);
            _sendingLoop = new NetworkSendingLoop(_sendingQueue, _client);

            //Handling Instances
            _packetHandler = new PacketHandler();
            _dataHandler = new DataHandler();

            //Handling Variables
            _isRunning = true;
            _packetHandlingThread = new Thread(PacketHandlingLoop);
            _packetHandlingThread.Start();

            StartReceiveLoop();

            Logger.Log("Server started\n");
        }

        private void StartReceiveLoop()
        {
            Logger.Log("Receive loop started...\n");

            Task.Factory.StartNew(async () =>
            {
                while (_isRunning)
                {
                    //Get and queue the received data for handling
                    Received receivedData = await Receive();
                    _packetHandler.QueuePacket(receivedData.Data, receivedData.Sender);
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
                    ClientNetworkPackage package = _packetHandler.PacketQueue.Dequeue();

                    if(_dataHandler.Packets.TryGetValue((int)package.PacketType, out DataHandler.Packet packet))
                    {
                        packet?.Invoke(package);
                    }
                }
            }
        }

        public void Dispose()
        {
            //Data Instances
            _sendingQueue.Dispose();
            _packetHandler.Dispose();

            //Handling Variables
            _isRunning = false;
            _packetHandlingThread.Abort();

            _client.Dispose();
        }
    }

    public struct Received
    {
        public IPEndPoint Sender;
        public byte[] Data;
    }
}
