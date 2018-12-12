using System;
using System.Net;
using System.Collections.Generic;

using BPG.NATServer.Utility;

namespace BPG.NATServer.Data.Handlers
{
    public class PacketHandler : IDisposable
    {
        private ByteBuffer _buffer;
        public Queue<ClientNetworkPackage> PacketQueue;

        public PacketHandler()
        {
            PacketQueue = new Queue<ClientNetworkPackage>();
            _buffer = new ByteBuffer();
        }

        public void QueuePacket(byte[] data, IPEndPoint endPoint)
        {
            _buffer.Clear();
            _buffer.WriteBytes(data);

            PacketType packetType = (PacketType)_buffer.ReadInt();

            PacketQueue.Enqueue(new ClientNetworkPackage(packetType, endPoint, _buffer.ReadBytes(_buffer.Length())));
        }

        public bool HasPackets()
        {
            return PacketQueue.Count > 0;
        }

        public void Dispose()
        {
            _buffer = null;
            PacketQueue.Clear();
        }
    }
}
