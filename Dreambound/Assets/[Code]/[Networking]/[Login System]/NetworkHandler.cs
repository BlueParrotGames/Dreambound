using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

namespace Dreambound.Networking
{
    public class NetworkHandler
    {
        private Socket _socket;

        public NetworkHandler()
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void ConnectUsingSettings(string ip, int port)
        {
            _socket.BeginConnect(ip, port, new AsyncCallback(ConnectCallback), null);
        }
        private void ConnectCallback(IAsyncResult result)
        {
            Debug.Log("Connection made!");
        }
    }
}