using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    Rigidbody[] rbs;
    Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rbs = GetComponentsInChildren<Rigidbody>();
        animator = GetComponent<Animator>();
        DeactivateRagdoll();
    }

    public void DeactivateRagdoll()
    {
        foreach (var rb in rbs)
        {
            rb.isKinematic = true;
        }
        animator.enabled = true;
    }

    public void ActivateRagdoll()
    {
        foreach (var rb in rbs)
        {
            rb.isKinematic = false;
        }
        animator.enabled = false;
    }

    public void ApplyForce(Vector3 force, Rigidbody bone)
    {
        bone.AddForce(force, ForceMode.Impulse);
    }
}
