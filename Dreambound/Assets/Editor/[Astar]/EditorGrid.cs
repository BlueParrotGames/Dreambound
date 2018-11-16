using UnityEngine;
using UnityEditor;

using Grid = Dreambound.Astar.Grid;
using Node = Dreambound.Astar.Node;

namespace Dreambound.Astar.Editor
{
    [CustomEditor(typeof(Grid))]
    public class EditorGrid : UnityEditor.Editor
    {
        private GameObject _targetObject;
        private Grid _grid;
        private static Node[,,] _nodes;

        private static Vector3Int _gridSize;
        private static float _nodeDiameter;

        private static EditorGrid _instance;

        [DrawGizmo(GizmoType.Selected | GizmoType.NonSelected)]
        private static void DrawGizmosSelected(Grid grid, GizmoType gizmoType)
        {
            //just use gismos here
        }

        private void OnSceneGUI()
        {
            _targetObject = ((Grid)target).gameObject;
            _instance = this;
        }

        public static void GenerateGrid()
        {
            _nodes = EditorGridGenerator.GenerateGrid(_instance.GetGenerationSettings());
        }

        GridGenerateSettings GetGenerationSettings()
        {
            //Get all the variables needed for generation
            LayerMask unwalkableMask = serializedObject.FindProperty("_unwalkableMask").intValue;
            Vector3 gridWorldSize = serializedObject.FindProperty("_gridWorldSize").vector3Value;
            float nodeRadius = serializedObject.FindProperty("_nodeRadius").floatValue;
            int blurSize = serializedObject.FindProperty("_blurSize").intValue;

            int obstacleProximityPenalty = serializedObject.FindProperty("_obstacleProximityPenalty").intValue;

            TerrainType[] walkableRegions = new TerrainType[serializedObject.FindProperty("_walkableRegions").arraySize];
            for(int i = 0; i < walkableRegions.Length; i++)
            {
                walkableRegions[i] = new TerrainType
                {
                    TerrainMask = serializedObject.FindProperty("_walkableRegions").GetArrayElementAtIndex(i).FindPropertyRelative("TerrainMask").intValue,
                    TerrainPenalty = serializedObject.FindProperty("_walkableRegions").GetArrayElementAtIndex(i).FindPropertyRelative("TerrainPenalty").intValue,
                };
            }

            //Set the gridsize
            _nodeDiameter = nodeRadius * 2f;
            _gridSize.x = Mathf.RoundToInt(gridWorldSize.x / _nodeDiameter);
            _gridSize.y = Mathf.RoundToInt(gridWorldSize.y / _nodeDiameter);
            _gridSize.z = Mathf.RoundToInt(gridWorldSize.z / _nodeDiameter);

            return new GridGenerateSettings(unwalkableMask, gridWorldSize, nodeRadius, blurSize, obstacleProximityPenalty, walkableRegions, _targetObject.transform);
        }
    }
}
