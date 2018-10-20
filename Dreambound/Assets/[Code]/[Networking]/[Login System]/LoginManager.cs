using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using LoginState = Dreambound.Networking.LoginSystem.LoginState;
using Dreambound.Networking.Utility;
using Dreambound.Networking.Threading;

namespace Dreambound.Networking.LoginSystem
{
    public class LoginManager : ThreadObject
    {
        [SerializeField] private string _ipAddress;
        [SerializeField] private int _port;

        [Header("UI Properties")]
        [SerializeField] private InputField _usernameText;
        [SerializeField] private InputField _passwordText;
        [SerializeField] private InputField _emailText;
        [SerializeField] private Text _feedbackText;

        private NetworkHandler _networkHandler;
        private bool _connected = false;

        private void Awake()
        {
            //Events settings
            SetFunction(UpdateFeedBackText);
            NetworkEvents.Instance.OnLoginAttempt += Listener;

            _networkHandler = new NetworkHandler();
        }
        private void Start()
        {
            _networkHandler.ConnectUsingSettings(_ipAddress, _port);
        }

        public void Login()
        {

            //_networkHandler.Login(_usernameText.text, _passwordText.text, _emailText.text);
            _networkHandler.Login("test", "test", "test");
        }

        private void UpdateFeedBackText(object loginState)
        {
            switch ((LoginState)loginState)
            {
                case LoginState.CannotConnectToDatabase:

                    _feedbackText.text = "Could not connect to user database!";
                    break;
                case LoginState.SuccelfullLogin:

                    _feedbackText.text = "Succesfully logged in!";
                    break;
                case LoginState.UserAlreadyLoggedIn:

                    _feedbackText.text = "Someone is already logged in with this account";
                    break;
                case LoginState.wrongUsernameOrPassword:

                    _feedbackText.text = "Password, Username or Email is incorrect";
                    break;
            }
        }
    }
}
