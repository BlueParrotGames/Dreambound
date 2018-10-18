using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

namespace Dreambound.Networking
{
    public class NetworkHandler
    {
        public void ConnectUsingSettings(string ip, int port)
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                socket.Connect(ip, port);
            }
            catch
            {
                Debug.LogWarning("Connection could not be made");
            }

            if (socket.Connected)
            {
                Client client = new Client(socket, new DataHandling.DataManager());
            }
        }
    }
}