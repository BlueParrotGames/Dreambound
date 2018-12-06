using System;
using System.Net;
using System.Text;
using System.Net.Sockets;

using UnityEngine;

using Dreambound.Networking.Utility;

namespace Dreambound.Networking.P2P
{
    public class NATHole
    {
        private static NATHole _instance;
        public static NATHole Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new NATHole();

                return _instance;
            }
        }

        private Socket _workSocket;
        private byte[] _buffer = new byte[1024];

        private Socket _returnSocket;

        public static Socket Punch(string ip, int port)
        {
            Instance._workSocket?.Close();
            Instance._workSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                Instance._workSocket.Connect(ip, port);
            }
            catch
            {
                Debug.LogWarning("Could not connect to the NAT Server");
            }

            if (Instance._workSocket.Connected)
                Instance._workSocket.BeginReceive(Instance._buffer, 0, Instance._buffer.Length, SocketFlags.None, new AsyncCallback(Instance.ReceiveCallback), null);
            
            return Instance._returnSocket;
        }

        private void ReceiveCallback(IAsyncResult result)
        {
            try
            {
                int byteLenght = Instance._workSocket.EndReceive(result);

                byte[] data = new byte[byteLenght];
                Buffer.BlockCopy(_buffer, 0, data, 0, byteLenght);

                ByteBuffer byteBuffer = new ByteBuffer();
                byteBuffer.WriteBytes(data);

                byteBuffer.ReadInt();
                byteBuffer.ReadInt();
                string[] response = byteBuffer.ReadString().Split(':');

                string ip = response[0];
                int port = Int32.Parse(response[1]);

                string endPoint = response[0] + ":" + response[1];
                Debug.Log(endPoint);

                _returnSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                _returnSocket.Bind(new IPEndPoint(IPAddress.Parse(ip), port));
            }
            catch
            {
            }
        }
    }
}