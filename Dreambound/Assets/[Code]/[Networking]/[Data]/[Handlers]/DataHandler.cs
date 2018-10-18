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

        public DataHandler()
        {
            _packets = new Dictionary<int, Packet>();

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
            Debug.Log("Login response received");
        }
        private void HandleDataResponse(ClientNetworkPackage package)
        {
            Debug.Log("Data response received");
        }
    }
}