using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

using BPS.LoginServer.Utility;
using BPS.LoginServer.Sending;

namespace BPS.LoginServer.Security
{
    class VerificationLogic
    {
        public void SetClientHash(Client client)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteInt((int)PacketType.Verification);
            string time = DateTime.UtcNow.Millisecond.ToString();
            string hash = CreateHash(time);
            buffer.WriteString(hash);
            client.HashToken = hash;

            Thread.Sleep(1000);

            NetworkSender.SendPacket(buffer, client.Socket);
        }

        public static string CreateHash(string input)
        {
            string salt = input = input + "243s";
            string hashCode = string.Format("{0:X}", input.GetHashCode());

            return hashCode;
        }
    }
}
