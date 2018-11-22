using UnityEngine;
using UnityEditor;

namespace Dreambound.Astar
{
    [System.Serializable]
    public class TerrainType
    {
        public LayerMask TerrainMask;
        public int TerrainPenalty;
    }
}

namespace Dreambound.Astar.Editor
{
    [CreateAssetMenu(fileName = "TerrainTypes.Asset", menuName = "Dreambound/Terrain Types")]
    public class TerrainTypes : ScriptableObject
    {
        public TerrainType[] WalkableTerrainTypes;
    }
}
