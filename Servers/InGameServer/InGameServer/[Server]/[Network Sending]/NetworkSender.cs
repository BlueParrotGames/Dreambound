using System.Net.Sockets;

using BPS.InGameServer.Utility;

namespace BPS.InGameServer.Sending
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
