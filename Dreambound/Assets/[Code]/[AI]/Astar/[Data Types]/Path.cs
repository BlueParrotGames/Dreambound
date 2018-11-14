using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dreambound.Astar
{
    public class Path
    {
        public readonly Vector3[] LookPoints;
        public readonly Line[] TurnBoundaries;
        public readonly int FinishLineIndex;
        public readonly int SlowdownIndex;

        public Path(Vector3[] waypoints, Vector3 startPosition, float turnDistance, float stoppingDistance)
        {
            LookPoints = waypoints;
            TurnBoundaries = new Line[LookPoints.Length];
            FinishLineIndex = TurnBoundaries.Length - 1;

            Vector3 previousPoint = startPosition;
            for (int i = 0; i < LookPoints.Length; i++)
            {
                Vector3 currentPoint = LookPoints[i];
                Vector3 directionToCurrentPoint = (currentPoint - previousPoint).normalized;
                Vector3 turnBoundaryPoint = (i == FinishLineIndex) ? currentPoint : currentPoint - directionToCurrentPoint * turnDistance;

                TurnBoundaries[i] = new Line(turnBoundaryPoint, previousPoint - directionToCurrentPoint * turnDistance);
                previousPoint = turnBoundaryPoint;
            }

            float distanceFromEndpoint = 0;
            for(int i = LookPoints.Length - 1; i > 0; i--)
            {
                distanceFromEndpoint += Vector3.Distance(LookPoints[i], LookPoints[i - 1]);
                if(distanceFromEndpoint > stoppingDistance)
                {
                    SlowdownIndex = i;
                    break;
                }
            }
        }

        public void DrawWithGizmos()
        {
            Gizmos.color = Color.black;

            foreach(Vector3 point in LookPoints)
            {
                Gizmos.DrawCube(point + Vector3.up, Vector3.one);
            }

            foreach(Line line in TurnBoundaries)
            {
                line.DrawWithGizmos(10);
            }
        }
    }
}
