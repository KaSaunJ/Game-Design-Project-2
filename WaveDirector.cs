using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using AlterunaFPS; // Needed for Health script

public class WaveDirector : MonoBehaviour
{
    [Header("Zombie Configuration")]
    public List<ZombieType> zombieTypes; 

    [Header("Wave Settings")]
    public float spawnRate = 2.0f; // This will be controlled by WaveManager
    public Transform playerTransform; 
    
    [Header("Spawn Range")]
    public float minSpawnDistance = 20f;
    public float maxSpawnDistance = 40f;

    private float nextSpawnTime;

    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnRandomZombie();
            nextSpawnTime = Time.time + spawnRate;
        }
    }

    void SpawnRandomZombie()
    {
        if (zombieTypes.Count == 0) return;
        ZombieType selectedType = GetWeightedRandomType();

        Vector3 spawnPos = GetValidSpawnPointAroundPlayer();

        if (spawnPos != Vector3.zero)
        {
            GameObject zombieObj = Instantiate(selectedType.prefab, spawnPos, Quaternion.identity);

            // FIX 1: Use 'ZombieHealth' script to match your setup
            if(zombieObj.TryGetComponent(out ZombieHealth hpScript))
            {
                // FIX 2: Use 'maxHealth' (not health)
                hpScript.maxHealth = selectedType.maxHealth; 
                hpScript.PenetrationResistance = selectedType.penetrationResistance;
            }

            // Also check for ZombieAI to initialize it
            if(zombieObj.TryGetComponent(out ZombieAI aiScript))
            {
                aiScript.Initialize(selectedType);
            }

            NavMeshAgent agent = zombieObj.GetComponent<NavMeshAgent>();
            // FIX 3: Use 'moveSpeed' (not speed)
            if(agent != null) agent.speed = selectedType.moveSpeed; 
        }
    }

    Vector3 GetValidSpawnPointAroundPlayer()
    {
        if (playerTransform == null) return Vector3.zero;

        for (int i = 0; i < 10; i++) 
        {
            Vector2 randomCircle = Random.insideUnitCircle.normalized * Random.Range(minSpawnDistance, maxSpawnDistance);
            Vector3 randomPoint = playerTransform.position + new Vector3(randomCircle.x, 0, randomCircle.y);

            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 5.0f, NavMesh.AllAreas))
            {
                return hit.position;
            }
        }
        return Vector3.zero; 
    }

    ZombieType GetWeightedRandomType()
    {
        float totalWeight = 0;
        foreach (var type in zombieTypes) totalWeight += type.spawnChanceWeight;

        float randomValue = Random.Range(0, totalWeight);
        float currentWeight = 0;

        foreach (var type in zombieTypes)
        {
            currentWeight += type.spawnChanceWeight;
            if (randomValue <= currentWeight)
            {
                return type;
            }
        }
        return zombieTypes[0]; 
    }
}