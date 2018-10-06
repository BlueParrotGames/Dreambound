using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

using BPS.LoginServer.Utility;

namespace BPS.LoginServer.Sending
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
