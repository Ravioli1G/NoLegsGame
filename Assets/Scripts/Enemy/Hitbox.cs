using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public Enemy enemy;

    public void OnRaycastHit(PlayerController weapon, Vector3 dir, Rigidbody bone) 
    { 
        enemy.Destroy(dir, bone);
    }
}
