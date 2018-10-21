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
        public event Action<object, object> OnLoginAttempt;
        public event Action<string> OnHashReceived;

        //Register Functions
        public void RegisterLoginAttempt(LoginState loginState, int id)
        {
            OnLoginAttempt?.Invoke(loginState, id);
        }
        public void RegisterHashReceived(string Hash)
        {
            OnHashReceived?.Invoke(Hash);
        }
    }
}

