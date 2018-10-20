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
        }
        private void HandleLoginResponse(ClientNetworkPackage package)
        {
            _byteBuffer.Clear();

            _byteBuffer.WriteBytes(package.Data);
            int length = _byteBuffer.ReadInt();
            PacketType type = (PacketType)_byteBuffer.ReadInt();

            LoginData loginData = new LoginData((LoginState)_byteBuffer.ReadInt(), _byteBuffer.ReadString(), _byteBuffer.ReadInt(), (GamePerks)_byteBuffer.ReadInt());

            UserData.SetUserData(loginData);
        }
        private void HandleDataResponse(ClientNetworkPackage package)
        {
            Debug.Log("Data response received");
        }
    }
}