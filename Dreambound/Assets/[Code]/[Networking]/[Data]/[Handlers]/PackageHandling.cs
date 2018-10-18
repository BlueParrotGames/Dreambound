using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

using Dreambound.Networking.Utility;

namespace Dreambound.Networking.DataHandling
{
    public class PackageHandling
    {
        private ByteBuffer _buffer;
        private ClientNetworkPackage _networkPackage;

        private Queue<ClientNetworkPackage> _packages;
        public Queue<ClientNetworkPackage> PackageQueue
        {
            get { return _packages; }
            set { _packages = value; }
        }

        public PackageHandling()
        {
            _packages = new Queue<ClientNetworkPackage>();
            _buffer = new ByteBuffer();
        }

        public void QueuePackage(byte[] data, Socket socket)
        {
            _buffer.WriteBytes(data);
            int packetSize = _buffer.ReadInt();
            int packetID = _buffer.ReadInt();

            PackageQueue.Enqueue(new ClientNetworkPackage((PacketType)packetID, socket, data));

            _buffer.Clear();
        }

        public bool HasPackets()
        {
            return (_packages.Count > 0);
        }
    }
}
