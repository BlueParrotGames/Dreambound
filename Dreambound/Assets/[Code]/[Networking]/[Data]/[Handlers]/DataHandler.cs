using System.Text;
using System.Collections.Generic;

using UnityEngine;

using Dreambound.Networking.Utility;

namespace Dreambound.Networking.Data.Handlers
{
    class DataHandler
    {
        public delegate void Packet(ClientNetworkPackage package);

        private Dictionary<int, Packet> _packets;
        public Dictionary<int, Packet> Packets
        {
            get { return _packets; }
        }

        private ByteBuffer _byteBuffer;

        public DataHandler()
        {
            _packets = new Dictionary<int, Packet>();
            _byteBuffer = new ByteBuffer();

            SetupNetworkPackets();
        }
        private void SetupNetworkPackets()
        {
            _packets.Add((int)PacketType.Message, HandleServerTestMessage);
        }
        
        private void HandleServerTestMessage(ClientNetworkPackage package)
        {
            _byteBuffer.Clear();
            _byteBuffer.WriteBytes(package.Data);

            Debug.Log(_byteBuffer.ReadString());
        }
    }
}