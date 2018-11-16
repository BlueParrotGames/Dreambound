using System.Threading;
using System.Collections.Generic;
using UnityEngine;

namespace Dreambound.Astar.Editor
{
    public class EditorGridGenerator
    {
        private static EditorGrid _grid;
        private static Vector3Int _gridSize;

        private static GridGenerateSettings _previousGenerationSettings;
        private static ThreadStart _generationThread;

        public static EditorGrid GenerateGrid(GridGenerateSettings generateSettings)
        {

            if (_generationThread == null)
                _generationThread = delegate
                {
                    GenerateNodes(generateSettings);
                };

            if (_previousGenerationSettings == generateSettings)
                return _grid;

            _generationThread.Invoke();

            _previousGenerationSettings = generateSettings;

            return _grid;
        }

        private static void GenerateNodes(GridGenerateSettings generateSettings)
        {
            float nodeDiameter = generateSettings.NodeRadius * 2f;

            _gridSize.x = Mathf.RoundToInt(generateSettings.GridWorldSize.x / nodeDiameter);
            _gridSize.y = Mathf.RoundToInt(generateSettings.GridWorldSize.y / nodeDiameter);
            _gridSize.z = Mathf.RoundToInt(generateSettings.GridWorldSize.z / nodeDiameter);

            LayerMask walkableMask = 0;
            Dictionary<int, int> _walkableReagionsDictionary;

            if (_gridSize.y % 2 == 0)
                _gridSize.y += 1;

            _walkableReagionsDictionary = new Dictionary<int, int>();
            foreach (TerrainType region in generateSettings.WalkableRegions)
            {
                walkableMask.value = walkableMask |= region.TerrainMask.value;

                int key = (int)Mathf.Log(region.TerrainMask.value, 2);
                _walkableReagionsDictionary.Add(key, region.TerrainPenalty);
            }

            EditorNode[,,] nodes = new EditorNode[_gridSize.x, _gridSize.y, _gridSize.z];

            //Calculate the position of the grid far bottom left corner
            Vector3 worldBottomLeft = Vector3.zero;
            worldBottomLeft = generateSettings.Object.position - Vector3.right * generateSettings.GridWorldSize.x / 2f - Vector3.up * generateSettings.GridWorldSize.y / 2f -
            worldBottomLeft - Vector3.forward * generateSettings.GridWorldSize.z / 2f;

            for (int x = 0; x < _gridSize.x; x++)
            {
                for (int y = 0; y < _gridSize.y; y++)
                {
                    for (int z = 0; z < _gridSize.z; z++)
                    {
                        //Calculate the worldpoint of the current node
                        Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + generateSettings.NodeRadius) + Vector3.up * (y * nodeDiameter + generateSettings.NodeRadius) + Vector3.forward * (z * nodeDiameter + generateSettings.NodeRadius);

                        bool walkable = !(Physics.CheckSphere(worldPoint, generateSettings.NodeRadius, generateSettings.UnwalkableMask, QueryTriggerInteraction.Ignore));
                        bool floatingNode = !Physics.CheckSphere(worldPoint, generateSettings.NodeRadius);

                        nodes[x, y, z] = new EditorNode(walkable, worldPoint, x, y, z, floatingNode);
                    }
                }
            }

            CalculateEdgeNodes(ref nodes);

            _grid = new EditorGrid(nodes, _gridSize);
        }

