using System;
using System.Collections.Generic;
using BPS.InGameServer.Utility;
using System.IO.Pipes;

namespace BPS.PipeLine.Sending
{
    class PipeSendingQueue
    {
        public Queue<SendingData> SendingQueue;
        private ByteBuffer _buffer;
        private NamedPipeServerStream _stream;

        public PipeSendingQueue(NamedPipeServerStream stream)
        {
            _stream = stream;

            SendingQueue = new Queue<SendingData>();
            _buffer = new ByteBuffer();
        }

        public void QueuePackage(byte[] data)
        {
            _buffer.Clear();
            _buffer.WriteInt(data.Length);
            _buffer.WriteBytes(data);

            SendingData sendData = new SendingData()
            {
                Buffer = _buffer,
                Stream = _stream,
                ByteLength = data.Length
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
        public NamedPipeServerStream Stream;
        public int ByteLength;
    }
}
