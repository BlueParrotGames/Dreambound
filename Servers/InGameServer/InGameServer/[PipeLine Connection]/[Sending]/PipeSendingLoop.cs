using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Pipes;
using System.Threading;

namespace BPS.PipeLine.Sending
{
    class PipeSendingLoop
    {
        private readonly PipeServer _server;
        private readonly PipeSendingQueue _queue;
        private readonly bool _isSending;

        public PipeSendingLoop(PipeServer server, PipeSendingQueue queue)
        {
            _server = server;
            _queue = queue;
            _isSending = true;

            Thread sendingTreadOne = new Thread(CoreLoop);
            sendingTreadOne.Start();
        }

        private void CoreLoop()
        {
            while (_isSending)
            {
                if (_queue.HasPackets())
                {
                    SendingData sendableData = _queue.SendingQueue.Dequeue();

                    sendableData.Stream.BeginWrite(sendableData.Buffer.ToArray(), 0, sendableData.Buffer.Count(), new AsyncCallback(SendCallback), sendableData.Stream);
                }
            }
        }

        private void SendCallback(IAsyncResult result)
        {
            NamedPipeServerStream stream = (NamedPipeServerStream)result.AsyncState;
            stream.EndWrite(result);

            Debugging.Logger.Log("Send data through the pipe");
        }
    }
}
