using System.Collections;
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

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isReloading)
        {
            Shoot();
        }
    }

    void Shoot() 
    {
        LoadShell("Slug");

        float delay;

        // add recoil to the player when flying
        if (!player.isGrounded)
        {
            Vector3 facingPos = -mouseLook.transform.forward;
            player.rb.AddForce(facingPos * loadedShell.shootingForce, ForceMode.Impulse);

            delay = airDelay;
        }
        else
        {
            delay = groundDelay;
        }

        shotgunPelletGen(loadedShell.pellets);


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

            // hit cases
            if (Physics.Raycast(barrel.position, RandomSpread(), out hit, loadedShell.range)) 
            {
                HitCases(hit);
            }
        }
    }

    void HitCases(RaycastHit hit) 
    {
        if (hit.collider.tag == "Enemy")
        {
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

    Vector3 RandomSpread() 
    { 
        Vector3 targetPos = barrel.position + barrel.forward * loadedShell.range;
        targetPos = new Vector3
            (
                targetPos.x + Random.Range(-maxSpread, maxSpread),
                targetPos.y + Random.Range(-maxSpread, maxSpread),
                targetPos.z + Random.Range(-maxSpread, maxSpread)
            );

        Vector3 dir = targetPos - barrel.position;
        return dir.normalized;
    }

    IEnumerator gunDelay(float delay)
    {
        isReloading = true;
        yield return new WaitForSeconds(delay);
        isReloading = false;
    }

    // Debugging only
    void DebugGun(Vector3 end)
    {
        LineRenderer lr = Instantiate(debugRay).GetComponent<LineRenderer>();
        lr.SetPositions(new Vector3[2] { barrel.position, end });
    }
}
