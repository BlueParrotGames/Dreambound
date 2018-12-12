using Dreambound.Networking.Utility;
using System.Net;
using UnityEngine;

namespace Dreambound.Networking.Data.Sending
{
    public class NetworkSender
    {
        private static NetworkSendingQueue _queue;

        public NetworkSender(NetworkSendingQueue queue)
        {
            _queue = queue;
        }

        public static void SendPacket(byte[] buffer, IPEndPoint receiver)
        {
            _queue.QueuePackage(buffer, receiver);
        }
    }
}
