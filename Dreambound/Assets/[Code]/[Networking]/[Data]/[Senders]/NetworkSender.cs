using Dreambound.Networking.Utility;
using System.Net.Sockets;

namespace Dreambound.Networking.DataHandling
{
    public class NetworkSender
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
