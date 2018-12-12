using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

using Dreambound.Networking.Utility;

namespace Dreambound.Networking.Data.Sending
{
    public class NetworkSendingQueue : IDisposable
    {
        private Queue<SendingData> _sendingQueue;
        public Queue<SendingData> SendingQueue
        {
            get { return _sendingQueue; }
            internal set { _sendingQueue = value; }
        }

        public NetworkSendingQueue()
        {
            SendingQueue = new Queue<SendingData>();
        }

        public void QueuePackage(byte[] data, IPEndPoint endPoint)
        {
            SendingData sendData = new SendingData
            {
                Buffer = data,
                Receiver = endPoint,
                ByteLength = data.Length
            };

            SendingQueue.Enqueue(sendData);
        }

        public bool HasPackets()
        {
            return (SendingQueue.Count > 0);
        }

        public void Dispose()
        {
            SendingQueue.Clear();
        }
    }

    public struct SendingData
    {
        public byte[] Buffer;
        public IPEndPoint Receiver;
        public int ByteLength;
    }
}
