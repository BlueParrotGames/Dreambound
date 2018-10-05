using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.Rendering.PostProcessing;

[ExecuteInEditMode]
public class GameManager : MonoBehaviour
{
    private Camera cam;

    public static bool useController;
    [SerializeField] Toggle toggle;

    [Header("Parents")]
    [SerializeField] Transform floraParent;


    private void Start()
    {
        if(cam == null)
        {
            Debug.LogError("Camera has not been set up!");
        }

        useController = toggle.isOn;
    }

    public void SetController(bool b)
    {
        useController = b;
    }

    public void CleanScene()
    {
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Flora"))
        {
            g.transform.parent = floraParent;
        }
    }

    public void SetCamera()
    {
        cam = Camera.main;

        cam.fieldOfView = 65f;
        cam.backgroundColor = Color.black;
        cam.clearFlags = CameraClearFlags.SolidColor;
        //cam.gameObject.AddComponent<PostProcessVolume>();
    }

}
