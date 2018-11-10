using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dreambound.Astar
{
    public class PathFinding : MonoBehaviour
    {
        private Grid _grid;
        private PathRequestManagar _requestManagar;

        private void Awake()
        {
            _grid = GetComponent<Grid>();
            _requestManagar = GetComponent<PathRequestManagar>();
        }

        public void StartFindPath(Vector3 startPos, Vector3 targetPos)
        {
            StartCoroutine(FindPath(startPos, targetPos));
        }
        private IEnumerator FindPath(Vector3 startPosition, Vector3 targetPosition)
        {
            List<Node> path = new List<Node>();
            Vector3[] waypoints = new Vector3[0];
            bool pathSucces = false;

            Node startNode = _grid.GetNodeFromWorldPoint(startPosition);
            Node targetNode = _grid.GetNodeFromWorldPoint(targetPosition);

            if (startNode.Walkable && targetNode.Walkable)
            {
                Heap<Node> openSet = new Heap<Node>(_grid.MaxSize);
                HashSet<Node> closedSet = new HashSet<Node>();

                openSet.Add(startNode);

                while (openSet.Count > 0)
                {
                    Node currentNode = openSet.RemoveFirst();
                    closedSet.Add(currentNode);

                    if (currentNode == targetNode)
                    {
                        pathSucces = true;

                        break;
                    }

                    foreach (Node neigbour in _grid.GetNeigbours(currentNode))
                    {
                        if (!neigbour.Walkable || closedSet.Contains(neigbour))
                            continue;

                        int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neigbour) + neigbour.MovementPenalty;
                        if (newMovementCostToNeighbour < neigbour.gCost || !openSet.Contains(neigbour))
                        {
                            neigbour.gCost = newMovementCostToNeighbour;
                            neigbour.hCost = GetDistance(neigbour, targetNode);
                            neigbour.Parent = currentNode;

                            if (!openSet.Contains(neigbour))
                                openSet.Add(neigbour);
                            else
                                openSet.UpdateItem(neigbour);
                        }
                    }
                }
            }

            yield return null;

            if (pathSucces)
            {
                waypoints = RetracePath(startNode, targetNode);
            }

            _requestManagar.FinishedProcessingPath(waypoints, pathSucces);
        }
        private Vector3[] RetracePath(Node startNode, Node targetNode)
        {
            List<Node> path = new List<Node>();
            Node currentNode = targetNode;

            while (currentNode != startNode)
            {
                path.Add(currentNode);
                currentNode = currentNode.Parent;
            }

            Vector3[] waypoints = SimplifyPath(path);
            Array.Reverse(waypoints);

            return waypoints;
        }

        private Vector3[] SimplifyPath(List<Node> path)
        {
            List<Vector3> waypoints = new List<Vector3>();
            Vector3 oldDirection = Vector3.zero;

            for(int i = 1; i < path.Count; i++)
            {
                Vector3 newDirection = path[i - 1].WorldPosition - path[i].WorldPosition;

                if(newDirection != oldDirection)
                {
                    waypoints.Add(path[i].WorldPosition);
                }

                oldDirection = newDirection;
            }

            return waypoints.ToArray();
        }

        private int GetDistance(Node node1, Node node2)
        {
            int distanceX = Mathf.Abs(node1.GridPosition.x - node2.GridPosition.x);
            int distanceY = Mathf.Abs(node1.GridPosition.y - node2.GridPosition.y);
            int distanceZ = Mathf.Abs(node1.GridPosition.z - node2.GridPosition.z);

            //Calculate the distance between X and Y
            int xyDist;
            if (distanceX > distanceY)
                xyDist = 14 * distanceY + 10 * (distanceX - distanceY);
            else
                xyDist = 14 * distanceX + 10 * (distanceY - distanceX);

            //Calculate the Distance between xyDist and Z
            if (xyDist > distanceZ)
                return 14 * distanceZ + 10 * (xyDist - distanceZ);
            else
                return 14 * xyDist + 10 * (distanceZ - xyDist);
        }
    }
}
