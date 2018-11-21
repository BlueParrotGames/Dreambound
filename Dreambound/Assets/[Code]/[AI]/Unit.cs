using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Dreambound.Astar;

namespace Dreambound.AI
{
    public class Unit : MonoBehaviour
    {
        private const float _minimumPathUpdateTime = 0.2f;
        private const float _pathUpdateThreshold = 0.5f;

        [SerializeField] private Transform _target;
        [SerializeField] private float _speed;
        [SerializeField] private float _turnSpeed;
        [SerializeField] private float _turnDistance;
        [SerializeField] private float _stoppingDistance;

        [SerializeField] private bool _drawPathGizmos;

        private Vector3 _moveDirection;
        private Vector3 _targetLastKnownPosition;

        private Path _path;
        private CharacterController _controller;

        private void Start()
        {
            _controller = GetComponent<CharacterController>();
            StartCoroutine(UpdatePath());
        }

        public void OnPathFound(Vector3[] waypoints, bool pathSuccesful)
        {
            if (pathSuccesful)
            {
                _path = new Path(waypoints, transform.position, _turnDistance, _stoppingDistance);

                StopCoroutine("FollowPath");
                StartCoroutine("FollowPath");
            }
        }

        private IEnumerator UpdatePath()
        {
            if (Time.timeSinceLevelLoad < 0.3f)
                yield return new WaitForSeconds(0.3f);

            PathRequestManager.RequestPath(new PathRequest(transform.position, GetTargetGroundPosition(), OnPathFound));

            float sqrMoveThreshold = _pathUpdateThreshold * _pathUpdateThreshold;
            Vector3 oldTargetPosition = GetTargetGroundPosition();

            while (true)
            {
                yield return new WaitForSeconds(_minimumPathUpdateTime);

                if ((GetTargetGroundPosition() - oldTargetPosition).sqrMagnitude > sqrMoveThreshold)
                {
                    PathRequestManager.RequestPath(new PathRequest(transform.position, GetTargetGroundPosition(), OnPathFound));
                    oldTargetPosition = _target.position;
                }
            }
        }

        private IEnumerator FollowPath()
        {
            bool followingPath = true;
            int pathIndex = 0;

            transform.LookAt(_path.LookPoints[0]);

            float speedPercent = 1;

            while (followingPath)
            {
                while (_path.TurnBoundaries[pathIndex].HasCrossedLine(transform.position))
                {
                    if (pathIndex == _path.FinishLineIndex)
                    {
                        followingPath = false;
                        break;
                    }
                    else
                    {
                        pathIndex++;
                    }
                }

                if (followingPath)
                {
                    if (pathIndex >= _path.SlowdownIndex && _stoppingDistance > 0)
                    {
                        speedPercent = Mathf.Clamp01(_path.TurnBoundaries[_path.FinishLineIndex].DistanceFromPoint(transform.position) / _stoppingDistance);

                        if (speedPercent < 0.01f)
                        {
                            followingPath = false;
                        }
                    }

                    Vector3 newForward = new Vector3(_path.LookPoints[pathIndex].x, 0, _path.LookPoints[pathIndex].z) - new Vector3(transform.position.x, 0, transform.position.z);
                    newForward.Normalize();

                    Quaternion targetRotation = Quaternion.LookRotation(newForward);
                    transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, _turnSpeed * Time.deltaTime);

                    _controller.Move(transform.forward * _speed * speedPercent * Time.deltaTime);
                }

                yield return null;
            }
        }

        private Vector3 GetTargetGroundPosition()
        {
            if(Physics.Raycast(_target.position, Vector3.down * 1000f, out RaycastHit hit))
            {
                _targetLastKnownPosition = hit.point;
            }

            return _targetLastKnownPosition;
        }

        private void OnDrawGizmos()
        {
            if (_path != null && _drawPathGizmos)
            {
                _path.DrawWithGizmos();
            }
        }
    }
}
