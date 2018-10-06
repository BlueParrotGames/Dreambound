using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

using BPS.LoginServer.Utility;

namespace BPS.LoginServer.DataHandling
{
    public interface INetworkPackage
    {

    }

    public struct ClientNetworkPackage : INetworkPackage
    {
        public PacketType Packet { get; }
        public Socket Socket { get; }
        public byte[] Data { get; }

        public ClientNetworkPackage(PacketType packet, Socket socket, byte[] data)
        {
            Packet = packet;
            Socket = socket;
            Data = data;
        }
    }
}
