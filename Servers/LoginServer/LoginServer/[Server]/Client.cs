using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

using BPS.Debugging;

namespace BPS.LoginServer
{
    class Client
    {
        private readonly Server _server;
        private readonly byte[] _buffer;

        public string HashToken { get; set; }
        public string Username { get; set; }

        public Socket Socket { get; }

        public Client(Socket socket, Server server)
        {
            _server = server;
            Socket = socket;

            _buffer = new byte[1024];

            BeginReceive();
        }

        private void BeginReceive()
        {
            Socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), Socket);
        }
        private void ReceiveCallback(IAsyncResult result)
        {
            try
            {
                int readBytes = Socket.EndReceive(result);

                if (readBytes <= 0)
                {
                    Logger.LogError("Received 0 bytes?");
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
                Logger.LogError("Received some kind of error?!");
                CloseConnection();
            }
        }
        private void GetPacketsOutOfStream(byte[] buffer)
        {
            int readPos = 0;
            int totalBytes = buffer.Length;

            while(readPos < totalBytes)
            {
                int packetLenght = BitConverter.ToInt32(buffer, readPos) + sizeof(int);
                byte[] data = new byte[packetLenght];
                Buffer.BlockCopy(buffer, readPos, data, 0, packetLenght);

                _server.PacketHandler.QueuePackage(data, Socket);

                readPos += packetLenght;
            }
        }

        private void CloseConnection()
        {
            //Logger.Warn("A client has disconnected (IP: " + Socket.RemoteEndPoint + ")");

            Socket.Close();
            _server.DisconnectPlayer(Socket);
        }
    }
}
