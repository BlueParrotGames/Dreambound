using BPS.InGameServer.Utility;

namespace BPS.PipeLine.Sending
{
    class PipeSender
    {
        private static PipeSendingQueue _queue;

        public PipeSender(PipeSendingQueue queue)
        {
            _queue = queue;
        }

        public static void SendPacket(ByteBuffer buffer)
        {
            SendingData data = new SendingData
            {
                Buffer = buffer,
            };

            _queue.QueuePackage(buffer.ToArray());
        }
    }
}
