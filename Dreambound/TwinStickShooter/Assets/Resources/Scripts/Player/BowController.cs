using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowController : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] GameObject sprSource;
    [SerializeField] Transform sprRight;
    [SerializeField] Transform sprLeft;
    [Space]
    [SerializeField] Transform firePoint;
    [SerializeField] Transform pointer;
    [SerializeField] Material fadeMat;
    [Space]
    [SerializeField] GameObject arrow;

    [Header("Attributes")]
    [SerializeField] float aimSpeed;

    [Header("Developer")]
    Color cacheColor;

    Vector3 rLineRot = new Vector3(0, 0, -25);
    Vector3 lLineRot = new Vector3(0, 0, 25);
    Quaternion instRot;

    private void Start()
    {
        sprSource.SetActive(false);
        cacheColor = fadeMat.color;
        ResetAim();
    }

    void ResetAim()
    {
        fadeMat.color = new Color(cacheColor.r, cacheColor.g, cacheColor.b, 1);
        sprRight.localRotation = Quaternion.Euler(rLineRot);
        sprLeft.localRotation = Quaternion.Euler(lLineRot);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ResetAim();
            sprSource.SetActive(true);
        }

        if(Input.GetMouseButton(0))
        {
            float rAngle = sprRight.localRotation.eulerAngles.z;
            rAngle = (rAngle > 180) ? rAngle - 360 : rAngle;
            float lAngle = sprLeft.localRotation.eulerAngles.z;

            sprRight.localRotation = Quaternion.Lerp(sprRight.localRotation, Quaternion.Euler(Vector3.zero), aimSpeed / 50);
            sprLeft.localRotation = Quaternion.Lerp(sprLeft.localRotation, Quaternion.Euler(Vector3.zero), aimSpeed / 50);

            pointer.localRotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(rAngle, lAngle)));
        }

        if (Input.GetMouseButtonUp(0))
        {
            GameObject a = Instantiate(arrow, firePoint.position, pointer.rotation);

            sprSource.SetActive(false);
        }
    }
}