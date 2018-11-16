using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dreambound.Astar.Editor
{
    public class EditorNode
    {
        public bool Walkable;
        public bool IsEdgeNode;

        public readonly Vector3 WorldPosition;
        public readonly Vector3Int GridPosition;
        public readonly bool IsFloatingNode;

        public EditorNode Parent;

        public EditorNode(bool walkable, Vector3 worldPosition, Vector3Int gridPosition, bool isFloatingNode)
        {
            Walkable = walkable;
            WorldPosition = worldPosition;
            GridPosition = gridPosition;
            IsFloatingNode = isFloatingNode;
        }
        public EditorNode(bool walkable, Vector3 worldPosition, int x, int y, int z, bool isFloatingNode)
        {
            Walkable = walkable;
            WorldPosition = worldPosition;
            GridPosition = new Vector3Int(x, y, z);
            IsFloatingNode = isFloatingNode;
        }
    }
}
