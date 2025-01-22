using UnityEngine;
using System.Collections;

public class Pistol : MonoBehaviour
{
    public PlayerControllerNew player;
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
    public float range = 80f;
    public float force = 0f;

    private bool isReloading = false;
    private float delay;

    private void OnEnable()
    {
        // incase reloading is not complete when player moves
        if (isReloading)
        {
            StartCoroutine(Reload(delay));
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isReloading)
        {
            Shoot();
        }
    }

    void Shoot()
    {

        // add recoil to the player when flying
        if (!player.isGrounded)
        {
            Vector3 facingPos = -mouseLook.transform.forward;
            player.rb.AddForce(facingPos * force, ForceMode.Impulse);

            delay = airDelay;
        }
        else
        {
            delay = groundDelay;
        }

        BulletGen();
        GunDelay(delay);
    }

    void BulletGen()
    {
        RaycastHit hit;

        // hit cases
        if (Physics.Raycast(barrel.position, barrel.forward, out hit, range))
        {
            HitCases(hit);
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

    void GunDelay(float delay)
    {
        isReloading = true;
        StartCoroutine(Reload(delay));
    }

    IEnumerator Reload(float delay)
    {
        Debug.Log("Delay has started" + delay + " isreloading: " + isReloading);
        // play reload anim
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
