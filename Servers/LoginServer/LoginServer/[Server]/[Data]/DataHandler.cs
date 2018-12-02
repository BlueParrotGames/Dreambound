using System.Collections.Generic;

using BPS.LoginServer.Utility;
using BPS.User.Database;
using BPS.LoginServer.Sending;

namespace BPS.LoginServer.DataHandling
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
            Packets.Add((int)PacketType.LoginRequest, HandleLoginRequest);
        }

        private void HandleLoginRequest(ClientNetworkPackage package)
        {
            LoginData loginData = UserDatabase.Login(package.Data);
        
            _buffer.Clear();

            //Check if the given username has already been used to succesfully log in
            if (Server.Instance.UserAlreadyOnline(loginData.Username, loginData.UserId))
            {
                _buffer.WriteInt((int)PacketType.LoginResponse);
                _buffer.WriteInt((int)LoginState.UserAlreadyLoggedIn);
                _buffer.WriteString("");
                _buffer.WriteInt(0);
                _buffer.WriteBool(true);
                _buffer.WriteInt(0);
            }
            else
            {
                _buffer.WriteInt((int)PacketType.LoginResponse);
                _buffer.WriteInt((int)loginData.LoginState);
                _buffer.WriteString(loginData.Username);
                _buffer.WriteInt(loginData.UserId);
                _buffer.WriteBool(false);
                _buffer.WriteInt((int)loginData.GamePerks);

                if(loginData.LoginState == LoginState.SuccelfullLogin)
                {
                    string userString = loginData.Username + loginData.UserId.ToString("00000");

                    Server.Instance.UpdateUserList(userString, false);
                    Server.Instance.DisconnectSocket(package.Socket);
                }
            }

            //Send the packet
            NetworkSender.SendPacket(_buffer, package.Socket);

            //Tell the package to return itself to the NetworkPackagePool
            package.ReturnToPool();
        }
    }
}
