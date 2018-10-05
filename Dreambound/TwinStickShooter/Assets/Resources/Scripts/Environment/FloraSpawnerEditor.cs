using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FloraSpawner))]
public class FloraSpawnerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GUILayout.Space(10);
        FloraSpawner f = (FloraSpawner)target;

        if(GUILayout.Button("Spawn Trees"))
        {
            f.SpawnBigAssets();
        }

        if(GUILayout.Button("Spawn Small"))
        {
            f.SpawnSmallAssets();
        }

        GUILayout.Space(50);
        GUI.backgroundColor = Color.red;
        if(GUILayout.Button("Clear ALL Flora"))
        {
            f.DeleteAssets();
        }
    }
}
