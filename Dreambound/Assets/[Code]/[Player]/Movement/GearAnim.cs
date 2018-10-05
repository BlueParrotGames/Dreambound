using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearAnim : MonoBehaviour
{
    [SerializeField] Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if(!GameManager.useController)
        {
            anim.SetFloat("VelocityX", Input.GetAxis("Horizontal"));
            anim.SetFloat("VelocityZ", Input.GetAxis("Vertical"));
        }

        if(GameManager.useController)
        {
            anim.SetFloat("VelocityX", Input.GetAxis("LHorizontal"));
            anim.SetFloat("VelocityZ", Input.GetAxis("LVertical"));
        }
    }
}
