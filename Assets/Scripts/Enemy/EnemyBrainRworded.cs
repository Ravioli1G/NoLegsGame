using UnityEngine;

public class EnemyBrainRworded : MonoBehaviour
{
    public Transform target;

    private EnemyReferences enemyReferences;

    private float shootingDistance;

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
        
    }
}
