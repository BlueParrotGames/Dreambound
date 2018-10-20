using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainThread : MonoBehaviour
{
    private static MainThread _instance;
    private static Queue<ThreadCall> _callQueue;

    private void Awake()
    {
        if (_instance != null)
        {
            Debug.LogWarning("Multiple instances of MainThread where found, make sure there is only one instance in you scene!");
            Destroy(this);
            return;
        }

        _callQueue = new Queue<ThreadCall>();
        _instance = this;
    }

    public static void Call(Action<object> func, object parameter)
    {
        _callQueue.Enqueue(new ThreadCall(func, parameter));
    }
    public static void Call(Action<object> func)
    {
        _callQueue.Enqueue(new ThreadCall(func, null));
    }

    private void Update()
    {
        if (_callQueue.Count > 0)
        {
            for (int i = 0; i < _callQueue.Count; i++)
            {
                ThreadCall call = _callQueue.Dequeue();
                if(call.parameter == null)
                {
                    call.callback.Invoke(null);
                }
                else
                {
                    call.callback.Invoke(call.parameter);
                }
            }
        }
    }
}

struct ThreadCall
{
    public readonly Action<object> callback;
    public readonly object parameter;

    public ThreadCall(Action<object> callback, object parameter)
    {
        this.callback = callback;
        this.parameter = parameter;
    }
}