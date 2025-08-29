using System.Collections;
using TMPro;
using UnityEngine;

public class Shotgun : MonoBehaviour
{
    public PlayerControllerNew player;
    public Bullets bullets;
    public ParticleSystem particles;
    public MouseLook mouseLook;
    public Transform barrel;

    [Header("Debugging")]
    public GameObject debugRay;
    public bool debugRayOn = false;

    [Header("Delay Times")]
    public float airDelay;
    public float groundDelay;

    [Header("Gun Values")]
    public float maxSpread = 5f;

    private Shell loadedShell;
    private bool isReloading = false;
    private float delay;
    private bool isActive = true;
    public TextMeshProUGUI canShoot;
    public GameObject gunMesh;

    private void OnEnable()
    {
        if (isReloading)
        {
            isReloading = false;
        }
    }
    private void Update()
    {
        canShoot.text = "weap RELOADING: " + isReloading;
        if (Input.GetMouseButtonDown(0) && !isReloading)
        {
            Shoot();
        }
    }

    public void setMeshActive(bool enableWeap)
    {

        if (enableWeap && !isActive) // prevent checks when already active
        {
            gunMesh.GetComponent<MeshRenderer>().enabled = true;
            isActive = true;
        }
        else if (!enableWeap)
        {
            gunMesh.GetComponent<MeshRenderer>().enabled = false;
            isActive = false;
        }
    }

    void Shoot() 
    {
        LoadShell("Buck");

        // add recoil to the player when flying
        if (!player.isGrounded)
        {
            // opposite of current looking direction
            Vector3 facingPos = -mouseLook.transform.forward;
            player.rb.AddForce(facingPos * loadedShell.shootingForce, ForceMode.Impulse);

            // air racking animation
            delay = airDelay;
        }
        else
        {
            // racking animation
            delay = groundDelay;
        }

        shotgunPelletGen(loadedShell.pellets);
        gunDelay(delay);
    }

    void LoadShell(string bulletType) 
    {
        loadedShell = bullets.getShell(bulletType);
    }

    void shotgunPelletGen(int pellets)
    {
        for (int i = 0; i < pellets; i++)
        {
            RaycastHit hit;

            // get result of the current raycast
            if (Physics.Raycast(barrel.position, RandomSpread(), out hit, loadedShell.range)) 
            {
                HitCases(hit);
            }
        }
    }

    void HitCases(RaycastHit hit) 
    {
        // go through each possible case
        if (hit.collider.tag == "Enemy")
        {
            // replace with enemy death when implemented
            //Destroy(hit.collider.gameObject);
        }
        if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Instantiate(particles, hit.point, Quaternion.identity);
        }
        // remove on final
        if (debugRayOn) 
        {
            DebugGun(hit.point);
        }
    }

    Vector3 RandomSpread() 
    { 
        // get the endpoint of the current pellet
        Vector3 targetPos = barrel.position + barrel.forward * loadedShell.range;
        
        // apply spread to endpoint
        targetPos = new Vector3
            (
                targetPos.x + Random.Range(-maxSpread, maxSpread),
                targetPos.y + Random.Range(-maxSpread, maxSpread),
                targetPos.z + Random.Range(-maxSpread, maxSpread)
            );

        // account for barrel position
        Vector3 dir = targetPos - barrel.position;
        return dir.normalized;
    }

    void gunDelay(float delay)
    {
        isReloading = true;
        StartCoroutine(reload(delay));
    }

    IEnumerator reload(float delay) 
    {
        // play reload anim
        yield return new WaitForSeconds(delay);
        isReloading = false;
    }

    // Debugging only, remove on final
    void DebugGun(Vector3 end)
    {
        LineRenderer lr = Instantiate(debugRay).GetComponent<LineRenderer>();
        lr.SetPositions(new Vector3[2] { barrel.position, end });
    }
}
