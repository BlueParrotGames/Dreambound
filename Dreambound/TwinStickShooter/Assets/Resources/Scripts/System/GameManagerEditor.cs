using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameManager))]
public class GameManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GameManager gm = (GameManager)target;

        if (GUILayout.Button("Clean Scene"))
        {
            gm.CleanScene();
        }

        if(GUILayout.Button("Set Camera Settings"))
        {
            gm.SetCamera();
        }

    }
}