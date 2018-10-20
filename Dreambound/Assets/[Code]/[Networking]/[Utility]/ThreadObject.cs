using System;
using UnityEngine;

namespace Dreambound.Networking.Threading
{
    public class ThreadObject : MonoBehaviour
    {
        private event Action<object> _function;

        protected void SetFunction(Action<object> func)
        {
            _function = func;
        }
        protected void Listener(object obj)
        {
            MainThreadDispatcher.Instance().Enqueue(_function, obj);
        }
    }
}
