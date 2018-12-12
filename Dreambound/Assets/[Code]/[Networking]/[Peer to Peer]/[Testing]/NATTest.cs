using System;
using System.Net;
using System.Text;
using System.Net.Sockets;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.UI;

using Dreambound.Networking.P2P;

public class NATTest : MonoBehaviour
{
    private P2PClient _natHandler;
    [SerializeField] InputField _inputField;

    private void Start()
    {
        _natHandler = new P2PClient(new IPEndPoint(IPAddress.Parse("178.84.28.2"), 4383));
        //_natHandler = new P2PClient(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 4383));
    }

    public void Send()
    {
        _natHandler.Send(_inputField.text);
    }
}