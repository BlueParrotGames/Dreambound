using System.Threading;
using System.Collections.Generic;
using UnityEngine;

namespace Dreambound.Astar.Editor
{
    public class EditorGridGenerator
    {
        private static Node[,,] _grid;
        private static GridGenerateSettings _previousGenerationSettings;
        private static ThreadStart _generationThread;

        public static Node[,,] GenerateGrid(GridGenerateSettings generateSettings)
        {

            if (_generationThread == null)
                _generationThread = delegate
                {
                    GenerateNodes(out _grid, generateSettings);
                };

            if (_previousGenerationSettings == generateSettings)
                return _grid;

            _generationThread.Invoke();

            _previousGenerationSettings = generateSettings;

            return _grid;
        }

        private static void GenerateNodes(out Node[,,] nodes, GridGenerateSettings generateSettings)
        {
            float nodeDiameter = generateSettings.NodeRadius * 2f;

            Vector3Int gridSize = Vector3Int.zero;
            gridSize.x = Mathf.RoundToInt(generateSettings.GridWorldSize.x / nodeDiameter);
            gridSize.y = Mathf.RoundToInt(generateSettings.GridWorldSize.y / nodeDiameter);
            gridSize.z = Mathf.RoundToInt(generateSettings.GridWorldSize.z / nodeDiameter);

            LayerMask walkableMask = 0;
            Dictionary<int, int> _walkableReagionsDictionary;

            if (gridSize.y % 2 == 0)
                gridSize.y += 1;

            _walkableReagionsDictionary = new Dictionary<int, int>();
            foreach (TerrainType region in generateSettings.WalkableRegions)
            {
                walkableMask.value = walkableMask |= region.TerrainMask.value;

                int key = (int)Mathf.Log(region.TerrainMask.value, 2);
                _walkableReagionsDictionary.Add(key, region.TerrainPenalty);
            }

            nodes = new Node[gridSize.x, gridSize.y, gridSize.z];

            //Calculate the position of the grid far bottom left corner
            Vector3 worldBottomLeft = Vector3.zero;
            worldBottomLeft = generateSettings.Object.position - Vector3.right * generateSettings.GridWorldSize.x / 2f - Vector3.up * generateSettings.GridWorldSize.y / 2f -
            worldBottomLeft - Vector3.forward * generateSettings.GridWorldSize.z / 2f;

            for (int x = 0; x < gridSize.x; x++)
            {
                for (int y = 0; y < gridSize.y; y++)
                {
                    for (int z = 0; z < gridSize.z; z++)
                    {
                        //Calculate the worldpoint of the current node
                        Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + generateSettings.NodeRadius) + Vector3.up * (y * nodeDiameter + generateSettings.NodeRadius) + Vector3.forward * (z * nodeDiameter + generateSettings.NodeRadius);

                        bool walkable = !(Physics.CheckSphere(worldPoint, generateSettings.NodeRadius, generateSettings.UnwalkableMask, QueryTriggerInteraction.Ignore));
                        bool floatingNode = !Physics.CheckSphere(worldPoint, generateSettings.NodeRadius);

                        //Find movement penalty
                        int movementPenalty = 0;
                        if (walkable)
                        {
                            Ray ray = new Ray(worldPoint + Vector3.up * 50, Vector3.down);
                            RaycastHit hit;
                            if (Physics.Raycast(ray, out hit, 100, walkableMask))
                            {
                                _walkableReagionsDictionary.TryGetValue(hit.collider.gameObject.layer, out movementPenalty);
                            }
                        }
                        else if (!walkable)
                        {
                            movementPenalty += generateSettings.ObstacleProximityPenalty;
                        }

                        nodes[x, y, z] = new Node(walkable, worldPoint, x, y, z, movementPenalty);
                    }
                }
            }

            BlurPenaltyMap(generateSettings.BlurSize, gridSize, ref nodes);
        }
        private static void BlurPenaltyMap(int blurSize, Vector3Int gridSize, ref Node[,,] grid)
        {
            int kernelSize = blurSize * 2 - 1;
            int kernelExtends = (kernelSize - 1) / 2;

            for (int y = 0; y < gridSize.y; y++)
            {
                for (int x = 0; x < gridSize.x; x++)
                {
                    for (int z = 0; z < gridSize.z; z++)
                    {
                        int totalPenalty = 0;
                        for (int rangeX = -kernelExtends; rangeX <= kernelExtends; rangeX++)
                        {
                            for (int rangeY = -kernelExtends; rangeY <= kernelExtends; rangeY++)
                            {
                                for (int rangeZ = -kernelExtends; rangeZ <= kernelExtends; rangeZ++)
                                {
                                    int sampleX = Mathf.Clamp(rangeX + x, 0, gridSize.x - 1);
                                    int sampleY = Mathf.Clamp(rangeY + y, 0, gridSize.y - 1);
                                    int sampleZ = Mathf.Clamp(rangeZ + z, 0, gridSize.z - 1);

                                    totalPenalty += grid[sampleX, sampleY, sampleZ].MovementPenalty;
                                }
                            }
                        }

                        int blurredPenalty = Mathf.RoundToInt((float)totalPenalty / (kernelSize * kernelSize * kernelSize));
                        grid[x, y, z].MovementPenalty = blurredPenalty;
                    }
                }
            }
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