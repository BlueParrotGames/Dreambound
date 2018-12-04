using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Threading;

namespace Dreambound.Networking.DataHandling
{
    public class DataManager : IDisposable
    {
        private PackageHandling _packetHandler;
        private DataHandler _dataHandler;

        //Sending instances
        private NetworkSender _networkSender;
        private NetworkSendingQueue _sendingQueue;
        private NetworkSendingLoop _sendingLoop;

        Thread _packetHandlingThread;

        public PackageHandling PacketHandler
        {
            get { return _packetHandler; }
            set { _packetHandler = value; }
        }

        private bool _isRunning;

        public DataManager()
        {
            PacketHandler = new PackageHandling();
            _dataHandler = new DataHandler();

            _sendingQueue = new NetworkSendingQueue();
            _networkSender = new NetworkSender(_sendingQueue);
            _sendingLoop = new NetworkSendingLoop(_sendingQueue);

            _isRunning = true;

            _packetHandlingThread = new Thread(PacketHandlingLoop);
            _packetHandlingThread.Start();
        }

        private void PacketHandlingLoop()
        {
            while (_isRunning)
            {
                if (_packetHandler.HasPackets())
                {
                    ClientNetworkPackage package = _packetHandler.PackageQueue.Dequeue();

                    DataHandler.Packet packet;
                    if(_dataHandler.Packets.TryGetValue((int)package.Packet, out packet))
                    {
                        if(packet != null)
                        {
                            packet.Invoke(package);
                        }
                    }
                }

                Thread.Sleep(10);
            }
        }

        public void Dispose()
        {
            _packetHandlingThread.Abort();
        }
    }
}
