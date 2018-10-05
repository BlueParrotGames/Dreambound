using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Flora : MonoBehaviour
{

    private void Start()
    {
        RandomRotation();
    }

    public void RandomRotation()
    {
        float x = Random.Range(-10, 10);
        float y = Random.Range(0, 360);
        float z = Random.Range(-10, 10);

        transform.rotation = Quaternion.Euler(x, y, z);
    }
}
