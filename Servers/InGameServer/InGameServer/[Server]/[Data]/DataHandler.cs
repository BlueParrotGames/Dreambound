using System.Collections.Generic;

using BPS.InGameServer.Utility;
using BPS.InGameServer.Sending;
using BPS.PipeLine.Sending;

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
            Packets.Add((int)PacketType.AccountInfo, HandleAccountInfo);
        }

        private void HandleAccountInfo(ClientNetworkPackage package)
        {
            _buffer.Clear();
            _buffer.WriteBytes(package.Data);

            //get the username and id to update the UserList
            string username = _buffer.ReadString();
            int id = _buffer.ReadInt();
            Server.Instance.UpdateUserList(username, id, true);

            //Rewrite the username, id and write an extra bool to send through the pipeline
            _buffer.WriteString(username);
            _buffer.WriteInt(id);
            _buffer.WriteBool(true);

            PipeSender.SendPacket(_buffer);

            package.ReturnToPool();
        }
    }
}
