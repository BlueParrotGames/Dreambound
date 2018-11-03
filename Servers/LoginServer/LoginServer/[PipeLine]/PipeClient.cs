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

        ByteBuffer _byteBuffer;
        private byte[] _buffer = new byte[1024];
        private bool _isRunning = false;

        public static event Action<string, int, bool> OnUserStateUpdated;

        public PipeClient()
        {
            Instance = this;
            _packetHandler = new PipePackageHandling(_pipeClient);

            ConnectPipeLine();
            BeginReceiving();

            _byteBuffer = new ByteBuffer();

            _isRunning = true;
            Thread handlingLoop = new Thread(PacketHandlingLoop);
            handlingLoop.Start();
        }
        private void ConnectPipeLine()
        {
            Logger.Log("Pipeline connecting...");

            _pipeClient = new NamedPipeClientStream(".", "A", PipeDirection.InOut, PipeOptions.Asynchronous);

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

                _byteBuffer.Clear();
                _byteBuffer.WriteBytes(data);

                OnUserStateUpdated?.Invoke(_byteBuffer.ReadString(), _byteBuffer.ReadInt(), _byteBuffer.ReadBool());

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
                }
            }
        }
    }
}
