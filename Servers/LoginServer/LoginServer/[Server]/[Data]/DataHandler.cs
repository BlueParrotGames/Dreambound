using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            Packets.Add((int)PacketType.DataRequest, HandleDataRequest);
        }

        private void HandleLoginRequest(ClientNetworkPackage package)
        {
            LoginData loginData = UserDatabase.Login(package.Data);

            _buffer.Clear();

            //Check if the given username has already been used to succesfully log in
            if (Server.Instance.IsUserAlreadyOnline(loginData.Username))
            {
                _buffer.WriteInt((int)PacketType.LoginResponse);
                _buffer.WriteInt((int)LoginState.UserAlreadyLoggedIn);
                _buffer.WriteString("");
                _buffer.WriteInt(0);
                _buffer.WriteInt(0);
            }
            else
            {
                _buffer.WriteInt((int)PacketType.LoginResponse);
                _buffer.WriteInt((int)loginData.LoginState);
                _buffer.WriteString(loginData.Username);
                _buffer.WriteInt(loginData.UserId);
                _buffer.WriteInt((int)loginData.GamePerks);

                //Set the username of the client
                Server.Instance.ConnectedUsers.Add(loginData.Username, loginData.UserId);
                Server.Instance.ConnectedClients[package.Socket].Username = loginData.Username;
            }

            //Send the packet
            NetworkSender.SendPacket(_buffer, package.Socket);
        }
        private void HandleDataRequest(ClientNetworkPackage package)
        {
            Console.WriteLine("sending data response!");
        }
    }
}
