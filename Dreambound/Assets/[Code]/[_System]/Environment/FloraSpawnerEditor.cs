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

        if(GUILayout.Button("Spawn Big"))
        {
            f.SpawnBigAssets();
        }

        if(GUILayout.Button("Spawn Small"))
        {
            f.SpawnSmallAssets();
        }

        if(GUILayout.Button("Generate Both"))
        {
            f.SpawnBigAssets();
            f.SpawnSmallAssets();
        }

        GUILayout.Space(20);
        GUI.backgroundColor = Color.red;
        if (GUILayout.Button("Safe Delete Flora"))
        {
            f.SafeDelete();
        }
        GUI.backgroundColor = Color.red;
        if (GUILayout.Button("Delete ALL Flora"))
        {
            f.FullDelete();
        }
    }

    private void OnSceneGUI()
    {
        FloraSpawner f = (FloraSpawner)target;
        Event e = Event.current;

        if (EventType.KeyDown == e.type && KeyCode.X == e.keyCode)
        {
            f.SpawnBigAssets();
        }

        if (EventType.KeyDown == e.type && KeyCode.C == e.keyCode)
        {
            f.SpawnSmallAssets();
        }

        if (EventType.KeyDown == e.type && KeyCode.V == e.keyCode)
        {
            f.SpawnBigAssets();
        }
    }
}
