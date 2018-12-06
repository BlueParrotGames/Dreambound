using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace BPG.ProgramControl
{
    public class InputReader
    {
        public static InputReader Instance { get; internal set; }

        private bool _isRunning;
        private Thread _readLoopThread;

        public InputReader()
        {
            Instance = this;

            _isRunning = true;
            _readLoopThread = new Thread(ReadInputLoop);
            _readLoopThread.Start();
        }

        private void ReadInputLoop()
        {
            while (_isRunning)
            {
                string input = Console.ReadLine();
            }
        }
    }
}
