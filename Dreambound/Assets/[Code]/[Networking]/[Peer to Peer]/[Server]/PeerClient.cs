using System;
using System.Net.Sockets;

namespace Dreambound.Networking.PeerToPeer.Server
{
    public class PeerClient
    {
        private readonly PeerServer _server;
        private readonly byte[] _buffer;

        public string HashToken { get; set; }
        public string UserToken { get; set; }
        public Socket Socket { get; }

        public PeerClient(Socket socket, PeerServer server)
        {
            _server = server;
            Socket = socket;

            _buffer = new byte[4096];

            StartReceive();
        }

        private void StartReceive()
        {
            Socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), null);
        }
        private void ReceiveCallback(IAsyncResult result)
        {
            try
            {
                int readBytes = Socket.EndReceive(result);

                if (readBytes <= 0)
                {
                    //Received 0 bytes?
                    CloseConnection();
                    return;
                }

                byte[] tempBuffer = new byte[readBytes];
                Buffer.BlockCopy(_buffer, 0, tempBuffer, 0, readBytes);

                if (readBytes > 0)
                    GetPacketsOutOfStream(tempBuffer);

                StartReceive();
            }
            catch
            {
                //Received some kind of error
            }
        }

        private void GetPacketsOutOfStream(byte[] buffer)
        {
            int readPos = 0;
            int totalBytes = buffer.Length;

            while (readPos < totalBytes)
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
            _server.DisconnectSocket(Socket);
            Socket.Close();
        }
    }
}
