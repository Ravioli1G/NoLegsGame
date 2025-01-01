using Mono.Cecil.Cil;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public KeyCode Slug;
    public KeyCode Buck;
    public KeyCode Bird;
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
    public float walkspeed = 20f;
    public float jumpforce = 1f;
    public float groundDist = 0.4f;
    public float maxSpread = 5f;
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
    private int pellets;
    private Vector3 playerMovementInput;
    private Vector3 facingPos;

    private void Start()
    {
        LoadShell("Slug");
        slugUI.SetBool("SlugSelected", true);
    }
    // enable walking by uncommenting code - currently bugged with jump shooting as setting the velocity
    // of the rigid body gives it a teleporting effect - line 33
    void Update()
    {
        //playerMovementInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        MovePlayer();
        //fucking mess
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

    void MovePlayer() 
    { 
        //Vector3 MoveVector = transform.TransformDirection(playerMovementInput) * walkspeed;
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDist, groundMask);

        //rb.linearVelocity = new Vector3(MoveVector.x, rb.linearVelocity.y, MoveVector.z);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpforce, ForceMode.Impulse);
        }
        if (Input.GetMouseButtonDown(0) && !isShooting)
        {
            Shoot();
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
        // force checks
        if (!isGrounded)
        {
            facingPos = -ml.transform.forward;

            rb.AddForce(facingPos * shootforce, ForceMode.Impulse);
            delay = shootDelayAir;
            Debug.Log("Shoot in air");
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
                    hit.collider.GetComponent<Hitbox>().OnRaycastHit(this, pointer.transform.forward, hit.rigidbody);
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
