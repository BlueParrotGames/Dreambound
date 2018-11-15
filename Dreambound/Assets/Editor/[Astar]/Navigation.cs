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
            Navigation window = (Navigation)GetWindow(typeof(Navigation), false, "A* Navigation", true);
            window.Show();
        }

        private void OnGUI()
        {
            if(GUILayout.Button("Generate Grid"))
            {
                EditorGrid.GenerateGrid();
            }
        }
    }
}