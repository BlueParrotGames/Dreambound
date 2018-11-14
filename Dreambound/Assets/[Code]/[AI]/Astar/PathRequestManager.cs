using System;
using System.Threading;
using System.Collections.Generic;
using UnityEngine;

namespace Dreambound.Astar
{
    public class PathRequestManager : MonoBehaviour
    {
        private Queue<PathResult> _results;

        private static PathRequestManager _instance;
        private PathFinding _pathFinding;

        private void Awake()
        {
            _instance = this;
            _pathFinding = GetComponent<PathFinding>();

            _results = new Queue<PathResult>();
        }
        private void Update()
        {
            if(_results.Count > 0)
            {
                lock (_results)
                {
                    for(int i = 0; i < _results.Count; i++)
                    {
                        PathResult result = _results.Dequeue();
                        result.Callback(result.Path, result.Success);
                    }
                }
            }
        }

        public static void RequestPath(PathRequest request)
        {
            ThreadStart threadStart = delegate
            {
                _instance._pathFinding.FindPath(request, _instance.FinishedProcessingPath);
            };
            threadStart.Invoke();
        }

        public void FinishedProcessingPath(PathResult result)
        {
            lock (_results)
            {
                _results.Enqueue(result);
            }
        }
    }
    
    public struct PathResult
    {
        public readonly Vector3[] Path;
        public readonly bool Success;
        public readonly Action<Vector3[], bool> Callback;

        public PathResult(Vector3[] path, bool success, Action<Vector3[], bool> callback)
        {
            Path = path;
            Success = success;
            Callback = callback;
        }
    }

    public struct PathRequest
    {
        public readonly Vector3 PathStart;
        public readonly Vector3 PathEnd;
        public readonly Action<Vector3[], bool> Callback;

        public PathRequest(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callback)
        {
            PathStart = pathStart;
            PathEnd = pathEnd;
            Callback = callback;
        }
    }
}

