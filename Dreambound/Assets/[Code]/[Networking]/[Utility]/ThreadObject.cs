using System;
using UnityEngine;

namespace Dreambound.Networking.Threading
{
    public class ThreadObject : MonoBehaviour
    {
        private event Action<object> _singleParamFunction;
        private event Action<object, object> _mulitParamsFunction;

        protected void SetFunction(Action<object> func)
        {
            _singleParamFunction = func;
        }
        protected void SetFunction(Action<object, object> func)
        {
            _mulitParamsFunction = func;
        }
        protected void Listener(object obj)
        {
            MainThreadDispatcher.Instance().Enqueue(_singleParamFunction, obj);
        }
        protected void Listener(object obj1, object obj2)
        {
            MainThreadDispatcher.Instance().Enqueue(_mulitParamsFunction, obj1, obj2);
        }
    }
}
