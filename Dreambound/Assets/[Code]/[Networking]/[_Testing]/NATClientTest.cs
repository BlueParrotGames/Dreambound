using System.Net.Sockets;
using UnityEngine;

public class NATClientTest : MonoBehaviour
{
    [SerializeField] private string _ip;

    private Socket _socket;

    private void Start()
    {
        _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        try
        {
            _socket.Connect(_ip, 4382);

            Debug.LogWarning("Socket Connected");
        }
        catch
        {
            Debug.LogError("Fuck it didn't work :'(");
        }
    }
}
