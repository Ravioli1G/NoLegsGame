using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CharacterController controller;

    public float speed = 12f;
    public float grav = -9.81f;
    public float groundDist = 0.4f;
    public float jumpHeight = 3f;

    public LayerMask groundMask;

    public Transform groundCheck;

    Vector3 vel;
    bool isGrounded;

    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDist, groundMask);

        if (isGrounded && vel.y < 0)
        {
            vel.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            vel.y = Mathf.Sqrt(jumpHeight * -2f * grav);
        }

        vel.y += grav * Time.deltaTime;

        controller.Move(vel * Time.deltaTime);
    }

}
