using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    public bool isFiring;

    [SerializeField] BulletController bullet;
    
    public float timeBetweenShots;
    private float shotCounter;

    public Transform firePoint;

    // Update is called once per frame
    void Update()
    {
        if(isFiring)
        {
            shotCounter -= Time.deltaTime;

            if(shotCounter <= 0)
            {
                shotCounter = timeBetweenShots;
                Instantiate(bullet, firePoint.position, firePoint.rotation);
            }
        }
        else
        {
            shotCounter = 0;
        }
    }
}
