using System.Threading;
using System.Net.Sockets;

using BPG.Debugging;

namespace BPG.NATServer.Data.Sending
{
    public class NetworkSendingLoop
    {
        private UdpClient _client;
        private NetworkSendingQueue _sendingQueue;

        private bool _isRunning;
        private Thread _sendingLoopThread;

        public NetworkSendingLoop(NetworkSendingQueue sendingQueue, UdpClient client)
        {
            _sendingQueue = sendingQueue;
            _client = client;

            _isRunning = true;
            _sendingLoopThread = new Thread(SendingLoop);
            _sendingLoopThread.Start();
        }

        private void SendingLoop()
        {
            Logger.Log("Sending loop started...");     

            while (_isRunning)
            {
                if (_sendingQueue.HasPackets())
                {
                    SendingData sendingData = _sendingQueue.SendingQueue.Dequeue();
                    _client.Send(sendingData.Buffer, sendingData.BufferLength, sendingData.ReceiveEndpoint);
                }
            }
        }
    }
}
