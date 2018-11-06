using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace Dreambound.Astar
{
    public class PathFinding : MonoBehaviour
    {
        [SerializeField] private Transform _player;
        [SerializeField] private Transform _target;

        private Grid _grid;

        private void Awake()
        {
            _grid = GetComponent<Grid>();
        }
        private void Update()
        {
            FindPath(_player.position, _target.position);
        }

        private void FindPath(Vector3 startPosition, Vector3 targetPosition)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            Node startNode = _grid.GetNodeFromWorldPoint(startPosition);
            Node targetNode = _grid.GetNodeFromWorldPoint(targetPosition);

            Heap<Node> openSet = new Heap<Node>(_grid.MaxSize);
            HashSet<Node> closedSet = new HashSet<Node>();

            openSet.Add(startNode);
            
            while(openSet.Count > 0)
            {
                Node currentNode = openSet.RemoveFirst();
                closedSet.Add(currentNode);

                if (currentNode == targetNode)
                {
                    stopwatch.Stop();
                    UnityEngine.Debug.Log("Path found: " + stopwatch.ElapsedMilliseconds + "ms");

                    RetracePath(startNode, targetNode);
                    return;
                }

                foreach(Node neigbour in _grid.GetNeigbours(currentNode))
                {
                    if (!neigbour.Walkable || closedSet.Contains(neigbour))
                        continue;

                    int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neigbour);
                    if(newMovementCostToNeighbour < neigbour.gCost || !openSet.Contains(neigbour))
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

        private void RetracePath(Node startNode, Node targetNode)
        {
            List<Node> path = new List<Node>();
            Node currentNode = targetNode;

            while (currentNode != startNode)
            {
                path.Add(currentNode);
                currentNode = currentNode.Parent;
            }

            path.Reverse();

            _grid.path = path;
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
