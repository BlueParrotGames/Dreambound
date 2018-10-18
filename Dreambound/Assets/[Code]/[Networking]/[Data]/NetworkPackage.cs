using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;

using Dreambound.Networking.Utility;

namespace Dreambound.Networking.DataHandling
{
    public interface INetworkPackage
    {

    }

    public struct ClientNetworkPackage : INetworkPackage
    {
        public readonly PacketType Packet;
        public readonly Socket Socket;
        public readonly byte[] Data;

        public ClientNetworkPackage(PacketType packet, Socket socket, byte[] data)
        {
            Packet = packet;
            Socket = socket;
            Data = data;
        }
    }
}
