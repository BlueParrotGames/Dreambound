using UnityEngine;

namespace Dreambound.Astar
{
    public class Node : IHeapItem<Node>
    {
        public bool Walkable;
        public Vector3 WorldPosition;
        public Vector3Int GridPosition;
        public int MovementPenalty;

        public int gCost;
        public int hCost;
        public Node Parent;

        private int _heapIndex;
        public int HeapIndex { get => _heapIndex; set => _heapIndex = value; }

        public Node(bool walkable, Vector3 worldPosition, Vector3Int gridPosition, int penalty)
        {
            Walkable = walkable;
            WorldPosition = worldPosition;
            GridPosition = gridPosition;
            MovementPenalty = penalty;
        }
        public Node(bool walkable, Vector3 worldPosition, int x, int y, int z, int penalty)
        {
            Walkable = walkable;
            WorldPosition = worldPosition;
            GridPosition = new Vector3Int(x, y ,z);
            MovementPenalty = penalty;
        }

        public int fCost
        {
            get { return gCost + hCost; }
        }

        public int CompareTo(Node node)
        {
            int compare = fCost.CompareTo(node.fCost);
            if (compare == 0)
            {
                compare = hCost.CompareTo(node.hCost);
            }
            return -compare;
        }
    }
}
