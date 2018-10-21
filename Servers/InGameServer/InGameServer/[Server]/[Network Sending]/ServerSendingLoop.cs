using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace BPS.InGameServer.Sending
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

                    sendableData.Receiver.BeginSend(sendableData.Buffer.ToArray(), 0, sendableData.Buffer.Count(), SocketFlags.None, new AsyncCallback(SendCallback), null);
                }
            }
        }

        private void SendCallback(IAsyncResult result)
        {
        }
    }
}
