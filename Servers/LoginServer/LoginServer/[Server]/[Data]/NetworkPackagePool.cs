using System.Collections.Generic;
using System.Net.Sockets;

using BPS.LoginServer.Utility;

namespace BPS.LoginServer.DataHandling
{
    public class NetworkPackagePool
    {
        private Queue<ClientNetworkPackage> _packageQueue;

        public NetworkPackagePool()
        {
            _packageQueue = new Queue<ClientNetworkPackage>();
        }

        public ClientNetworkPackage GetPackage(PacketType packet, Socket socket, byte[] data)
        {
            if (_packageQueue.Count == 0)
                CreateNewPackage();

            ClientNetworkPackage package = _packageQueue.Dequeue();
            package.Packet = packet;
            package.Socket = socket;
            package.Data = data;

            return package;
        }
        public void ReturnPackage(ClientNetworkPackage package)
        {
            _packageQueue.Enqueue(package);
        }

        private void CreateNewPackage()
        {
            _packageQueue.Enqueue(new ClientNetworkPackage(PacketType.LoginRequest, null, null, this));
        }
    }
}
