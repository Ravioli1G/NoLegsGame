using Mono.Cecil.Cil;
using System.Collections;
using UnityEngine;

public class RigidBodyPlayerControllerWithAdjustedShootForce : MonoBehaviour
{
    public Rigidbody rb;
    public MouseLook ml;
    public Transform pointer;
    public ParticleSystem particles;
    public GameObject debugRay;
    [Space]
    public float walkspeed = 20f;
    public float jumpforce = 1f;
    public float groundDist = 0.4f;
    public float maxSpread = 5f;
    [Space]
    public float shootforce = 45;
    public float shootforceAir = 15f;
    public float shootDelayGround = 0.75f;
    public float shootDelayAir = 1.25f;
    public float range = 50f;
    public int pellets = 6;
    [Space]
    public LayerMask groundMask;
    public Transform groundCheck;

    private bool isGrounded;
    private bool isShooting = false;
    private bool lessShootForce;

    private float OGShootForce;
    private Vector3 playerMovementInput;
    private Vector3 facingPos;


    private void Start()
    {
        OGShootForce = shootforce;
    }
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

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpforce, ForceMode.Impulse);
            lessShootForce = false;
        }
        if (Input.GetMouseButtonDown(0) && !isShooting)
        {
            Shoot();
        }
    }

    void Shoot() 
    {
        float delay;
        // force checks
        if (!isGrounded)
        {
            facingPos = -ml.transform.forward;

            float shootforcetemp = lessShootForce ? shootforceAir : shootforce;
            rb.AddForce(facingPos * shootforcetemp, ForceMode.Impulse);
            delay = shootDelayAir;
            Debug.Log("Shoot in air");
            lessShootForce = true;
        }
        else
        {
            delay = shootDelayGround;
            Debug.Log("Shoot on ground");
        }

        // shotgun pellet gen
        for (int i = 0; i < pellets; i++)
        {
            RaycastHit hit;

            if (Physics.Raycast(pointer.position, RandomSpread(), out hit, range))
            {
                if (hit.collider.tag == "Enemy") 
                {
                    hit.collider.GetComponent<Enemy>().Destroy();
                }
                Instantiate(particles, hit.point, Quaternion.identity);
                //DebugGun(hit.point);
            }
        }

        StartCoroutine(gunDelay(delay));
    }

    IEnumerator gunDelay(float delay) 
    { 
        isShooting = true;
        yield return new WaitForSeconds(delay);
        isShooting = false;
    }

    Vector3 RandomSpread() 
    { 
        Vector3 targetPos = pointer.position + pointer.forward * range;
        targetPos = new Vector3
        (
            targetPos.x + Random.Range(-maxSpread, maxSpread),
            targetPos.y + Random.Range(-maxSpread, maxSpread),
            targetPos.z + Random.Range(-maxSpread, maxSpread)
        );

        Vector3 dir = targetPos - pointer.position;
        return dir.normalized;
    }

    void DebugGun(Vector3 end) 
    { 
        LineRenderer lr = Instantiate(debugRay).GetComponent<LineRenderer>();
        lr.SetPositions(new Vector3[2] {pointer.position, end});
    }
}
