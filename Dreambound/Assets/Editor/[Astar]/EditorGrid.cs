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
        private Node[,,] _nodes;

        [DrawGizmo(GizmoType.Selected | GizmoType.NonSelected)]
        static void DrawGizmosSelected(Grid grid, GizmoType gizmoType)
        {
        }

        private void OnSceneGUI()
        {
            _grid = target as Grid;
            _targetObject = _grid.gameObject;

            _nodes = EditorGridGenerator.GenerateGrid(GetGenerationSettings());
        }

        private GridGenerateSettings GetGenerationSettings()
        {
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

            return new GridGenerateSettings(unwalkableMask, gridWorldSize, nodeRadius, blurSize, obstacleProximityPenalty, walkableRegions, _targetObject.transform);
        }
    }
}
