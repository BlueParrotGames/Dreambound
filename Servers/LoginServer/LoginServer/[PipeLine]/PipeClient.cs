using System;
using System.IO.Pipes;

using BPS.Debugging;
using BPS.LoginServer.Utility;
using BPS.PipeLine.Data;
using System.Threading;

namespace BPS.PipeLine
{
    class PipeClient
    {
        public static PipeClient Instance;

        private NamedPipeClientStream _pipeClient;
        private PipePackageHandling _packetHandler;

        ByteBuffer _handlingBuffer;
        private byte[] _buffer = new byte[1024];
        private bool _isRunning = false;

        public static event Action<string, int, bool> OnUserStateUpdated;

        public PipeClient()
        {
            Instance = this;
            _packetHandler = new PipePackageHandling(_pipeClient);

            ConnectPipeLine();
            BeginReceiving();

            _handlingBuffer = new ByteBuffer();

            _isRunning = true;
            Thread handlingLoop = new Thread(PacketHandlingLoop);
            handlingLoop.Start();
        }
        private void ConnectPipeLine()
        {
            Logger.Log("Connecting to pipeline...");

            _pipeClient = new NamedPipeClientStream(".", "BPG Dreambound Pipeline", PipeDirection.InOut, PipeOptions.Asynchronous);

            _pipeClient.Connect();

            Logger.Log("Pipeline connection established...\n");
        }

        private void BeginReceiving()
        {
            _pipeClient.BeginRead(_buffer, 0, _buffer.Length, new AsyncCallback(ReceiveCallback), null);
        }
        private void ReceiveCallback(IAsyncResult result)
        {
            try
            {
                int readBytes = _pipeClient.EndRead(result);

                if(readBytes <= 0)
                {
                    Logger.LogError("Pipe received 0 bytes?");
                    return;
                }

                byte[] tempBuffer = new byte[readBytes];
                Buffer.BlockCopy(_buffer, 0, tempBuffer, 0, readBytes);

                if (readBytes > 0)
                    GetPacketsOutOfStream(tempBuffer);

                BeginReceiving();
            }
            catch
            {

            }
        }
        private void GetPacketsOutOfStream(byte[] buffer)
        {
            int readPos = 0;
            int totalBytes = buffer.Length;

            while (readPos < totalBytes)
            {
                int packetLength = BitConverter.ToInt32(buffer, readPos) + sizeof(int);

                byte[] data = new byte[packetLength];
                Buffer.BlockCopy(buffer, readPos, data, 0, packetLength);

                _packetHandler.QueuePackage(data, _pipeClient);

                readPos += packetLength;
            }
        }

        private void PacketHandlingLoop()
        {
            while (_isRunning)
            {
                if (_packetHandler.HasPackets())
                {
                    PipePackage package = _packetHandler.PackageQueue.Dequeue();

                    _handlingBuffer.Clear();
                    _handlingBuffer.WriteBytes(package.Data);
                    
                    OnUserStateUpdated?.Invoke(_handlingBuffer.ReadString(), _handlingBuffer.ReadInt(), _handlingBuffer.ReadBool());
                }
            }
        }
    }
}
