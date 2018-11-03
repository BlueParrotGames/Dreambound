using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using LoginState = Dreambound.Networking.LoginSystem.LoginState;
using Dreambound.Networking.Utility;
using Dreambound.Networking.Threading;

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

        public void Login(string Email, string Password)
        {
            _networkHandler.Login(Email, Password);
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