using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Dreambound.Astar.Editor
{
    public class Navigation : EditorWindow
    {
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
            if(GUILayout.Button("Generate Grid"))
            {
                EditorGrid.GenerateGrid();
            }
        }
    }
}