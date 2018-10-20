using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ThreadedDataRequester : MonoBehaviour
{
    private static ThreadedDataRequester _instance;
    private Queue<ThreadInfo> _dataQueue = new Queue<ThreadInfo>();

    public delegate void Event(object Data);

    void Awake()
    {
        _instance = FindObjectOfType<ThreadedDataRequester>();
    }

    public static void RequestData(Func<object> Data, Action<object> Callback)
    {
        ThreadStart threadStart = delegate
        {
            _instance.DataThread(Data, Callback);
        };
    }
    private void DataThread(Func<object> Data, Action<object> Callback)
    {
        object data = Data();
        lock (_dataQueue)
        {
            _dataQueue.Enqueue(new ThreadInfo(Callback, data));
        }
    }

    private void Update()
    {
        if (_dataQueue.Count > 0)
        {
            for (int i = 0; i < _dataQueue.Count; i++)
            {
                ThreadInfo threadInfo = _dataQueue.Dequeue();
                threadInfo.callback(threadInfo.parameter);

            }
        }
    }

    struct ThreadInfo
    {
        public readonly Action<object> callback;
        public readonly object parameter;

        public ThreadInfo(Action<object> callback, object parameter)
        {
            this.callback = callback;
            this.parameter = parameter;
        }

    }
}
