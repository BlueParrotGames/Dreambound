using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Pipes;

namespace BPS.PipeLine.Data
{
    public interface INetworkPackage
    {
        void ReturnToPool();
    }

    public struct PipePackage : INetworkPackage
    {
        public NamedPipeClientStream Stream { get; internal set; }
        public byte[] Data { get; internal set; }

        private PipePackagePool _masterPool;

        public PipePackage(NamedPipeClientStream stream, byte[] data, PipePackagePool masterPool)
        {
            Stream = stream;
            Data = data;
            _masterPool = masterPool;
        }

        public void ReturnToPool()
        {
            _masterPool.ReturnPackage(this);
        }
    }
}
