using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dreambound.Networking
{
    public class LoginManager : MonoBehaviour
    {
        [SerializeField] private string _username;
        [SerializeField] private string _password;

        [SerializeField] private string _ipAddress;
        [SerializeField] private int _port;

        private NetworkHandler _networkHandler;

        private void Awake()
        {
            _networkHandler = new NetworkHandler();
        }
        private void Start()
        {
            _networkHandler.ConnectUsingSettings(_ipAddress, _port);
        }
    }
}
