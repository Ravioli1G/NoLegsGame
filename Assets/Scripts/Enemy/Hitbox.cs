using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public Enemy enemy;

    public void OnRaycastHit(PlayerControllerNew weapon, Vector3 dir, Rigidbody bone) 
    { 
        enemy.Destroy(dir, bone);
    }
}
