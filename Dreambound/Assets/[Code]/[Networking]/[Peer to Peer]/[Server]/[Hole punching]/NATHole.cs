using System.Net;
using System.Net.Sockets;
using UnityEngine;

namespace Dreambound.Networking.PeerToPeer.Server
{
    public class NATHole
    {
        public static string Punch()
        {
            IPHostEntry hostEntry = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress localAddress = null;

            for(int i = 0; i < hostEntry.AddressList.Length; i++)
            {
                if(hostEntry.AddressList[i].AddressFamily == AddressFamily.InterNetwork)
                    return hostEntry.AddressList[i].ToString();
            }

            return localAddress.ToString();
        }
    }
}