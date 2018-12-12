using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

using BPG.NATServer.Utility;

namespace BPG.NATServer.Data.Sending
{
    public class NetworkSender
    {
        private static NetworkSendingQueue _sendingQueue;

        public NetworkSender(NetworkSendingQueue sendingQueue)
        {
            _sendingQueue = sendingQueue;
        }

        public static void SendToOne(byte[] data, IPEndPoint endPoint)
        {
            _sendingQueue.QueuePackage(data, endPoint);
        }

    }
}