        private static void CalculateEdgeNodes(ref EditorNode[,,] nodes)
        {
            for (int x = 0; x < _gridSize.x; x++)
            {
                for (int y = 0; y < _gridSize.y; y++)
                {
                    for (int z = 0; z < _gridSize.z; z++)
                    {
                        if (!nodes[x, y, z].IsFloatingNode)
                        {
                            List<EditorNode> neighbours2D = Get2DNodeNeigbours(x, y, z, nodes);
                            List<EditorNode> neighbours3D = Get3DNodeNeigbours(x, y, z, nodes);

                            //Get the Outer-edge nodes
                            if (neighbours2D.Count < 6)
                                nodes[x, y, z].IsEdgeNode = true;

                            //Get the Inner-edge nodes
                            for (int i = 0; i < neighbours3D.Count; i++)
                            {
                                if (!neighbours3D[i].Walkable)
                                {
                                    nodes[x, y, z].IsEdgeNode = true;
                                    continue;
                                }
                            }
                        }
                    }
                }
            }
        }
        private static List<EditorNode> Get3DNodeNeigbours(int nodeX, int nodeY, int nodeZ, EditorNode[,,] nodes)
        {
            List<EditorNode> neighbours = new List<EditorNode>();

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    for (int z = -1; z <= 1; z++)
                    {
                        if (x == 0 && y == 0 && z == 0)
                            continue;

                        int checkX = nodeX + x;
                        int checkY = nodeY + y;
                        int checkZ = nodeZ + z;

                        //Check if X,Y,Z are inside the grid
                        if ((checkX >= 0 && checkX < _gridSize.x) && (checkY >= 0 && checkY < _gridSize.y) && (checkZ >= 0 && checkZ < _gridSize.z))
                        {
                            neighbours.Add(nodes[checkX, checkY, checkZ]);
                        }
                    }
                }
            }

            return neighbours;
        }
        private static List<EditorNode> Get2DNodeNeigbours(int nodeX, int nodeY, int nodeZ, EditorNode[,,] nodes)
        {
            List<EditorNode> neighbours = new List<EditorNode>();

            for (int x = -1; x <= 1; x++)
            {
                for (int z = -1; z <= 1; z++)
                {
                    if (x == 0 && z == 0)
                        continue;

                    int checkX = nodeX + x;
                    int checkZ = nodeZ + z;

                    //Check if X,Y,Z are inside the grid
                    if ((checkX >= 0 && checkX < _gridSize.x) && (checkZ >= 0 && checkZ < _gridSize.z))
                    {
                        neighbours.Add(nodes[checkX, nodeY, checkZ]);
                    }
                }
            }

            return neighbours;
        }
    }

    public struct GridGenerateSettings
    {
        //General Settings
        public readonly LayerMask UnwalkableMask;
        public readonly Vector3 GridWorldSize;
        public readonly float NodeRadius;
        public readonly int BlurSize;

        //Weight Settings
        public readonly int ObstacleProximityPenalty;
        public readonly TerrainType[] WalkableRegions;

        //Object info
        public readonly Transform Object;

        public GridGenerateSettings(LayerMask unwalkableMask, Vector3 gridWorldSize, float nodeRadius, int blurSize, int obstacleProximity, TerrainType[] walkableRegions, Transform obj)
        {
            UnwalkableMask = unwalkableMask;
            GridWorldSize = gridWorldSize;
            NodeRadius = nodeRadius;
            BlurSize = blurSize;

            ObstacleProximityPenalty = obstacleProximity;
            WalkableRegions = walkableRegions;

            Object = obj;
        }

        public static bool operator ==(GridGenerateSettings instance1, GridGenerateSettings instance2)
        {
            bool unwalkableCheck = instance1.UnwalkableMask == instance2.UnwalkableMask;
            bool gridWorldSizeCheck = instance1.GridWorldSize == instance2.GridWorldSize;
            bool nodeRadiusCheck = instance1.NodeRadius == instance2.NodeRadius;
            bool proximityCheck = instance1.ObstacleProximityPenalty == instance2.ObstacleProximityPenalty;
            bool walkableCheck = instance1.WalkableRegions == instance2.WalkableRegions;
            bool targetCheck = instance1.Object == instance2.Object;

            return (unwalkableCheck && gridWorldSizeCheck && nodeRadiusCheck && proximityCheck && walkableCheck && targetCheck);
        }
        public static bool operator !=(GridGenerateSettings instance1, GridGenerateSettings instance2)
        {
            bool unwalkableCheck = instance1.UnwalkableMask != instance2.UnwalkableMask;
            bool gridWorldSizeCheck = instance1.GridWorldSize != instance2.GridWorldSize;
            bool nodeRadiusCheck = instance1.NodeRadius != instance2.NodeRadius;
            bool proximityCheck = instance1.ObstacleProximityPenalty != instance2.ObstacleProximityPenalty;
            bool walkableCheck = instance1.WalkableRegions != instance2.WalkableRegions;
            bool targetCheck = instance1.Object != instance2.Object;

            return (unwalkableCheck || gridWorldSizeCheck || nodeRadiusCheck || proximityCheck || walkableCheck || targetCheck);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}