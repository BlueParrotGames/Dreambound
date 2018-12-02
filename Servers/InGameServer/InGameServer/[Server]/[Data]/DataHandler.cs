using System.Collections.Generic;

using BPS.PipeLine.Sending;

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
            Packets.Add((int)PacketType.AccountInfo, HandleAccountInfo);
            Packets.Add((int)PacketType.LogoutRequest, HandleLogoutRequest);
        }

        private void HandleAccountInfo(ClientNetworkPackage package)
        {
            _buffer.Clear();
            _buffer.WriteBytes(package.Data);

            Server.Instance.UpdateUserList(_buffer.ReadString(), _buffer.ReadInt(), false);
        }
        private void HandleLogoutRequest(ClientNetworkPackage package)
        {
            _buffer.Clear();
            _buffer.WriteBytes(package.Data);

            string username = _buffer.ReadString();
            int id = _buffer.ReadInt();

            Server.Instance.UpdateUserList(username, id, true);

            _buffer.Clear();
            _buffer.WriteInt((int)PacketType.LogoutResponse);
            _buffer.WriteString(username + id.ToString("00000"));

            PipeSender.SendPacket(_buffer);
            NetworkSender.SendPacket(_buffer, package.Socket);
        }

        private void HandleOnlineFriendsRequest(ClientNetworkPackage package)
        {
            _buffer.Clear();
            //_buffer.WriteBytes(package.Data);

            _buffer.WriteInt((int)PacketType.OnlineFriendsResponse);
            string[] userFriends = { "test2#2" };
            for(int i = 0; i < userFriends.Length; i++)
            {
                _buffer.WriteString(userFriends[i]);
            }

            NetworkSender.SendPacket(_buffer, package.Socket);
        }
    }
}
