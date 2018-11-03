using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Pipes;

namespace BPS.PipeLine.Data
{
    public class PipePackagePool
    {
        private Queue<PipePackage> _packageQueue;

        public PipePackagePool()
        {
            _packageQueue = new Queue<PipePackage>();
        }

        public PipePackage GetPackage(NamedPipeClientStream stream, byte[] data)
        {
            if (_packageQueue.Count <= 0)
                CreateNewPackage(stream);

            PipePackage package = _packageQueue.Dequeue();
            package.Data = data;

            return package;
        }
        private void CreateNewPackage(NamedPipeClientStream stream)
        {
            _packageQueue.Enqueue(new PipePackage(stream, null, this));
        }

        public void ReturnPackage(PipePackage package)
        {
            _packageQueue.Enqueue(package);
        }
    }
}
