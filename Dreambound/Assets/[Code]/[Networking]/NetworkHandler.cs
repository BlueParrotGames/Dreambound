using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

using Dreambound.Networking.DataHandling;
using Dreambound.Networking.Utility;

namespace Dreambound.Networking
{
    public class NetworkHandler
    {
        private Socket _socket;
        private bool _connected = false;

        public void ConnectUsingSettings(string ip, int port)
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                _socket.Connect(ip, port);
            }
            catch
            {
                Debug.LogWarning("Connection could not be made");
            }

            if (_socket.Connected)
            {
                Client client = new Client(_socket, new DataManager());
                _connected = true;
            }
        }

        public void Login(string username, string password)
        {
            if (!_connected)
                return;

            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteInt((int)PacketType.LoginRequest);
            buffer.WriteString(username);
            buffer.WriteString(password);

            NetworkSender.SendPacket(buffer, _socket);
        }
    }
}