using UnityEngine;

public class RigidBodyPlayerController : MonoBehaviour
{
    public Rigidbody rb;
    public MouseLook ml;

    public float walkspeed = 20f;
    public float jumpforce = 1f;
    public float shootforce = 15f;
    public float groundDist = 0.4f;

    private bool isGrounded;
    private Vector3 playerMovementInput;
    private Vector3 facingPos;

    public LayerMask groundMask;

    public Transform groundCheck;
    // enable walking by uncommenting code - currently bugged with jump shooting as setting the velocity
    // of the rigid body gives it a teleporting effect - line 33
    void Update()
    {
        //playerMovementInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        MovePlayer();
    }

    void MovePlayer() 
    { 
        //Vector3 MoveVector = transform.TransformDirection(playerMovementInput) * walkspeed;
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDist, groundMask);

        //rb.linearVelocity = new Vector3(MoveVector.x, rb.linearVelocity.y, MoveVector.z);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * jumpforce, ForceMode.Impulse);
        }
        if (Input.GetMouseButtonDown(0) && !isGrounded)
        {
            facingPos = -ml.transform.forward;

            Debug.Log("shoot" + (facingPos*shootforce) +" applied");
            rb.AddForce(facingPos * shootforce, ForceMode.Impulse);
        }
    }
}
