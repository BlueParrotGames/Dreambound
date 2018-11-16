using System.Collections.Generic;
using UnityEngine;

using Dreambound.Astar.Data;
using Dreambound.AI;

using Debug = UnityEngine.Debug;

namespace Dreambound.Astar
{
    public class Grid : MonoBehaviour
    {
        [SerializeField] private LayerMask _unwalkableMask;
        [SerializeField] private Vector3 _gridWorldSize;
        [SerializeField] private float _nodeRadius;

        [Header("Weight settings")]
        [SerializeField] private int _obstacleProximityPenalty;
        [SerializeField] private TerrainType[] _walkableRegions;

        [Header("Agent settings")]
        [SerializeField] private Agent[] _agents;
        [SerializeField] private float _agentHeight;
        [SerializeField] private float _agentMaxStepHeight;

        [Space]
        [SerializeField] private int _blurSize;

        private Vector3Int _gridSize;
        private Node[,,] _grid;
        private float _nodeDiameter;

        private LayerMask _walkableMask;
        private Dictionary<int, int> _walkableRegionsDic;

        private int _penaltyMin = int.MaxValue;
        private int _penaltyMax = int.MinValue;

        private Unit[] _units;

        private void Awake()
        {
            _units = FindObjectsOfType<Unit>();

            _nodeDiameter = _nodeRadius * 2f;
            _gridSize.x = Mathf.RoundToInt(_gridWorldSize.x / _nodeDiameter);
            _gridSize.y = Mathf.RoundToInt(_gridWorldSize.y / _nodeDiameter);
            _gridSize.z = Mathf.RoundToInt(_gridWorldSize.z / _nodeDiameter);

            //Make sure the _gridSize.y is always uneven
            if (_gridSize.y % 2 == 0)
            {
                _gridSize.y += 1;
                Debug.LogWarning("the Y axis of the 'Grid World Size' should always be uneven, it has been automatically corrected");
            }

            _walkableRegionsDic = new Dictionary<int, int>();
            foreach (TerrainType region in _walkableRegions)
            {
                _walkableMask.value = _walkableMask |= region.TerrainMask.value;

                int key = (int)Mathf.Log(region.TerrainMask.value, 2);
                _walkableRegionsDic.Add(key, region.TerrainPenalty);
            }

            GenerateGrid();
        }

        private void GenerateGrid()
        {
            _grid = new Node[_gridSize.x, _gridSize.y, _gridSize.z];

            //Calculate the position of the grid far bottom left corner
            Vector3 worldBottomLeft = Vector3.zero;
            worldBottomLeft = transform.position - Vector3.right * _gridWorldSize.x / 2f - Vector3.up * _gridWorldSize.y / 2f -
            worldBottomLeft - Vector3.forward * _gridWorldSize.z / 2f;


            for (int x = 0; x < _gridSize.x; x++)
            {
                for (int y = 0; y < _gridSize.y; y++)
                {
                    for (int z = 0; z < _gridSize.z; z++)
                    {
                        //Calculate the worldpoint of the current node
                        Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * _nodeDiameter + _nodeRadius) + Vector3.up * (y * _nodeDiameter + _nodeRadius) + Vector3.forward * (z * _nodeDiameter + _nodeRadius);

                        //Check if the node is within or too close to an obstacle
                        //If so set the node to non-walkable
                        bool walkable = !(Physics.CheckSphere(worldPoint, _nodeRadius, _unwalkableMask, QueryTriggerInteraction.Ignore));

                        //If the node is originaly walkable check if the passage way isn'y too low for the agent
                        //If it is set walkable to false
                        if (walkable)
                        {
                            if (Physics.Raycast(worldPoint, Vector3.up * _agentHeight, out RaycastHit hitUp))
                            {
                                if (Physics.Raycast(worldPoint, Vector3.down * _agentHeight, out RaycastHit hitDown))
                                {
                                    walkable = !(hitUp.point.y - hitDown.point.y < _agentHeight);
                                }
                            }
                        }

                        //Check if the node is a ground node
                        //If so set the nodes groundPosition
                        Ray groundRay = new Ray(worldPoint, Vector3.down);
                        Vector3 groundPosition = Vector3.zero;
                        bool groundNode = false;
                        if (Physics.Raycast(groundRay, out RaycastHit groundHit, _nodeDiameter))
                        {
                            groundNode = true;
                            groundPosition = groundHit.point;
                        }

                        //Find movement penalty
                        int movementPenalty = 0;
                        if (walkable)
                        {
                            Ray ray = new Ray(worldPoint + Vector3.up * 50, Vector3.down);
                            if (Physics.Raycast(ray, out RaycastHit penaltyHit, 100, _walkableMask))
                            {
                                _walkableRegionsDic.TryGetValue(penaltyHit.collider.gameObject.layer, out movementPenalty);
                            }
                        }
                        else if (!walkable)
                        {
                            movementPenalty += _obstacleProximityPenalty;
                        }

                        _grid[x, y, z] = new Node(walkable, worldPoint, x, y, z, movementPenalty, groundNode, groundPosition);
                    }
                }
            }

