using UnityEngine;

public class EnemyBrainRworded : MonoBehaviour
{
    public Transform target;

    private EnemyReferences enemyReferences;

    private float shootingDistance;
    private float pathUpdateDelay;

    private void Awake()
    {
        enemyReferences = GetComponent<EnemyReferences>();
    }

    void Start()
    {
        shootingDistance = enemyReferences.agent.stoppingDistance;
    }

    void Update()
    {
        if (target != null)
        {
            bool inRange = Vector3.Distance(transform.position, target.position) <= shootingDistance;

            if (inRange)
            {
                LookAtTarget();
            }
            else
            {
                UpdatePath();
            }
        }

    }

    void LookAtTarget() 
    { 
        Vector3 lookPos = target.position - transform.position;
        lookPos.y = 0f;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.2f);
    }

    void UpdatePath() 
    {
        if (Time.time >= pathUpdateDelay)
        {
            pathUpdateDelay = Time.time + enemyReferences.pathUpdateDelay;
            enemyReferences.agent.SetDestination(target.position);
        }
    }
        
}
