using System;
using System.Threading;
using System.Net.Sockets;

namespace BPS.LoginServer.Sending
{
    class ServerSendingLoop
    {
        private readonly NetworkSendingQueue _sendingQueue;
        private readonly Server _server;
        private readonly bool _isSending;

        public ServerSendingLoop(Server server, NetworkSendingQueue queue)
        {
            _sendingQueue = queue;
            _isSending = true;
            _server = server;

            Thread sendingThreadOne = new Thread(CoreLoop);
            sendingThreadOne.Start();
        }

        private void CoreLoop()
        {
            while (_isSending)
            {
                if (_sendingQueue.HasPackets())
                {
                    SendingData sendableData = _sendingQueue.SendingQueue.Dequeue();

                    sendableData.Receiver.BeginSend(sendableData.Buffer.ToArray(), 0, sendableData.Buffer.Count(), SocketFlags.None, new AsyncCallback(SendCallback), sendableData.Receiver);
                }
            }
        }

        private void SendCallback(IAsyncResult result)
        {
            Socket socket = (Socket)result.AsyncState;
            socket.EndSend(result);
        }
    }
}
