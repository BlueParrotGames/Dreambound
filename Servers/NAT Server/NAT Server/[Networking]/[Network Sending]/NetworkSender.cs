using System.Net.Sockets;

using BPG.NATServer.Utility;

namespace BPG.NATServer.Sending
{
    class NetworkSender
    {
        private static NetworkSendingQueue _queue;

        public NetworkSender(NetworkSendingQueue queue)
        {
            _queue = queue;
        }
        
        public static void SendPacket(ByteBuffer buffer, Socket socket)
        {
            SendingData data = new SendingData
            {
                Buffer = buffer,
                Receiver = socket
            };

            _queue.QueuePackage(buffer.ToArray(), socket);
        }
    }
}
