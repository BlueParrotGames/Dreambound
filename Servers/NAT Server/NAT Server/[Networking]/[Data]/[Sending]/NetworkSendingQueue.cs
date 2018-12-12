using System;
using System.Net;
using System.Collections.Generic;

namespace BPG.NATServer.Data.Sending
{
    public class NetworkSendingQueue : IDisposable
    {
        public Queue<SendingData> SendingQueue;

        public NetworkSendingQueue()
        {
            SendingQueue = new Queue<SendingData>();
        }

        public void QueuePackage(byte[] data, IPEndPoint receiveEndpoint)
        {
            SendingData sendingData = new SendingData
            {
                Buffer = data,
                ReceiveEndpoint = receiveEndpoint,
                BufferLength = data.Length
            };

            SendingQueue.Enqueue(sendingData);
        }

        public bool HasPackets()
        {
            return SendingQueue.Count > 0;
        }

        public void Dispose()
        {
            SendingQueue.Clear();
        }
    }

    public struct SendingData
    {
        public byte[] Buffer;
        public IPEndPoint ReceiveEndpoint;
        public int BufferLength;
    }
}
