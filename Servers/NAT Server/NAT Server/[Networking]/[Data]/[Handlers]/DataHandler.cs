using System.Text;
using System.Collections.Generic;

using BPG.Debugging;
using BPG.NATServer.Utility;
using BPG.NATServer.Data.Sending;

namespace BPG.NATServer.Data.Handlers
{
    public class DataHandler
    {
        public delegate void Packet(ClientNetworkPackage packet);

        public Dictionary<int, Packet> Packets { get; internal set; }
        private ByteBuffer _buffer;

        public DataHandler()
        {
            Packets = new Dictionary<int, Packet>();
            _buffer = new ByteBuffer();

            SetupNetworkPackets();
        }

        private void SetupNetworkPackets()
        {
            Packets.Add((int)PacketType.Message, HandleMessage);
        }

        private void HandleMessage(ClientNetworkPackage package)
        {
            _buffer.Clear();
            _buffer.WriteBytes(package.Data);

            Logger.Log(Encoding.ASCII.GetString(_buffer.ReadBytes(_buffer.Length())));
            Logger.Log(package.EndPoint);

            _buffer.Clear();
            _buffer.WriteInt((int)PacketType.Message);
            _buffer.WriteString("this is a response from the server");

            Logger.Log(_buffer.ToArray().Length);

            NetworkSender.SendToOne(_buffer.ToArray(), package.EndPoint);
        }
    }
}
