using System.Net;
using System.Net.Sockets;
using UnityEngine;

namespace Dreambound.Networking.PeerToPeer.Server
{
    public class NATHole
    {
        public static void Punch()
        {
            IPHostEntry hostEntry = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress localAddress = null;

            for(int i = 0; i < hostEntry.AddressList.Length; i++)
            {
                if(hostEntry.AddressList[i].AddressFamily == AddressFamily.InterNetwork)
                {
                    localAddress = hostEntry.AddressList[i];
                }
            }

            Debug.Log(localAddress.ToString());
        }
    }
}