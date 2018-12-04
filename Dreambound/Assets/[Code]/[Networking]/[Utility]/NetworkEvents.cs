using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        public event Action<string> OnHashReceived;

        public void RegisterHashReceived(string Hash)
        {
            OnHashReceived?.Invoke(Hash);
        }
    }
}

