using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BPS.LoginServer.Utility;

namespace BPS.LoginServer.DataHandling
{
    class DataHandler
    {
        public delegate void Packet(ClientNetworkPackage package);

        private Dictionary<int, Packet> _packets;
        public Dictionary<int, Packet> Packets { get => _packets; set => _packets = value; }

        public DataHandler()
        {
            _packets = new Dictionary<int, Packet>();

            SetupNetworkPackets();
        }

        private void SetupNetworkPackets()
        {
            Packets.Add((int)PacketType.LoginRequest, SendLoginResponse);
            Packets.Add((int)PacketType.DataRequest, SendDataResponse);
        }
        private void SendLoginResponse(ClientNetworkPackage package)
        {
            Console.WriteLine("sending login response!");
        }
        private void SendDataResponse(ClientNetworkPackage package)
        {
            Console.WriteLine("sending data response!");
        }
    }
}
