using System.Collections;
using System.Net;
using System.Collections.Generic;

using UnityEngine;

using Dreambound.Networking.Utility;

namespace Dreambound.Networking.Data.Handlers
{
    public class PackageHandling
    {
        private ByteBuffer _buffer;

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

        public void QueuePackage(byte[] data, IPEndPoint endPoint)
        {
            if (_buffer == null)
                _buffer = new ByteBuffer();

            _buffer.Clear();
            _buffer.WriteBytes(data);

            int packetID = _buffer.ReadInt();

            PackageQueue.Enqueue(new ClientNetworkPackage((PacketType)packetID, endPoint, _buffer.ReadBytes(_buffer.Length())));
        }

        public bool HasPackets()
        {
            return (_packages.Count > 0);
        }
    }
}
