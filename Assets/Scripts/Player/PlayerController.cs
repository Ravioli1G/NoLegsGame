using Mono.Cecil.Cil;
using System.Collections;
using UnityEngine;
using UnityEngine.Animations;

public class PlayerController : MonoBehaviour
{
    // TODO clean up globals
    public KeyCode Slug;
    public KeyCode Buck;
    public KeyCode Bird;
    public GameObject gun;
    [Space]
    public Rigidbody rb;
    public MouseLook ml;
    public Transform pointer;
    public ParticleSystem particles;
    public GameObject debugRay;
    public Bullets bullets;
    public Animator birdUI;
    public Animator buckUI;
    public Animator slugUI;
    [Space]
    public float walkspeed = 1f;
    public float jumpforce = 1f;
    public float groundDist = 0.4f;
    public float maxSpread = 5f;
    public float sprintMultiplier = 2f;
    [Space]
    public LayerMask groundMask;
    public Transform groundCheck;
    public bool debugRayOn = false;

    private bool isGrounded;
    private bool isShooting = false;
    public float shootforce;
    private float shootDelayGround = 0.5f;
    private float shootDelayAir = 0.5f;
    private float range;
    private float playerSpeed;
    private int pellets;
    private Vector3 facingPos;

    private void Start()
    {
        LoadShell("Slug");
        slugUI.SetBool("SlugSelected", true);
        playerSpeed = walkspeed;
    }

    void Update()
    {
        // get player input axises
        Vector3 playerMovementInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

        MovePlayer(playerMovementInput);

        //fixxxxxxxxx!!
        if (Input.GetKeyDown(Slug))
        {
            LoadShell("Slug");
            slugUI.SetBool("SlugSelected", true);
            birdUI.SetBool("BirdSelected", false);
            buckUI.SetBool("BuckSelected", false );
        }
        if (Input.GetKeyDown(Buck))
        {
            LoadShell("Buck");
            slugUI.SetBool("SlugSelected", false);
            birdUI.SetBool("BirdSelected", false);
            buckUI.SetBool("BuckSelected", true);
        }
        if (Input.GetKeyDown(Bird))
        {
            LoadShell("Bird");
            slugUI.SetBool("SlugSelected", false);
            birdUI.SetBool("BirdSelected", true);
            buckUI.SetBool("BuckSelected", false);
        }
    }

    void MovePlayer(Vector3 input) 
    {
        // ground check
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDist, groundMask);

        // hide gun when player is walking
        gun.SetActive(InputCheck() || !isGrounded);

        // make vector based on player input
        Vector3 MoveVector = transform.TransformDirection(input) * playerSpeed;

        // new method
        rb.AddForce(MoveVector.normalized, ForceMode.Force);
      
        // apply movement to existing velocity
        //rb.linearVelocity = new Vector3(MoveVector.x, rb.linearVelocity.y, MoveVector.z);

        // should movement velocity be applied
        if (isGrounded)
        {
            // jump
            if (Input.GetKeyDown(KeyCode.Space))
            {
                rb.AddForce(Vector3.up * jumpforce, ForceMode.Impulse);
                Debug.Log("Jumppy");
            }

            // sprint
            if (Input.GetKeyDown(KeyCode.LeftShift)) 
            {
                Sprint();
            }
        }
        // allow reset when in air
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            Walk();
        }

        // check if player is able to shoot
        if (Input.GetMouseButtonDown(0) && !isShooting)
        {
            Shoot();
        }
    }

    bool InputCheck() 
    {

        if (Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void LoadShell(string name) 
    {
        Shell currentShell = bullets.getShell(name);

        shootforce = currentShell.shootingForce;
        maxSpread = currentShell.spread;
        pellets = currentShell.pellets;
        range = currentShell.range;
        Debug.Log("Shell loaded: " + currentShell.shellName);
    }
    void Shoot() 
    {
        float delay;
        // dont apply force when player is grounded
        if (!isGrounded)
        {
            facingPos = -ml.transform.forward;

            rb.AddForce(facingPos * shootforce, ForceMode.Impulse);
            delay = shootDelayAir;
        }
        else
        {
            delay = shootDelayGround;
        }

        // shotgun pellet gen
        for (int i = 0; i < pellets; i++)
        {
            RaycastHit hit; // raycast for each pellet
            
            // check what is hit
            if (Physics.Raycast(pointer.position, RandomSpread(), out hit, range))
            {
                if (hit.collider.tag == "Enemy") 
                {
                    // destroy enemy on hit
                    Destroy(hit.collider.gameObject);
                        
                }
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground")) 
                {
                    Instantiate(particles, hit.point, Quaternion.identity);
                }
                if (debugRayOn) 
                {
                    DebugGun(hit.point);
                }
            }
        }

        StartCoroutine(gunDelay(delay)); // delay for cocking gun
    }

    IEnumerator gunDelay(float delay) 
    { 
        isShooting = true;
        yield return new WaitForSeconds(delay);
        isShooting = false;
    }

    void Sprint() 
    {
        playerSpeed = walkspeed * sprintMultiplier;
        // hide gun
    }

    void Walk() 
    {
        playerSpeed = walkspeed;
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
