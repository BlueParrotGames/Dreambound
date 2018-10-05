using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamController : MonoBehaviour
{
    [Header("Transforms")]
    [SerializeField] Transform cam;
    [SerializeField] Transform player;
    [Header("Attributes")]
    [SerializeField] Vector3 offset;

    private void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    private void LateUpdate()
    {
        cam.position = player.position + offset;
    }
}
