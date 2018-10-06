using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

using BPS.LoginServer.Utility;

namespace BPS.LoginServer.Sending
{
    class NetworkSendingQueue
    {
        private Queue<SendingData> _sendingQueue;
        public Queue<SendingData> SendingQueue { get => _sendingQueue; internal set => _sendingQueue = value; }

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
                ByteLenght = data.Length
            };

            SendingQueue.Enqueue(sendData);
        }

        public bool HasPackets()
        {
            return (SendingQueue.Count > 0);
        }
    }

    public struct SendingData
    {
        public ByteBuffer Buffer;
        public Socket Receiver;
        public int ByteLenght;
    }
}
