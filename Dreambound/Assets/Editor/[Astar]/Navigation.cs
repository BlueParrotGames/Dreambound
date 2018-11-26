using System;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace Dreambound.Astar.Editor
{
    public class Navigation : EditorWindow
    {
        //General settings
        private Vector3 _gridWorldSize;
        private float _nodeRadius;

        //Weight settings
        private int _obstacleProximityPenalty;
        private TerrainTypes _terrainTypes;

        //Agent Settings
        private float _agentHeight;
        private float _agentRadius;
        private float _agentJumpHeight;

        //Baking Settings
        private int _blurSize;
        private Vector3 _colliderSizeMultiplier = new Vector3(1.2f, 1.2f, 1.2f);
        private float _radiusSizeMultiplier = 1.2f;

        private GenerationSettings _currentSettings;
        private GameObject _colliderParent;

        private static Navigation _instance;

        [MenuItem("Window/Dreambound/AI/Navigation")]
        private static void InitializeWindow()
        {
            //Store the currently selected object and select the grid gameObject to avoid errors
            GameObject currentSelection = Selection.activeGameObject;
            Selection.activeGameObject = FindObjectOfType<Grid>().gameObject;

            //Show the window
            _instance = (Navigation)GetWindow(typeof(Navigation), false, "A* Navigation", true);
            _instance.Show();

            //Reselect the stored gameObject
            Selection.activeGameObject = currentSelection;

            _instance.LoadGenerationSettings();
        }

        private void OnGUI()
        {
            //General Settings
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("General Settings", EditorStyles.boldLabel);
            _gridWorldSize = EditorGUILayout.Vector3Field(new GUIContent("Grid World Size"), _gridWorldSize);
            _nodeRadius = EditorGUILayout.FloatField(new GUIContent("Node Radius"), _nodeRadius);

            //Weight Settings
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Weight Settings", EditorStyles.boldLabel);
            _obstacleProximityPenalty = EditorGUILayout.IntField(new GUIContent("Obstacle Proximity Penalty"), _obstacleProximityPenalty);
            _terrainTypes = (TerrainTypes)EditorGUILayout.ObjectField(new GUIContent("Terrain Types"), _terrainTypes, typeof(TerrainTypes), false);

            //Agent Settings
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Agent Settings", EditorStyles.boldLabel);
            _agentHeight = EditorGUILayout.FloatField(new GUIContent("Agent Height"), _agentHeight);
            _agentRadius = EditorGUILayout.FloatField(new GUIContent("Agent Radius"), _agentRadius);
            _agentJumpHeight = EditorGUILayout.FloatField(new GUIContent("Agent Jump Height"), _agentJumpHeight);

            //Baking Settings
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Baking Settings", EditorStyles.boldLabel);
            _blurSize = EditorGUILayout.IntField(new GUIContent("Grid Blur Size"), _blurSize);
            _colliderSizeMultiplier = EditorGUILayout.Vector3Field(new GUIContent("Collider Size Multiplier", "The multiplier for the size of the original box colliders"), _colliderSizeMultiplier);
            _radiusSizeMultiplier = EditorGUILayout.FloatField(new GUIContent("Collider Radius Multiplier", "The multiplier for the radius of the original sphere colliders"), _radiusSizeMultiplier);

            EditorGUILayout.Space();
            if (GUILayout.Button("Generate Grid"))
            {
                SaveGenerationSettings();
                GenerateBakingColliders();
                SaveGridFile(GridGenerator.GenerateGrid(_currentSettings, _terrainTypes.WalkableTerrainTypes));
                DestroyBakingColliders();
            }
        }

        private void LoadGenerationSettings()
        {
            if (AssetDatabase.IsValidFolder("Assets/Resources/[Navigation]/[NavSettings]"))
            {
                _currentSettings = (GenerationSettings)AssetDatabase.LoadAssetAtPath("Assets/Resources/[Navigation]/[NavSettings]/" + GetCurrentSceneName() + "_NavGenSettings.Asset", typeof(GenerationSettings));

                _gridWorldSize = _currentSettings.GridWorldSize;
                _nodeRadius = _currentSettings.NodeRadius;

                _obstacleProximityPenalty = _currentSettings.ObstacleProximityPenalty;

                _agentHeight = _currentSettings.AgentHeight;
                _agentRadius = _currentSettings.AgentRadius;
                _agentJumpHeight = _currentSettings.AgentJumpHeight;

                _blurSize = _currentSettings.BlurSize;
                _colliderSizeMultiplier = _currentSettings.ColliderSizeMultiplier;
                _radiusSizeMultiplier = _currentSettings.ColliderRadiusMultiplier;
            }
        }
        private void SaveGenerationSettings()
        {
            _currentSettings = new GenerationSettings(_gridWorldSize, _nodeRadius, _obstacleProximityPenalty, _agentHeight, _agentRadius, _agentJumpHeight, _blurSize, _colliderSizeMultiplier, _radiusSizeMultiplier);

            if (!AssetDatabase.IsValidFolder("Assets/Resources/[Navigation]"))
                AssetDatabase.CreateFolder("Assets/Resources", "[Navigation]");

            if (!AssetDatabase.IsValidFolder("Assets/Resources/[Navigation]/[NavSettings]"))
                AssetDatabase.CreateFolder("Assets/Resources/[Navigation]", "[NavSettings]");

            AssetDatabase.CreateAsset(_currentSettings, "Assets/Resources/[Navigation]/[NavSettings]/" + GetCurrentSceneName() + "_NavGenSettings.Asset");
        }
        private string GetCurrentSceneName()
        {
            return EditorSceneManager.GetActiveScene().name;
        }

        private void SaveGridFile(Node[,,] grid)
        {
            if (!AssetDatabase.IsValidFolder("Assets/Resources/[Navigation]/[Grid Files]"))
                AssetDatabase.CreateFolder("Assets/Resources/[Navigation]", "[Grid Files]");

            GridFile gridFile = new GridFile(grid);

            //Something is going wrong with the reading or writing of the file
            AssetDatabase.CreateAsset(gridFile, "Assets/Resources/[Navigation]/[Grid Files]/" + GetCurrentSceneName() + "_NavGrid.Asset");
        }

        private void GenerateBakingColliders()
        {
            _colliderParent = new GameObject("Temp");

            Collider[] unwalkableColliders = FindObjectsOfType<Collider>().Where(x => GameObjectUtility.GetStaticEditorFlags(x.gameObject).HasFlag(StaticEditorFlags.NavigationStatic)).ToArray();
            for (int i = 0; i < unwalkableColliders.Length; i++)
            {
                Type colliderType = unwalkableColliders[i].GetType();

                GameObject obj = new GameObject(unwalkableColliders[i].transform.name + " GenCollider");
                obj.transform.parent = _colliderParent.transform;
                obj.transform.position = unwalkableColliders[i].transform.position;
                obj.transform.rotation = unwalkableColliders[i].transform.rotation;
                obj.transform.localScale = unwalkableColliders[i].transform.lossyScale;
                obj.layer = LayerMask.NameToLayer("Unwalkable");

                if (colliderType == typeof(BoxCollider))
                {
                    Vector3 newColliderSize = unwalkableColliders[i].GetComponent<BoxCollider>().size;
                    newColliderSize.x *= _colliderSizeMultiplier.x;
                    newColliderSize.y *= _colliderSizeMultiplier.y;
                    newColliderSize.z *= _colliderSizeMultiplier.z;

                    obj.AddComponent<BoxCollider>().size = newColliderSize;
                }
                else if (colliderType == typeof(SphereCollider))
                {
                    float newColliderRadius = unwalkableColliders[i].GetComponent<SphereCollider>().radius;
                    newColliderRadius *= newColliderRadius;

                    obj.AddComponent<SphereCollider>().radius = newColliderRadius;
                }
            }
        }
        private void DestroyBakingColliders()
        {
            DestroyImmediate(_colliderParent);
        }
    }
}