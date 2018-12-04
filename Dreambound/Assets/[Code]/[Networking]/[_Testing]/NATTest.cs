using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Dreambound.Networking.PeerToPeer.Server;

public class NATTest : MonoBehaviour
{
    [SerializeField] private Text _ipText;
    [SerializeField] private Text _connectText;

    private PeerServer _server;

    private void Start()
    {
        _server = new PeerServer();

        _ipText.text = NATHole.Punch();
    }

    private void Update()
    {
        _connectText.text = "Connected clients: " + _server.ConnectionsCount;
    }
}
