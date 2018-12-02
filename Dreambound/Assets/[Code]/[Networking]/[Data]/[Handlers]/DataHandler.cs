using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Dreambound.Networking.Utility;
using Dreambound.Networking.LoginSystem;

namespace Dreambound.Networking.DataHandling
{
    class DataHandler
    {
        public delegate void Packet(ClientNetworkPackage package);

        private Dictionary<int, Packet> _packets;
        public Dictionary<int, Packet> Packets
        {
            get { return _packets; }
            set { _packets = value; }
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
            //General
            Packets.Add((int)PacketType.Verification, HandleVerificationHash);

            //Login Server
            Packets.Add((int)PacketType.LoginResponse, HandleLoginResponse);

            //In-Game Server
            Packets.Add((int)PacketType.OnlineFriendsResponse, HandleOnlineFriendsResponse);
        }

        //General
        private void HandleVerificationHash(ClientNetworkPackage package)
        {
            _byteBuffer.Clear();
            _byteBuffer.WriteBytes(package.Data);

            NetworkEvents.Instance.RegisterHashReceived(_byteBuffer.ReadString());

            Debug.Log("Verification Hash received");
        }

        //Login Server
        private void HandleLoginResponse(ClientNetworkPackage package)
        {
            _byteBuffer.Clear();

            //Convert the ByteBuffer to LoginData
            _byteBuffer.WriteBytes(package.Data);
            LoginData loginData = new LoginData((LoginState)_byteBuffer.ReadInt(), _byteBuffer.ReadString(), _byteBuffer.ReadInt(), (GamePerks)_byteBuffer.ReadInt());

            //Register the event to the event handler
            NetworkEvents.Instance.RegisterLoginAttempt(loginData.LoginState, (int)loginData.GamePerks);

            //Update the UserData because we successfully logged in
            if (loginData.LoginState == LoginState.SuccelfullLogin && loginData.GamePerks > 0)
                UserData.SetUserData(loginData);

            Debug.Log("Login response received");
        }

        //In-Game Server
        private void HandleOnlineFriendsResponse(ClientNetworkPackage package)
        {
            Debug.Log("Online friends response received");
        }
    }
}