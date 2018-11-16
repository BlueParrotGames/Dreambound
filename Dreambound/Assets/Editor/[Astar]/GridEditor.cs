using UnityEngine;
using UnityEditor;

using Grid = Dreambound.Astar.Grid;
using Node = Dreambound.Astar.Node;

namespace Dreambound.Astar.Editor
{
    [CustomEditor(typeof(Grid))]
    public class GridEditor : UnityEditor.Editor
    {
        private GameObject _targetObject;
        private EditorGrid _grid;

        private Vector3Int _gridSize;
        private Vector3 _gridWorldSize;
        private float _nodeDiameter;

        private static GridEditor _instance;

        public static void GenerateGrid()
        {
            if (_instance == null)
                _instance = (GridEditor)CreateEditor(FindObjectOfType<Grid>());

            _instance._grid = EditorGridGenerator.GenerateGrid(_instance.GetGenerationSettings());
        }
        private GridGenerateSettings GetGenerationSettings()
        {
            //Get all the variables needed for generation
            LayerMask unwalkableMask = serializedObject.FindProperty("_unwalkableMask").intValue;
            _gridWorldSize = serializedObject.FindProperty("_gridWorldSize").vector3Value;
            float nodeRadius = serializedObject.FindProperty("_nodeRadius").floatValue;
            int blurSize = serializedObject.FindProperty("_blurSize").intValue;

            int obstacleProximityPenalty = serializedObject.FindProperty("_obstacleProximityPenalty").intValue;

            TerrainType[] walkableRegions = new TerrainType[serializedObject.FindProperty("_walkableRegions").arraySize];
            for (int i = 0; i < walkableRegions.Length; i++)
            {
                walkableRegions[i] = new TerrainType
                {
                    TerrainMask = serializedObject.FindProperty("_walkableRegions").GetArrayElementAtIndex(i).FindPropertyRelative("TerrainMask").intValue,
                    TerrainPenalty = serializedObject.FindProperty("_walkableRegions").GetArrayElementAtIndex(i).FindPropertyRelative("TerrainPenalty").intValue,
                };
            }

            //Set the gridsize
            _nodeDiameter = nodeRadius * 2f;
            _gridSize.x = Mathf.RoundToInt(_gridWorldSize.x / _nodeDiameter);
            _gridSize.y = Mathf.RoundToInt(_gridWorldSize.y / _nodeDiameter);
            _gridSize.z = Mathf.RoundToInt(_gridWorldSize.z / _nodeDiameter);

            if (_targetObject == null)
                _targetObject = ((Grid)target).gameObject;

            return new GridGenerateSettings(unwalkableMask, _gridWorldSize, nodeRadius, blurSize, obstacleProximityPenalty, walkableRegions, _targetObject.transform);
        }

        [DrawGizmo(GizmoType.Selected | GizmoType.NonSelected)]
        private static void DrawGizmosSelected(Grid grid, GizmoType gizmoType)
        {
            if (_instance == null)
            {
                _instance = (GridEditor)CreateEditor(FindObjectOfType<Grid>());

                if (_instance._targetObject == null)
                    _instance._targetObject = FindObjectOfType<Grid>().gameObject;
            }

            if(_instance._grid != null)
            {
                //Draw the outline of the grid
                Gizmos.DrawWireCube(_instance._targetObject.transform.position, _instance._gridWorldSize);

                for(int x = 0; x < _instance._gridWorldSize.x; x++)
                {
                    for(int y = 0; y < _instance._gridWorldSize.y; y++)
                    {
                        for(int z = 0; z < _instance._gridWorldSize.z; z++)
                        {
                            if (_instance._grid.Nodes[x, y, z].IsEdgeNode && !_instance._grid.Nodes[x,y,z].IsFloatingNode)
                            {
                                Gizmos.color = Color.blue;
                                Gizmos.DrawCube(_instance._grid.Nodes[x, y, z].WorldPosition, Vector3.one * (_instance._nodeDiameter - 0.1f));
                            }
                        }
                    }
                }
            }
        }
    }
}
