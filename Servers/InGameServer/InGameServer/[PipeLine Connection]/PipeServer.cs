using System.IO.Pipes;

using BPS.Debugging;
using BPS.PipeLine.Sending;

namespace BPS.PipeLine
{
    class PipeServer
    {
        public static PipeServer Instance;

        private NamedPipeServerStream _pipeServer;

        private PipeSender _sender;
        private PipeSendingQueue _sendingQueue;
        private PipeSendingLoop _sendingLoop;

        public PipeServer()
        {
            Instance = this;

            //Initialize Sending Components
            _pipeServer = new NamedPipeServerStream("BPG Dreambound Pipeline", PipeDirection.InOut);
            _sendingQueue = new PipeSendingQueue(_pipeServer);
            _sender = new PipeSender(_sendingQueue);
            _sendingLoop = new PipeSendingLoop(this, _sendingQueue);

            Logger.Log("Waiting for a pipeline client to connect...");

            //Wait for the login server to connect
            _pipeServer.WaitForConnection();

            Logger.Log("Pipeline connection established...\n");
        }
    }
}
