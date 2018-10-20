using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

using Dreambound.Networking.DataHandling;
using Dreambound.Networking.Utility;

namespace Dreambound.Networking
{
    class Client
    {
        private Socket _socket;

        private readonly byte[] _buffer;
        private readonly DataManager _dataManager;

        private string HashToken;

        public Client(Socket socket, DataManager dataManager)
        {
            _socket = socket;
            _dataManager = dataManager;

            _buffer = new byte[4096];

            NetworkEvents.Instance.OnHashReceived += UpdateHash;

            BeginReceive();
        }

        private void BeginReceive()
        {
            _socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), null);
        }
        private void ReceiveCallback(IAsyncResult result)
        {
            try
            {
                int readBytes = _socket.EndReceive(result);

                if (readBytes <= 0)
                {
                    //Received 0 bytes?
                    Debug.LogError("Received 0 bytes?");
                    CloseConnection();
                    return;
                }

                byte[] tempBuffer = new byte[readBytes];
                Buffer.BlockCopy(_buffer, 0, tempBuffer, 0, readBytes);

                if (readBytes > 0)
                    GetPacketsOutOfStream(tempBuffer);

                BeginReceive();
            }
            catch
            {
                Debug.LogError("Received some kind of error?!");
                CloseConnection();
            }

            Debug.Log("received package");
        }
        private void GetPacketsOutOfStream(byte[] buffer)
        {
            int readPos = 0;

            while(readPos < buffer.Length)
            {
                int packetLength = BitConverter.ToInt32(buffer, readPos) + sizeof(int);

                byte[] data = new byte[buffer.Length];
                Buffer.BlockCopy(buffer, readPos, data, 0, data.Length);

                _dataManager.PacketHandler.QueuePackage(data, _socket);

                readPos += data.Length;
            }
        }

        private void CloseConnection()
        {
            _socket.Close();
        }

        private void UpdateHash(string Hash)
        {
            HashToken = Hash;

            Debug.Log(HashToken);
        }
    }
}
