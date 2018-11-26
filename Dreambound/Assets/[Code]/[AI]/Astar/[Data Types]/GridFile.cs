namespace Dreambound.Astar
{
    [System.Serializable]
    public class GridFile : UnityEngine.ScriptableObject
    {
        public readonly Node[,,] Grid;

        public GridFile(Node[,,] grid)
        {
            Grid = grid;
        }
    }
}
