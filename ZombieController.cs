using UnityEngine;
using UnityEngine.AI;

public class ZombieController : MonoBehaviour
{
    [SerializeField] private ZombieType zombieData; 
    
    private NavMeshAgent agent;
    private Transform player;

    public void Initialize(ZombieType data) 
    {
        zombieData = data;
        SetupStats();
    }

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if(p != null) player = p.transform;
    }

    void Start()
    {
        if(zombieData != null) SetupStats();
    }

    void SetupStats()
    {
        // FIX: Updated to 'moveSpeed'
        agent.speed = zombieData.moveSpeed;
        
        // FIX: Updated to 'maxHealth'
        if(TryGetComponent(out ZombieHealth hpScript))
        {
            hpScript.maxHealth = zombieData.maxHealth;
        }
        
        // FIX: Updated to 'typeName'
        if(zombieData.typeName == "Tank") transform.localScale = Vector3.one * 1.5f;
        if(zombieData.typeName == "Runner") transform.localScale = Vector3.one * 0.8f;
    }

    void Update()
    {
        if (player != null)
        {
            agent.SetDestination(player.position);
        }
    }
}