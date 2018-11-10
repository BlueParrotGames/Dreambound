using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Dreambound.Astar;

namespace Dreambound.AI
{
    public class Unit : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private float _speed;

        private Vector3[] _path;
        private int _targetIndex;

        private void Start()
        {
            PathRequestManagar.RequestPath(transform.position, _target.position, OnPathFound);
        }

        public void OnPathFound(Vector3[] path, bool pathSuccesful)
        {
            if (pathSuccesful)
            {
                _path = path;

                StopCoroutine("FollowPath");
                StartCoroutine("FollowPath");
            }
        }

        private IEnumerator FollowPath()
        {
            Vector3 currentWaypoint = _path[0];

            while (true)
            {
                if (transform.position == currentWaypoint)
                {
                    _targetIndex++;

                    if (_targetIndex >= _path.Length)
                        yield break;

                    currentWaypoint = _path[_targetIndex];
                }

                transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, _speed * Time.deltaTime);
                yield return null;
            }
        }

        private void OnDrawGizmos()
        {
            if (_path != null)
            {
                for (int i = _targetIndex; i < _path.Length; i++)
                {

                    return;

                    Gizmos.color = Color.black;
                    Gizmos.DrawCube(_path[i], Vector3.one);


                    if (i == _targetIndex)
                        Gizmos.DrawLine(transform.position, _path[i]);
                    else
                        Gizmos.DrawLine(_path[i - 1], _path[i]);
                }
            }
        }
    }
}
