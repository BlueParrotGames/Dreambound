using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed;
    public float bulletLifetime;

    void Update()
    {
        destroyAfterLifetime();
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

     void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            Debug.Log("Hit");
        }
        Destroy(gameObject);
    }

    void destroyAfterLifetime()
    {
        bulletLifetime -= Time.deltaTime;

        if(bulletLifetime <= 0)
        {
            //Destroy(gameObject);
        }
    }
}
