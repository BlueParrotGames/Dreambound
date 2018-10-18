using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

using Dreambound.Networking.Utility;

namespace Dreambound.Networking.DataHandling
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

        public void QueuePackage(byte[] data, Socket socket)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteInt(data.Length);
            buffer.WriteBytes(data);

            SendingData sendData = new SendingData
            {
                Buffer = buffer,
                Receiver = socket,
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
        public ByteBuffer Buffer;
        public Socket Receiver;
        public int ByteLength;
    }
}
