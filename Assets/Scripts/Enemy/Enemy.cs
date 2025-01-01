using UnityEngine;

public class Enemy : MonoBehaviour
{
    Ragdoll rd;
    float dieForce = 20f;
    private void Start()
    {
        rd = GetComponent<Ragdoll>();

        var rigidBodies = GetComponentsInChildren<Rigidbody>();

        foreach (var rigidBody in rigidBodies) 
        {
            Hitbox hitbox = rigidBody.gameObject.AddComponent<Hitbox>();
            hitbox.enemy = this;
        }
    }
    public void Destroy(Vector3 dir, Rigidbody bone) 
    {
        Debug.Log("Enemy destroyed");
        dir.y = 1f;
        //Destroy(gameObject);
        rd.ActivateRagdoll();
        rd.ApplyForce(dir * dieForce, bone);
    }

}
