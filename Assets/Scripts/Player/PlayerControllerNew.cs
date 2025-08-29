using System.Collections;
using TMPro;
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
    public float jumpDelay;

    public float jumpForce;
    public float airMulti;
    public float sprintMulti;
    bool isSprinting;
    bool canJump;

    [Header("Keybind")]
    public KeyCode jumpBind = KeyCode.Space;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask ground;
    public bool isGrounded;

    [Header("Debug")]
    public TextMeshProUGUI currVel;

    public Transform orientation;
    public PlayerWeapInventory weap;

    private float currentSpeedMulti;

    Vector3 moveDir;

    public Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        canJump = true;
    }

    private void FixedUpdate()
    {
        // ground check
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, ground);

        PlayerInput();

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
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

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
        if (Input.GetKey(jumpBind) && isGrounded && canJump) 
        { 
            Jump();
        }

        MovePlayer(horizontalInput, verticalInput);
    }

    void MovePlayer(float hor, float ver)
    {
        // calc move dir
        moveDir = orientation.forward * ver + orientation.right * hor;

        currVel.text = "curr dir: " + moveDir;
        // hide gun
        weap.ShowEquipped(MovementCheck(hor, ver));
        SetMovementMultiplier();

        if (isGrounded)
        {
            rb.AddForce(moveDir * moveSpeed * 10f * currentSpeedMulti, ForceMode.Force);
        }
        //SpeedControl();
    }

    void SetMovementMultiplier() 
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

        // prevent player from reaching high than intended speeds
        if (flatVel.magnitude > moveSpeed && isGrounded) 
        { 
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
        }
    }

    void Jump() 
    {
        // reset y vel
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
    
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        StartCoroutine(JumpDelay());
    }

    bool MovementCheck(float hor, float ver) 
    {
        if ((hor != 0 || ver != 0) && isGrounded)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    IEnumerator JumpDelay()
    {
        canJump = false;
        yield return new WaitForSeconds(jumpDelay);
        canJump = true;
    }
}
