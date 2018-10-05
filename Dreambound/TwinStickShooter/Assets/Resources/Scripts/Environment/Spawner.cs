using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Object fields")]
    [SerializeField] GameObject enemy;
    [SerializeField] Transform spawnPoint;

    [Header("Attributes")]
    [Tooltip("Spawns ever x.x seconds I.E: 0.3f = every 0.3 seconds a spawn occurs")]
    [SerializeField] float spawnRate = 0.3f;

    void Start()
    {
        InvokeRepeating("Spawn", 0.3f, spawnRate);
    }
    
    void Spawn()
    {
        GameObject e = Instantiate(enemy, spawnPoint.position, Quaternion.identity);
    }

}
