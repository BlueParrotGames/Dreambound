using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Dreambound.Networking.Utility;

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

        //In-Game Server
        private void HandleOnlineFriendsResponse(ClientNetworkPackage package)
        {
            Debug.Log("Online friends response received");
        }
    }
}