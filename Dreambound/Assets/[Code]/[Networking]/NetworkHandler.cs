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
        Socket socket;

        public void ConnectUsingSettings(string ip, int port)
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

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

        public void Login(string username, string password, string email)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteInt((int)PacketType.LoginRequest);
            buffer.WriteString(username);
            buffer.WriteString(password);
            buffer.WriteString(email);

            NetworkSender.SendPacket(buffer, socket);
        }
    }
}