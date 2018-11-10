using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dreambound.Astar
{
    public class PathRequestManagar : MonoBehaviour
    {
        private Queue<PathRequest> _requestQueue;
        private PathRequest _currentRequest;

        private PathFinding _pathFinding;
        private bool _isProcessingPath;

        private static PathRequestManagar _instance;

        private void Awake()
        {
            _instance = this;
            _requestQueue = new Queue<PathRequest>();
            _pathFinding = GetComponent<PathFinding>();
        }

        public static void RequestPath(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callback)
        {
            PathRequest request = new PathRequest(pathStart, pathEnd, callback);

            _instance._requestQueue.Enqueue(request);
            _instance.TryProcessNext();
        }
        private void TryProcessNext()
        {
            if(!_isProcessingPath && _requestQueue.Count > 0)
            {
                _currentRequest = _requestQueue.Dequeue();
                _isProcessingPath = true;

                _pathFinding.StartFindPath(_currentRequest.PathStart, _currentRequest.PathEnd);
            }
        }

        public void FinishedProcessingPath(Vector3[] path, bool succes)
        {
            _currentRequest.Callback(path, succes);
            _isProcessingPath = false;
            TryProcessNext();
        }


        struct PathRequest
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
}

