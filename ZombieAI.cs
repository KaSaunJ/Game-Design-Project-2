using UnityEngine;
using UnityEngine.AI;
using System.Linq;
using System.Collections;
using AlterunaFPS; // REQUIRED: To access the Player's Health script

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))] 
public class ZombieAI : MonoBehaviour
{
    [Header("Data")]
    public ZombieType zombieData; 
    
    [Header("Settings")]
    public float updateRate = 0.5f; 
    public float attackRange = 2f;
    public float attackCooldown = 2f;
    public float damageDelay = 0.5f; // Adjust this to match the "punch" moment in animation
    
    // Add a default damage value since your Data files don't have a 'damage' variable yet
    public float damage = 10f; 

    private NavMeshAgent agent;
    private Transform targetPlayer;
    private Animator ani; 
    private float lastAttackTime;
    private bool isStopped = false;

    public void Initialize(ZombieType data)
    {
        zombieData = data;
        
        agent = GetComponent<NavMeshAgent>();
        
        // FIX: Matches 'moveSpeed' from your Data files
        if(zombieData != null) agent.speed = zombieData.moveSpeed; 
        
        // FIX: Matches 'maxHealth' from your Data files
        if(TryGetComponent(out ZombieHealth healthScript))
        {
            if(zombieData != null)
            {
                healthScript.maxHealth = zombieData.maxHealth;
                // Note: PenetrationResistance is not in your Data files yet, so we use the default on the prefab
            }
        }

        // FIX: Matches 'typeName' from your Data files
        if(zombieData != null)
        {
            if(zombieData.typeName == "Tank") transform.localScale = Vector3.one * 1.5f;
            if(zombieData.typeName == "Runner") transform.localScale = Vector3.one * 0.8f;
        }
    }

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        ani = GetComponent<Animator>(); 
    }

    void Start()
    {
        // Fallback in case Initialize wasn't called (e.g. placed in scene manually)
        if(zombieData != null && agent != null) agent.speed = zombieData.moveSpeed;
        
        InvokeRepeating(nameof(FindClosestPlayer), 0f, updateRate);
    }

    void Update()
    {
        if (targetPlayer == null) return;

        // Sync Animation Speed
        ani.SetFloat("Speed", agent.velocity.magnitude);

        float distance = Vector3.Distance(transform.position, targetPlayer.position);
        
        // 1. Chase
        if (distance > attackRange)
        {
            if(isStopped)
            {
                agent.isStopped = false;
                isStopped = false;
            }
            agent.SetDestination(targetPlayer.position);
        }
        // 2. Attack
        else
        {
            AttackBehavior();
        }
    }

    void AttackBehavior()
    {
        agent.isStopped = true; 
        isStopped = true;
        
        // Rotate to face player immediately
        Vector3 direction = (targetPlayer.position - transform.position).normalized;
        if(direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
        }

        if (Time.time >= lastAttackTime + attackCooldown)
        {
            ani.SetTrigger("Attack"); 
            StartCoroutine(DealDamageAfterDelay());
            lastAttackTime = Time.time;
        }
    }

    IEnumerator DealDamageAfterDelay()
    {
        // Wait for animation to swing
        yield return new WaitForSeconds(damageDelay);

        if (targetPlayer != null)
        {
            float distance = Vector3.Distance(transform.position, targetPlayer.position);
            
            // Check if player is still in range (with small buffer)
            if (distance <= attackRange + 1.0f) 
            {
                // Access AlterunaFPS Health Component
                Health playerHealth = targetPlayer.GetComponent<Health>();
                if (playerHealth != null)
                {
                    // ID 0 = World Damage
                    playerHealth.TakeDamage(0, damage);
                }
            }
        }
    }

    void FindClosestPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        if (players.Length == 0) return;

        GameObject closest = null;
        float closestDist = Mathf.Infinity;

        foreach(GameObject p in players)
        {
            float dist = Vector3.Distance(transform.position, p.transform.position);
            if(dist < closestDist)
            {
                closestDist = dist;
                closest = p;
            }
        }

        if(closest != null) targetPlayer = closest.transform;
    }
}