using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class FloraSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    [Tooltip("I.E. Big rocks, Trees")]
    [SerializeField] GameObject[] big_flora;
    [Tooltip("I.E. Sall rocks, Grass, Mushrooms ETC")]
    [SerializeField] GameObject[] small_flora;

    [Range(0.01f, 1f)]
    [Tooltip("The higher the spread, the more objects. The lower, the reverse.")]
    [SerializeField] float spread = 0.15f;

    [Header("Developer")]
    [ReadOnly] [SerializeField] int treecount;

    private void Update()
    {
        treecount = (int)Amount();

    }

    private float Amount()
    {
        float amount = 0;

        amount = transform.localScale.x * transform.localScale.z * spread;

        return amount;
    }

    private Vector2 CalculateX()
    {
        Vector2 bounds = new Vector2(0, 0);

        float i = transform.localScale.x / 2;
        bounds.x = transform.position.x - i;
        bounds.y = transform.position.x + i;

        return bounds;
    }

    private Vector2 CalculateZ()
    {
        Vector2 bounds = new Vector2(0, 0);

        float i = transform.localScale.z / 2;
        bounds.x = transform.position.z - i;
        bounds.y = transform.position.z + i;

        return bounds;
    }

    public void SpawnBigAssets()
    {
        List<Vector3> positions = new List<Vector3>((int)Amount());

        for(int i = 0; i < Amount(); i++)
        {
            float x = Random.Range(CalculateX().x, CalculateX().y);
            float z = Random.Range(CalculateZ().x, CalculateZ().y);

            positions.Add(new Vector3(x, 0, z));
        }

        Debug.Log("position count " + positions.Count);


        for(int i = 0; i < positions.Count; i++)
        {
            Instantiate(big_flora[Random.Range(0, big_flora.Length)], positions[i], Quaternion.identity);
        }

    }

    public void SpawnSmallAssets()
    {
        List<Vector3> positions = new List<Vector3>((int)Amount());

        for (int i = 0; i < Amount(); i++)
        {
            float x = Random.Range(CalculateX().x, CalculateX().y);
            float z = Random.Range(CalculateZ().x, CalculateZ().y);

            positions.Add(new Vector3(x, 0, z));
        }

        for (int i = 0; i < positions.Count; i++)
        {
            Instantiate(small_flora[Random.Range(0, small_flora.Length)], positions[i], Quaternion.identity);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(transform.localPosition + new Vector3(0, transform.localScale.y / 2, 0), transform.localScale);
    }

    public void FullDelete()
    {
        GameObject[] flora;

        flora = GameObject.FindGameObjectsWithTag("Flora");

        foreach(GameObject g in flora)
        {
            DestroyImmediate(g);
        }
    }

    public void SafeDelete()
    {
        GameObject[] flora;

        flora = GameObject.FindGameObjectsWithTag("Flora");

        foreach (GameObject g in flora)
        {   
            if(g.transform.parent == null)
            {
                DestroyImmediate(g);
            }
        }
    }
}
