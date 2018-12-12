using System;
using System.Net;

namespace BPG.NATServer.Data
{
    public interface INetworkPackage : IDisposable
    {
    }

    public struct ClientNetworkPackage : INetworkPackage
    {
        public PacketType PacketType { get; internal set; }
        public IPEndPoint EndPoint { get; internal set; }
        public byte[] Data { get; internal set; }

        public ClientNetworkPackage(PacketType packetType, IPEndPoint endPoint, byte[] data)
        {
            PacketType = packetType;
            EndPoint = endPoint;
            Data = data;
        }

        public void Dispose()
        {
            Data = null;
        }
    }
}
