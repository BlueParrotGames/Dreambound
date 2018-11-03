using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Pipes;
using BPS.LoginServer.Utility;

namespace BPS.PipeLine.Data
{
    class PipePackageHandling
    {
        private ByteBuffer _buffer;
        private PipePackagePool _packagePool;
        private NamedPipeClientStream _stream;

        public Queue<PipePackage> PackageQueue;

        public PipePackageHandling(NamedPipeClientStream stream)
        {
            _stream = stream;

            _buffer = new ByteBuffer();
            _packagePool = new PipePackagePool();

            PackageQueue = new Queue<PipePackage>();
        }

        public void QueuePackage(byte[] data, NamedPipeClientStream stream)
        {
            _buffer.Clear();
            _buffer.WriteBytes(data);
            int packetSize = _buffer.ReadInt();

            PackageQueue.Enqueue(_packagePool.GetPackage(_stream, _buffer.ReadBytes(packetSize)));
        }

        public bool HasPackets()
        {
            return (PackageQueue.Count > 0);
        }
    }
}
