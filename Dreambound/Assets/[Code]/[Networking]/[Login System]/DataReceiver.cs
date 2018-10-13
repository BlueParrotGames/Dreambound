using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System;

using UnityEngine;

namespace Dreambound.Networking
{
    public class DataReceiver
    {
        private Socket _socket;
        private byte[] _buffer = new byte[4096];

        public DataReceiver(Socket socket)
        {
            _socket = socket;
        }

        public void ReceiveData()
        {
            _socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), null);
        }
        private void ReceiveCallback(IAsyncResult result)
        {
            int bytes = _socket.EndReceive(result);
        }
    }
}
