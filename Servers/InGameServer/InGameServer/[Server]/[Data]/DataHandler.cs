using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BPS.InGameServer.Utility;
using BPS.InGameServer.Sending;

namespace BPS.InGameServer.DataHandling
{
    class DataHandler
    {
        public delegate void Packet(ClientNetworkPackage package);

        private Dictionary<int, Packet> _packets;
        public Dictionary<int, Packet> Packets { get => _packets; set => _packets = value; }

        private ByteBuffer _buffer;

        public DataHandler()
        {
            _packets = new Dictionary<int, Packet>();
            _buffer = new ByteBuffer();

            SetupNetworkPackets();
        }

        private void SetupNetworkPackets()
        {
        }
    }
}
