using System;
using System.Net.Sockets;

using BPS.LoginServer.Utility;

namespace BPS.LoginServer.DataHandling
{
    public interface INetworkPackage
    {
        void ReturnToPool();
    }

    public struct ClientNetworkPackage : INetworkPackage
    {
        public PacketType Packet { get; internal set; }
        public Socket Socket { get; internal set; }
        public byte[] Data { get; internal set; }

        private NetworkPackagePool _masterPool;

        public ClientNetworkPackage(PacketType packet, Socket socket, byte[] data, NetworkPackagePool masterPool)
        {
            Packet = packet;
            Socket = socket;
            Data = data;
            _masterPool = masterPool;
        }

        public void ReturnToPool()
        {
            _masterPool.ReturnPackage(this);
        }
    }
}
