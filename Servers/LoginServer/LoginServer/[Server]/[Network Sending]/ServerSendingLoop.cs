using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
            if (_sendingQueue.HasPackets())
            {
                while (_isSending)
                {
                    SendingData sendableData = _sendingQueue.SendingQueue.Dequeue();
                    byte[] bytes = sendableData.Buffer.ToArray();
                    int length = bytes.Length;

                    //send the data
                    sendableData.Receiver.BeginSend(bytes, 0, length, SocketFlags.None, new AsyncCallback(SendCallback), null);
                }
            }
        }

        private void SendCallback(IAsyncResult result)
        {

        }
    }
}
