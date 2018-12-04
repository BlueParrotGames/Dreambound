using System;
using System.Threading;
using System.Net.Sockets;

namespace Dreambound.Networking.DataHandling
{
    public class NetworkSendingLoop : IDisposable
    {
        private readonly NetworkSendingQueue _sendingQueue;
        private readonly bool _isSending;

        private Thread _sendingThreadOne;

        public NetworkSendingLoop(NetworkSendingQueue sendingQueue)
        {
            _sendingQueue = sendingQueue;
            _isSending = true;

            _sendingThreadOne = new Thread(CoreLoop);
            _sendingThreadOne.Start();
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

        public void Dispose()
        {
            _sendingThreadOne.Abort();
        }
    }
}
