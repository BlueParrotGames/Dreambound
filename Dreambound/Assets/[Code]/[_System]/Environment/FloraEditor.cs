using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Flora))]
[CanEditMultipleObjects]
public class FloraEditor : Editor
{
    public override void OnInspectorGUI()
    {

        GUILayout.Space(10);
        Flora f = (Flora)target;
        
        if (GUILayout.Button("Set Random Rotation"))
        {
            f.RandomRotation();
        }
    }
}
