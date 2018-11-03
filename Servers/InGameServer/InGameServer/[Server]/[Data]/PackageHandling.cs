using System.Collections.Generic;
using System.Net.Sockets;

using BPS.InGameServer.Utility;

namespace BPS.InGameServer.DataHandling
{
    public class PackageHandling
    {
        private ByteBuffer _buffer;
        private NetworkPackagePool _packagePool;

        private Queue<ClientNetworkPackage> _packages;
        public Queue<ClientNetworkPackage> PackageQueue
        {
            get => _packages;
            set => _packages = value;
        }

        public PackageHandling()
        {
            _packages = new Queue<ClientNetworkPackage>();
            _packagePool = new NetworkPackagePool();
            _buffer = new ByteBuffer();
        }

        public void QueuePackage(byte[] data, Socket socket)
        {
            _buffer.WriteBytes(data);
            int packetSize = _buffer.ReadInt();
            int packetID = _buffer.ReadInt();

            PackageQueue.Enqueue(_packagePool.GetPackage((PacketType)packetID, socket, _buffer.ReadBytes(_buffer.Length())));

            _buffer.Clear();
        }

        public bool HasPackets()
        {
            return (_packages.Count > 0);
        }
    }
}
