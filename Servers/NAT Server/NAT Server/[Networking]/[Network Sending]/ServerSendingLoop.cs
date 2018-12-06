using System;
using System.Threading;
using System.Net.Sockets;

using BPG.Debugging;

namespace BPG.NATServer.Sending
{
    class ServerSendingLoop
    {
        private readonly NetworkSendingQueue _sendingQueue;
        private readonly bool _isSending;

        public event Action<Socket> OnPacketSent;

        public ServerSendingLoop(NetworkSendingQueue queue)
        {
            _sendingQueue = queue;
            _isSending = true;

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

            Logger.Log("Packet successfuly sent", 2);

            OnPacketSent?.Invoke(socket);
        }
    }
}
