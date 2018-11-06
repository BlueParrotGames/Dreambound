using System.Collections.Generic;
using UnityEngine;

namespace Dreambound.Astar
{
    public class Grid : MonoBehaviour
    {
        [SerializeField] private LayerMask _unwalkableMask;
        [SerializeField] private Vector3 _gridWorldSize;
        [SerializeField] private float _nodeRadius;

        private Node[,,] _grid;

        private float _nodeDiameter;
        private Vector3Int _gridSize;

        private void Start()
        {
            _nodeDiameter = _nodeRadius * 2f;

            //Set the grid size
            _gridSize.x = Mathf.RoundToInt(_gridWorldSize.x / _nodeDiameter);
            _gridSize.y = Mathf.RoundToInt(_gridWorldSize.y / _nodeDiameter);
            _gridSize.z = Mathf.RoundToInt(_gridWorldSize.z / _nodeDiameter);

            CreateGrid();
        }

        private void CreateGrid()
        {
            _grid = new Node[_gridSize.x, _gridSize.y, _gridSize.z];

            //Set the far bottom left corner
            Vector3 worldBottomLeft = Vector3.zero;
            worldBottomLeft -= Vector3.right * _gridWorldSize.x / 2f;
            worldBottomLeft -= Vector3.up * _gridWorldSize.y / 2f;
            worldBottomLeft -= Vector3.forward * _gridWorldSize.z / 2f;


            for (int x = 0; x < _gridSize.x; x++)
            {
                for (int y = 0; y < _gridSize.y; y++)
                {
                    for (int z = 0; z < _gridSize.z; z++)
                    {
                        //Caluclate the worldpoint of the node
                        Vector3 worldPoint = worldBottomLeft;
                        worldPoint += Vector3.right * (x * _nodeDiameter + _nodeRadius);
                        worldPoint += Vector3.up * (y * _nodeDiameter + _nodeRadius);
                        worldPoint += Vector3.forward * (z * _nodeDiameter + _nodeRadius);

                        bool walkable = !(Physics.CheckSphere(worldPoint, _nodeRadius, _unwalkableMask));

                        _grid[x, y, z] = new Node(walkable, worldPoint, new Vector3Int(x, y, z));
                    }
                }
            }
        }

        public List<Node> GetNeigbours(Node node)
        {
            List<Node> neighbours = new List<Node>();

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    for (int z = -1; z <= 1; z++)
                    {
                        if (x == 0 && y == 0 && z == 0)
                            continue;

                        int checkX = node.GridPosition.x + x;
                        int checkY = node.GridPosition.y + y;
                        int checkZ = node.GridPosition.z + z;

                        //Check if X,Y,Z are inside the grid
                        if ((checkX >= 0 && checkX < _gridSize.x) && (checkY >= 0 && checkY < _gridSize.y) && (checkZ >= 0 && checkZ < _gridSize.z))
                        {
                            neighbours.Add(_grid[checkX, checkY, checkZ]);
                        }
                    }
                }
            }

            return neighbours;
        }

        public List<Node> path;
        public Node GetNodeFromWorldPoint(Vector3 worldPosition)
        {
            //Get the percent of the X,Y,Z compared to the grid world size
            float percentX = (worldPosition.x + _gridWorldSize.x / 2) / _gridWorldSize.x;
            float percentY = (worldPosition.y + _gridWorldSize.y / 2) / _gridWorldSize.y;
            float percentZ = (worldPosition.z + _gridWorldSize.z / 2) / _gridWorldSize.z;

            //Clamp the X,Y,Z values between 0 and 1
            percentX = Mathf.Clamp01(percentX);
            percentY = Mathf.Clamp01(percentY);
            percentZ = Mathf.Clamp01(percentZ);

            //Get the X,Y,Z coordinates
            int x = Mathf.RoundToInt((_gridSize.x - 1) * percentX);
            int y = Mathf.RoundToInt((_gridSize.y - 1) * percentY);
            int z = Mathf.RoundToInt((_gridSize.z - 1) * percentZ);

            return _grid[x, y, z];
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(Vector3.zero, _gridWorldSize);

            if (_grid != null)
            {
                foreach (Node node in _grid)
                {
                    Gizmos.color = (node.Walkable) ? Color.white : Color.red;

                    if(path != null)
                    {
                        if (path.Contains(node))
                            Gizmos.color = Color.black;
                    }

                    Gizmos.DrawCube(node.WorldPosition, Vector3.one * (_nodeDiameter - 0.1f));
                }
            }
        }

        public int MaxSize
        {
            get { return _gridSize.x * _gridSize.y * _gridSize.z; }
        }
    }
}
