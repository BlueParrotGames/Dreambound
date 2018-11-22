namespace Dreambound.Astar
{
    [System.Serializable]
    public class GridFile
    {
        public readonly Node[,,] Grid;

        public GridFile(Node[,,] grid)
        {
            Grid = grid;
        }
    }
}
