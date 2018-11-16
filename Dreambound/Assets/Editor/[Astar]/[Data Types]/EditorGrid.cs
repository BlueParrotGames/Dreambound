using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dreambound.Astar.Editor
{
    public class EditorGrid
    {
        public readonly EditorNode[,,] Nodes;
        public readonly Vector3Int GridSize;

        public EditorGrid(EditorNode[,,] nodes, Vector3Int gridSize)
        {
            Nodes = nodes;
            GridSize = gridSize;
        }

        public List<EditorNode> GetNeigbours(EditorNode node)
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

                        int checkX = node.GridPosition.x + x;
                        int checkY = node.GridPosition.y + y;
                        int checkZ = node.GridPosition.z + z;

                        //Check if X,Y,Z are inside the grid
                        if ((checkX >= 0 && checkX < GridSize.x) && (checkY >= 0 && checkY < GridSize.y) && (checkZ >= 0 && checkZ < GridSize.z))
                        {
                            neighbours.Add(Nodes[checkX, checkY, checkZ]);
                        }
                    }
                }
            }

            return neighbours;
        }
    }
}