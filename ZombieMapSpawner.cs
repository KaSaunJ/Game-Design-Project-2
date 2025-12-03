using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class ZombieMapSpawner : MonoBehaviour
{
    [Header("Zombie Configuration")]
    public List<ZombieType> zombieTypes; 

    [Header("Layer Settings")]
    public LayerMask groundLayer;   
    public LayerMask obstacleLayer; 

    public void SpawnZombiesInArea(Vector3 startPos, float width, float length, int count)
    {
        if (zombieTypes == null || zombieTypes.Count == 0) return;

        int spawned = 0;
        int attempts = 0;

        while (spawned < count && attempts < count * 10)
        {
            attempts++;
            float randomX = Random.Range(0, width);
            float randomZ = Random.Range(0, length);
            Vector3 candidatePos = startPos + new Vector3(randomX, 200f, randomZ);

            RaycastHit hit;
            if (Physics.Raycast(candidatePos, Vector3.down, out hit, 300f, groundLayer))
            {
                Vector3 spawnPoint = hit.point;
                if (!Physics.CheckSphere(spawnPoint, 1f, obstacleLayer))
                {
                    ZombieType selectedType = GetWeightedRandomType();
                    SpawnZombie(selectedType, spawnPoint);
                    spawned++;
                }
            }
        }
        Debug.Log($"Initial Spawn Complete: {spawned} zombies in area.");
    }

    public void SpawnSpecificZombie(ZombieType specificType, Vector3 targetPos)
    {
        if (specificType == null) return;

        Vector3 spawnPos = targetPos + Vector3.up * 50f; 
        RaycastHit hit;

        if (Physics.Raycast(spawnPos, Vector3.down, out hit, 100f, groundLayer))
        {
            // FIX 1: Use 'typeName' (not zombieName)
            Debug.Log($"BOSS SPAWNED: {specificType.typeName} at {hit.point}");
            SpawnZombie(specificType, hit.point);
        }
        else
        {
            Debug.LogWarning("Could not find ground for Boss spawn!");
        }
    }

    private void SpawnZombie(ZombieType type, Vector3 position)
    {
        if (type.prefab == null) return;

        GameObject zombieObj = Instantiate(type.prefab, position, Quaternion.identity);

        ZombieAI aiScript = zombieObj.GetComponent<ZombieAI>();
        if (aiScript != null)
        {
            aiScript.Initialize(type);
        }

        ZombieHealth healthScript = zombieObj.GetComponent<ZombieHealth>();
        if (healthScript != null)
        {
            // FIX 2: Use 'maxHealth'
            healthScript.maxHealth = type.maxHealth;
            healthScript.PenetrationResistance = type.penetrationResistance;
        }
        
        NavMeshAgent agent = zombieObj.GetComponent<NavMeshAgent>();
        // FIX 3: Use 'moveSpeed'
        if (agent != null) 
        {
            agent.speed = type.moveSpeed;
        }
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