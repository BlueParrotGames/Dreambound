using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;

using Dreambound.Networking.Utility;

namespace Dreambound.Networking.Data
{
    public interface INetworkPackage
    {

    }

    public struct ClientNetworkPackage : INetworkPackage
    {
        public readonly PacketType PacketType;
        public readonly IPEndPoint EndPoint;
        public readonly byte[] Data;

        public ClientNetworkPackage(PacketType packet, IPEndPoint endPoint, byte[] data)
        {
            PacketType = packet;
            EndPoint = endPoint;
            Data = data;
        }
    }
}
