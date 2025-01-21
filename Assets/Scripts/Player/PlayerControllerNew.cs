using Unity.VisualScripting;
using UnityEngine;

public class PlayerControllerNew : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float airSpeed;
    public float airStrafeForce;
    public float groundDrag;
    public float airDrag;

    public float jumpForce;
    public float airMulti;
    public float sprintMulti;
    bool isSprinting;

    [Header("Keybind")]
    public KeyCode jumpBind = KeyCode.Space;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask ground;
    public bool isGrounded;

    public Transform orientation;
    public GameObject currentGun;

    float horizontalInput;
    float verticalInput;

    private float currentSpeedMulti;

    Vector3 moveDir;

    public Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        // ground check
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, ground);

        PlayerInput();
        MovePlayer();
        SpeedControl();

        // apply drag when grounded
        if (isGrounded)
        {
            rb.linearDamping = groundDrag;
        }
        else
        {
            rb.linearDamping = airDrag;
        }
    }

    void PlayerInput()
    {
        // get inputs
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // sprint
        if (Input.GetKey(KeyCode.LeftShift))
        { 
            isSprinting = true;
        }
        else 
        { 
            isSprinting= false;
        }

        // jump
        if (Input.GetKey(jumpBind) && isGrounded) 
        { 
            Jump();
        }
    }

    void MovePlayer()
    {
        // calc move dir
        moveDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // hide gun
        currentGun.SetActive(MovementCheck());

        SetMulti();

        if (isGrounded)
        {
            rb.AddForce(moveDir * moveSpeed * 10f * currentSpeedMulti, ForceMode.Force);
        }
        else
        {
            AirMovement(moveDir);
        }
    }


    void SetMulti() 
    {
        if (isSprinting)
        {
            currentSpeedMulti = sprintMulti;
        }
        else
        {
            currentSpeedMulti = 1f;
        }
    }

    void SpeedControl() 
    { 
        // current vel of player
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        // reset vel to speed limit when players vel exceedes movespeed
        if (flatVel.magnitude > moveSpeed && isGrounded) 
        { 
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
        }
    }

    void AirMovement(Vector3 vector3) 
    {
        // sourced from u/doesnt_hate_people
        // project the velocity onto the movevector
        Vector3 projVel = Vector3.Project(GetComponent<Rigidbody>().linearVelocity, vector3);

        // check if the movevector is moving towards or away from the projected velocity
        bool isAway = Vector3.Dot(vector3, projVel) <= 0f;

        // only apply force if moving away from velocity or velocity is below MaxAirSpeed
        if (projVel.magnitude < airSpeed || isAway)
        {
            // calculate the ideal movement force
            Vector3 vc = vector3.normalized * airStrafeForce;

            // cap it if it would accelerate beyond MaxAirSpeed directly.
            if (!isAway)
            {
                vc = Vector3.ClampMagnitude(vc, airSpeed - projVel.magnitude);
            }
            else
            {
                vc = Vector3.ClampMagnitude(vc, airSpeed + projVel.magnitude);
            }

            // Apply the force
            GetComponent<Rigidbody>().AddForce(vc, ForceMode.VelocityChange);
        }

    }

    void Jump() 
    {
        // reset y vel
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
    
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    bool MovementCheck() 
    { 
        // allows player to use gun while strafing in air
        if (horizontalInput == 0f && verticalInput == 0f || !isGrounded) 
        { 
            return true;
        }
        else 
        { 
            return false;
        }
    }
}
