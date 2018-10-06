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
        private Queue<ClientNetworkPackage> _packages;
        public Queue<ClientNetworkPackage> PackageQueue
        {
            get => _packages;
            set => _packages = value;
        }

        public PackageHandling()
        {
            _packages = new Queue<ClientNetworkPackage>();
        }

        public void QueuePackage(byte[] data, Socket socket)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            int packetSize = buffer.ReadInt();
            int packetID = buffer.ReadInt();

            PackageQueue.Enqueue(new ClientNetworkPackage((PacketType)packetID, socket, data));
        }

        public bool HasPackets()
        {
            return (_packages.Count > 0);
        }
    }
}
