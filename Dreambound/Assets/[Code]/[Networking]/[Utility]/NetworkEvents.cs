using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Dreambound.Networking.LoginSystem;

namespace Dreambound.Networking.Utility
{
    public class NetworkEvents
    {
        private static NetworkEvents _instance;
        public static NetworkEvents Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new NetworkEvents();

                return _instance;
            }
        }

        //Events
        public event Action<LoginState> OnLoginAttempt;
        public event Action<string> OnHashReceived;

        //Register Functions
        public void RegisterLoginAttempt(LoginState loginState)
        {
            OnLoginAttempt?.Invoke(loginState);
        }

        public void RegisterHashReceived(string Hash)
        {
            OnHashReceived?.Invoke(Hash);
        }
    }
}

