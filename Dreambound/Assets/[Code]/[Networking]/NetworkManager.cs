using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dreambound.Networking
{
    public class NetworkManager : MonoBehaviour
    {
        private NetworkHandler _networkHandler;

        private void Awake()
        {
            DontDestroyOnLoad(this.gameObject);

            _networkHandler = new NetworkHandler();
        }

        public void ConnectUsingSettings(string ip, int port)
        {
            _networkHandler.ConnectUsingSettings(ip, port);
        }

        public void SendAccountInfo()
        {
            _networkHandler.SendAccountInfo();
        }

        public void Destroy()
        {
            Destroy(this.gameObject);
        }
    }
}