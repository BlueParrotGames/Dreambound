using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Dreambound.Astar.Editor
{
    public class Navigation : EditorWindow
    {
        private Vector3 _colliderSizeMultiplier;
        [Tooltip("tst")]

        private float _radiusSizeMultiplier;
        private List<GameObject> _colliderObjects;
        private GameObject _colliderParent;

        [MenuItem("Window/Dreambound/AI/Navigation")]
        private static void InitializeWindow()
        {
            //Store the currently selected object and select the grid gameObject to avoid errors
            GameObject currentSelection = Selection.activeGameObject;
            Selection.activeGameObject = FindObjectOfType<Grid>().gameObject;

            //Show the window
            Navigation window = (Navigation)GetWindow(typeof(Navigation), false, "A* Navigation", true);
            window.Show();

            //Reselect the stored gameObject
            Selection.activeGameObject = currentSelection;
        }

        private void OnGUI()
        {
            EditorGUILayout.Space();
            _colliderSizeMultiplier = EditorGUILayout.Vector3Field("Collider Size Multiplier", _colliderSizeMultiplier);

            _radiusSizeMultiplier = EditorGUILayout.FloatField("Collider Radius Multiplier", _radiusSizeMultiplier);

            EditorGUILayout.Space();
            if (GUILayout.Button("Generate Baking Colliders"))
            {
                GenerateBakingColliders();
            }
        }

        private void GenerateBakingColliders()
        {
            if (_colliderObjects == null)
                _colliderObjects = new List<GameObject>();

            if (_colliderParent == null)
            {
                _colliderParent = new GameObject("Colliders");
                _colliderParent.transform.parent = FindObjectOfType<Grid>().transform;
            }

            for(int i = 0; i < _colliderObjects.Count; i++)
            {
                _colliderObjects.Remove(_colliderObjects[i]);
            }

            Collider[] unwalkableColliders = FindObjectsOfType<Collider>().Where(x => x.gameObject.layer == LayerMask.NameToLayer("Unwalkable")).ToArray();
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
    }
}