            CalculateSlopeStartNodes();
            BlurPenaltyMap();
        }
        private void CalculateSlopeStartNodes()
        {
            for (int x = 0; x < _gridSize.x; x++)
            {
                for (int y = 0; y < _gridSize.y; y++)
                {
                    for (int z = 0; z < _gridSize.z; z++)
                    {

                    }
                }
            }
        }
        private void BlurPenaltyMap()
        {
            int kernelSize = _blurSize * 2 - 1;
            int kernelExtends = (kernelSize - 1) / 2;

            for (int y = 0; y < _gridSize.y; y++)
            {
                for (int x = 0; x < _gridSize.x; x++)
                {
                    for (int z = 0; z < _gridSize.z; z++)
                    {
                        int totalPenalty = 0;
                        for (int rangeX = -kernelExtends; rangeX <= kernelExtends; rangeX++)
                        {
                            for (int rangeY = -kernelExtends; rangeY <= kernelExtends; rangeY++)
                            {
                                for (int rangeZ = -kernelExtends; rangeZ <= kernelExtends; rangeZ++)
                                {
                                    int sampleX = Mathf.Clamp(rangeX + x, 0, _gridSize.x - 1);
                                    int sampleY = Mathf.Clamp(rangeY + y, 0, _gridSize.y - 1);
                                    int sampleZ = Mathf.Clamp(rangeZ + z, 0, _gridSize.z - 1);

                                    totalPenalty += _grid[sampleX, sampleY, sampleZ].MovementPenalty;
                                }
                            }
                        }

                        int blurredPenalty = Mathf.RoundToInt((float)totalPenalty / (kernelSize * kernelSize * kernelSize));
                        _grid[x, y, z].MovementPenalty = blurredPenalty;

                        if (blurredPenalty > _penaltyMax)
                            _penaltyMax = blurredPenalty;

                        if (blurredPenalty < _penaltyMin)
                            _penaltyMin = blurredPenalty;
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
        public Node GetNodeFromWorldPoint(Vector3 worldPosition)
        {
            float percentX = (worldPosition.x + _gridWorldSize.x / 2) / _gridWorldSize.x;
            float percentY = (worldPosition.y + _gridWorldSize.y / 2) / _gridWorldSize.y;
            float percentZ = (worldPosition.z + _gridWorldSize.z / 2) / _gridWorldSize.z;

            percentX = Mathf.Clamp01(percentX);
            percentY = Mathf.Clamp01(percentY);
            percentZ = Mathf.Clamp01(percentZ);

            int x = Mathf.RoundToInt((_gridSize.x - 1) * percentX);
            int y = Mathf.RoundToInt((_gridSize.y - 1) * percentY);
            int z = Mathf.RoundToInt((_gridSize.z - 1) * percentZ);

            return _grid[x, y, z];
        }

        private void OnDrawGizmos()
        {
            if (_grid != null)
            {
                for (int x = 0; x < _gridSize.x; x++)
                {
                    for (int y = 0; y < _gridSize.y; y++)
                    {
                        for (int z = 0; z < _gridSize.z; z++)
                        {
                            Gizmos.color = Color.gray;
                            Gizmos.color = (_grid[x, y, z].GroundNode) ? Color.blue : Gizmos.color;
                            Gizmos.color = (_grid[x, y, z].SlopeAccessNode) ? Color.yellow : Gizmos.color;
                            Gizmos.color = (_grid[x, y, z].Walkable) ? Gizmos.color : Color.red;

                            if (!_grid[x, y, z].Walkable || _grid[x, y, z].GroundNode || _grid[x,y,z].SlopeAccessNode)
                                Gizmos.DrawCube(_grid[x, y, z].WorldPosition, Vector3.one * (_nodeDiameter - 0.1f));
                        }
                    }
                }

                for(int i = 0; i < _units.Length; i++)
                {
                    Gizmos.color = Color.green;

                    Gizmos.DrawCube(_units[i].transform.position, Vector3.one * (_nodeDiameter - 0.1f));
                }
            }
        }

        public int MaxSize
        {
            get { return _gridSize.x * _gridSize.y * _gridSize.z; }
        }
        public Node[,,] Nodes
        {
            get { return _grid; }
        }
    }
}