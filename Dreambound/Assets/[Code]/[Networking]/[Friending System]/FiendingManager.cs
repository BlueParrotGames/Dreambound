using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dreambound.Networking.FriendNetworking
{
    public class FiendingManager : MonoBehaviour
    {
        [SerializeField] private string _connectionIp;
        [SerializeField] private int _connectionPort;

        private NetworkManager _networkManager;

        private void Start()
        {
            _networkManager = FindObjectOfType<NetworkManager>();
            _networkManager.ConnectUsingSettings(_connectionIp, _connectionPort);

            PeerToPeer.Server.NATHole.Punch();

            SendAccountInfo();
        }

        private void SendAccountInfo()
        {
            _networkManager.SendAccountInfo();
        }
    }
}
