using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Dreambound.Astar.Editor
{
    [CustomEditor(typeof(Grid))]
    class GridEditor : UnityEditor.Editor
    {
        private static SerializedObject _serializedObject;
        private static Grid _target;

        private void OnSceneGUI()
        {
            _target = (Grid)target;
        }
    }
}
