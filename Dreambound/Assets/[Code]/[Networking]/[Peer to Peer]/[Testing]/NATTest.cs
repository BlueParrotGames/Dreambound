using System.Net.Sockets;

using UnityEngine;

using Dreambound.Networking.P2P;

public class NATTest : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Socket socket = NATHole.Punch("127.0.0.1", 4383);
            Socket socket = NATHole.Punch("178.84.28.2", 4383);
        }
    }
}