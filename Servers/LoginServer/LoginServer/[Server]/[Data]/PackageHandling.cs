using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

using BPS.LoginServer.Utility;

namespace BPS.LoginServer.DataHandling
{
    public class PackageHandling
    {
        private ByteBuffer _buffer;
        //private ClientNetworkPackage _networkPackage;

        private Queue<ClientNetworkPackage> _packages;
        public Queue<ClientNetworkPackage> PackageQueue
        {
            get => _packages;
            set => _packages = value;
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

            PackageQueue.Enqueue(new ClientNetworkPackage((PacketType)packetID, socket, _buffer.ReadBytes(_buffer.Length())));

            _buffer.Clear();
        }

        public bool HasPackets()
        {
            return (_packages.Count > 0);
        }
    }
}
