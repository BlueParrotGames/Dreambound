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
            Packets.Add((int)PacketType.Verification, HandleVerificationHash);
            Packets.Add((int)PacketType.LoginResponse, HandleLoginResponse);
            Packets.Add((int)PacketType.DataResponse, HandleDataResponse);
        }

        private void HandleVerificationHash(ClientNetworkPackage package)
        {
            Debug.Log("Verification Hash received");

            _byteBuffer.Clear();
            _byteBuffer.WriteBytes(package.Data);

            NetworkEvents.Instance.RegisterHashReceived(_byteBuffer.ReadString());
        }
        private void HandleLoginResponse(ClientNetworkPackage package)
        {
            _byteBuffer.Clear();

            //Convert the ByteBuffer to LoginData
            _byteBuffer.WriteBytes(package.Data);
            LoginData loginData = new LoginData((LoginState)_byteBuffer.ReadInt(), _byteBuffer.ReadString(), _byteBuffer.ReadInt(), (GamePerks)_byteBuffer.ReadInt());

            //Register the event to the event handler
            NetworkEvents.Instance.RegisterLoginAttempt(loginData.LoginState);

            //Update the UserData because we successfully logged in
            if (loginData.LoginState == LoginState.SuccelfullLogin)
                UserData.SetUserData(loginData);

            Debug.Log("Login response received");
        }
        private void HandleDataResponse(ClientNetworkPackage package)
        {
            Debug.Log("Data response received");
        }
    }
}