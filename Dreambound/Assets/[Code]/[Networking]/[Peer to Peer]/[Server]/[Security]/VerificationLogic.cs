using System;

using Dreambound.Networking.Utility;
using Dreambound.Networking.DataHandling;

namespace Dreambound.Networking.PeerToPeer.Server.Security
{
    public class VerificationLogic
    {
        public void SetClientHash(PeerClient client)
        {
            ByteBuffer buffer = new ByteBuffer();

            buffer.WriteInt((int)PacketType.Verification);
            string time = DateTime.UtcNow.Millisecond.ToString();
            string hash = CreateHash(time);
            buffer.WriteString(hash);
            client.HashToken = hash;

            NetworkSender.SendPacket(buffer, client.Socket);
        }

        private static string CreateHash(string input)
        {
            string salt = input = input + "243s";
            string hashCode = string.Format("{0:X}", input.GetHashCode());

            return hashCode;
        }
    }
}
