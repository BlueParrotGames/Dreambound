using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] float speed = 6f;
    [SerializeField] float jumpHeight = 8f;
    [SerializeField] float gravity = 20.0f;
    [SerializeField] float deadzone = 0.25f;

    [SerializeField] Animator anim;

    Camera mainCam;
    CharacterController controller;
    Vector3 moveDir = Vector3.zero;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        mainCam = Camera.main;
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        // Mouse & Keyboard
        if (!GameManager.useController)
        {
            if (controller.isGrounded)
            {
                //moveDir = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
                anim.SetFloat("VelocityX", Input.GetAxis("Horizontal"));
                anim.SetFloat("VelocityZ", Input.GetAxis("Vertical"));

                //moveDir *= speed;
                //moveDir.y = -0.1f;
                if (Input.GetButtonDown("Jump"))
                {
                    moveDir.y = jumpHeight;
                }
            }
           
            Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
            float rayLength;

            if (groundPlane.Raycast(camRay, out rayLength))
            {
                Vector3 lookPoint = camRay.GetPoint(rayLength);
                Debug.DrawLine(gameObject.transform.position, lookPoint, Color.yellow);

                //transform.TransformDirection(moveDir);
                transform.LookAt(new Vector3(lookPoint.x, transform.position.y, lookPoint.z));
            }
        }

        // Controller
        if(GameManager.useController)
        {
            anim.SetFloat("VelocityX", Input.GetAxis("LHorizontal"));
            anim.SetFloat("VelocityZ", Input.GetAxis("LVertical"));

            if (controller.isGrounded)
            {
                if (Input.GetButtonDown("AJump"))
                {
                     // jump
                }
            }

            Vector2 input = new Vector2(Input.GetAxisRaw("RHorizontal"), Input.GetAxisRaw("RVertical"));
            if (input.magnitude < deadzone)
            {
                input = Vector2.zero;
            }
            else
            {
                input = input.normalized * ((input.magnitude - deadzone) / (1 - deadzone));
            }


            //Vector3 playerRot = Vector3.right * input.x + Vector3.forward * input.y;

            if (input.sqrMagnitude > 0.0f)
            {
                transform.rotation = Quaternion.LookRotation(new Vector3(input.x, 0, input.y), Vector3.up);
            }
        }

        // Global
        moveDir.y -= (gravity) * Time.deltaTime;

        float forward = Input.GetAxis("LVertical");
        float right = Input.GetAxis("LHorizontal");

        Vector3 dir = new Vector3(right, 0, forward);

        transform.Translate(dir / 10, Space.Self);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 10);
    }
}