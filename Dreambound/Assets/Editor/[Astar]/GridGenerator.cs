using System.Threading;
using System.Collections.Generic;
using UnityEngine;

namespace Dreambound.Astar.Editor
{
    public class GridGenerator
    {
        private static Node[,,] _grid;

        private static Vector3Int _gridSize;
        private static GenerationSettings _generationSettings;

        public static Node[,,] GenerateGrid(GenerationSettings generationSettings, TerrainType[] terrainTypes)
        {
            _generationSettings = generationSettings;

            #region Additional Settings
            float nodeDiameter = generationSettings.NodeRadius * 2f;

            _gridSize.x = Mathf.RoundToInt(generationSettings.GridWorldSize.x / nodeDiameter);
            _gridSize.y = Mathf.RoundToInt(generationSettings.GridWorldSize.y / nodeDiameter);
            _gridSize.z = Mathf.RoundToInt(generationSettings.GridWorldSize.z / nodeDiameter);

            LayerMask walkableMask = 0;
            Dictionary<int, int> walkableRegions = new Dictionary<int, int>();
            foreach (TerrainType terrainType in terrainTypes)
            {
                walkableMask.value = walkableMask |= terrainType.TerrainMask.value;

                int key = (int)Mathf.Log(terrainType.TerrainMask.value, 2);
                walkableRegions.Add(key, terrainType.TerrainPenalty);
            }
            #endregion

            Vector3 worldBottomLeft = Vector3.zero;
            worldBottomLeft = Vector3.zero - Vector3.right * generationSettings.GridWorldSize.x / 2f - Vector3.up * generationSettings.GridWorldSize.y / 2f - Vector3.forward * generationSettings.GridWorldSize.z / 2f;

            _grid = new Node[_gridSize.x, _gridSize.y, _gridSize.z];

            for (int x = 0; x < _gridSize.x; x++)
            {
                for (int y = 0; y < _gridSize.y; y++)
                {
                    for (int z = 0; z < _gridSize.z; z++)
                    {
                        //Calculate the world position of the current node
                        Vector3 worldPoint = worldBottomLeft +
                            Vector3.right * (x * nodeDiameter + generationSettings.NodeRadius) +
                            Vector3.up * (y * nodeDiameter + generationSettings.NodeRadius) +
                            Vector3.forward * (z * nodeDiameter + generationSettings.NodeRadius);

                        //Check if the current node is a useless floating node
                        if (Physics.CheckSphere(worldPoint, generationSettings.NodeRadius))
                        {
                            _grid[x, y, z] = null;
                            continue;
                        }

                        //Check if a node is too close to an obstacle, if so the node is unwalkable
                        bool walkable = !Physics.CheckSphere(worldPoint, generationSettings.AgentRadius, LayerMask.NameToLayer("Unwalkable"), QueryTriggerInteraction.Ignore);

                        //Check if the current passage way is to low for the agent to go through
                        if (walkable)
                        {
                            if (Physics.Raycast(worldPoint, Vector3.up * generationSettings.AgentHeight, out RaycastHit hitUp))
                            {
                                if (Physics.Raycast(worldPoint, Vector3.down * generationSettings.AgentHeight, out RaycastHit hitDown))
                                {
                                    walkable = !(hitUp.point.y - hitDown.point.y < generationSettings.AgentHeight);
                                }
                            }
                        }

                        //Check if the node is a ground node
                        bool groundNode = false;
                        Vector3 groundPosition = Vector3.zero;
                        if (Physics.Raycast(worldPoint, Vector3.down, out RaycastHit groundHit, nodeDiameter))
                        {
                            groundNode = true;
                            groundPosition = groundHit.point;
                        }

                        //Find the movement penalty of the current node
                        int movementPenalty = 0;
                        if (walkable)
                        {
                            Ray ray = new Ray(worldPoint + Vector3.up * 50, Vector3.down);
                            if (Physics.Raycast(ray, out RaycastHit penaltyHit, 100, walkableMask))
                            {
                                walkableRegions.TryGetValue(penaltyHit.collider.gameObject.layer, out movementPenalty);
                            }
                        }
                        else
                        {
                            movementPenalty += generationSettings.ObstacleProximityPenalty;
                        }

                        _grid[x, y, z] = new Node(walkable, worldPoint, x, y, z, groundPosition, movementPenalty, groundNode);

                    }
                }
            }
            Thread blurThread = new Thread(BlurPenaltyMap);
            blurThread.Start();

            return _grid;
        }
        private static void BlurPenaltyMap()
        {
            int kernelSize = _generationSettings.BlurSize * 2 - 1;
            int kernelExtends = (kernelSize - 1) / 2;

            for(int y = 0; y < _gridSize.y; y++)
            {
                for (int x = 0; x < _gridSize.x; x++)
                {
                    for (int z = 0; z < _gridSize.z; z++)
                    {
                        if (_grid[x,y,z] != null)
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

                                        if (_grid[sampleX, sampleY, sampleZ] != null)
                                            totalPenalty += _grid[sampleX, sampleY, sampleZ].MovementPenalty;
                                    }
                                }
                            }

                            int blurredPenalty = Mathf.RoundToInt((float)totalPenalty / (kernelSize * kernelSize * kernelSize));
                            _grid[x, y, z].MovementPenalty = blurredPenalty;
                        }
                    }
                }
            }
        }
    }
}