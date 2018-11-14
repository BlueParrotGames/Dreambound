using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dreambound.Astar
{
    public struct Line
    {
        private const float _verticalLineGradient = 1e5f;

        private float _gradient;
        private float _zIntercept;

        private Vector3 _pointOnLine1;
        private Vector3 _pointOnLine2;

        private float _gradientPerpendicular;

        private bool _approachSide;

        public Line(Vector3 pointOnLine, Vector3 pointPerpendicularToLine)
        {
            float deltaX = pointOnLine.x - pointPerpendicularToLine.x;
            float deltaZ = pointOnLine.z - pointPerpendicularToLine.z;

            if (deltaX == 0)
                _gradientPerpendicular = _verticalLineGradient;
            else
                _gradientPerpendicular = deltaZ / deltaX;

            if (_gradientPerpendicular == 0)
                _gradient = _verticalLineGradient;
            else
                _gradient = -1 / _gradientPerpendicular;

            _zIntercept = pointOnLine.z - _gradient * pointOnLine.x;
            _pointOnLine1 = pointOnLine;
            _pointOnLine2 = pointOnLine + new Vector3(1, 0, _gradient);

            _approachSide = false;
            _approachSide = GetSide(pointPerpendicularToLine);
        }

        private bool GetSide(Vector3 point)
        {
            return (point.x - _pointOnLine1.x) * (_pointOnLine2.z - _pointOnLine1.z) > (point.z - _pointOnLine1.z) * (_pointOnLine2.x - _pointOnLine1.x);
        }

        public bool HasCrossedLine(Vector3 point)
        {
            return GetSide(point) != _approachSide;
        }

        public float DistanceFromPoint(Vector3 point)
        {
            float zInterceptPerpendicular = point.z - _gradientPerpendicular * point.x;
            float intersectX = (zInterceptPerpendicular - _zIntercept) / (_gradient - _gradientPerpendicular);
            float intersectZ = _gradient * intersectX + _zIntercept;

            return Vector3.Distance(point, new Vector3(intersectX, point.y, intersectZ));
        }

        public void DrawWithGizmos(float length)
        {
            Vector3 lineDirection = new Vector3(1, 0, _gradient).normalized;
            Vector3 lineCenter = new Vector3(_pointOnLine1.x, _pointOnLine1.y, _pointOnLine1.z) + Vector3.up;

            Gizmos.color = Color.white;
            Gizmos.DrawLine(lineCenter - lineDirection * length / 2f, lineCenter + lineDirection * length / 2f);
        }
    }
}
