using System;
using System.Threading;
using System.Net.Sockets;

namespace Dreambound.Networking.Data.Sending
{
    public class NetworkSendingLoop : IDisposable
    {
        private readonly NetworkSendingQueue _sendingQueue;
        private UdpClient _client;

        private readonly bool _isSending;
        private Thread _sendingThreadOne;

        public NetworkSendingLoop(NetworkSendingQueue sendingQueue, UdpClient client)
        {
            _sendingQueue = sendingQueue;
            _client = client;

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
                    _client.Send(sendableData.Buffer, sendableData.ByteLength, sendableData.Receiver);
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
