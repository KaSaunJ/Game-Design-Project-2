using UnityEngine;
using Unity.AI.Navigation; // <--- ADD THIS LIBRARY

public class WorldGenerationManager : MonoBehaviour
{
    [Header("Navigation")]
    public NavMeshSurface navMeshSurface; // <--- Drag the component here

    [Header("Section Generators")]
    public CityGenerator cityGenerator;
    public ForestGenerator forestGenerator;
    public MountainGenerator mountainGenerator;

    [Header("Zombie Spawner")]
    public ZombieMapSpawner zombieSpawner;
    public int cityZombieCount = 50;
    public int forestZombieCount = 30;
    public int mountainZombieCount = 20;

    [Header("Boss Settings")]
    public ZombieType forestBossData;

    [Header("World Settings")]
    public Vector3 worldStartPosition = Vector3.zero;

    void Start()
    {
        GenerateWorld();
    }

    public void GenerateWorld()
    {
        // 1. GENERATE MAP
        Vector3 cityStart = worldStartPosition;
        Vector3 cityEnd = cityGenerator.GenerateCity(cityStart);
        Vector3 forestEnd = forestGenerator.GenerateForest(cityEnd);
        mountainGenerator.GenerateMountains(forestEnd);

        // 2. BAKE NAVMESH (Crucial Step!)
        // This tells Unity to scan the newly created city/forest and define walkable areas
        if (navMeshSurface != null)
        {
            navMeshSurface.BuildNavMesh();
            Debug.Log("NavMesh Baked Successfully.");
        }

        // 3. SPAWN ZOMBIES (Do this AFTER baking, so they find the NavMesh)
        
        // City Zombies
        float cityWidth = cityGenerator.gridX * cityGenerator.spacing;
        float cityLength = cityGenerator.gridZ * cityGenerator.spacing;
        zombieSpawner.SpawnZombiesInArea(cityStart, cityWidth, cityLength, cityZombieCount);

        // Forest Zombies
        zombieSpawner.SpawnZombiesInArea(cityEnd, forestGenerator.forestWidth, forestGenerator.forestLength, forestZombieCount);

        // Forest Boss
        Vector3 forestCenter = cityEnd + new Vector3(forestGenerator.forestWidth / 2, 0, forestGenerator.forestLength / 2);
        Vector3 bossSpawnPos = forestCenter + new Vector3(5f, 0, 5f); 
        zombieSpawner.SpawnSpecificZombie(forestBossData, bossSpawnPos);

        // Mountain Zombies
        float mountWidth = mountainGenerator.tilesX * mountainGenerator.spacing;
        float mountLength = mountainGenerator.tilesZ * mountainGenerator.spacing;
        zombieSpawner.SpawnZombiesInArea(forestEnd, mountWidth, mountLength, mountainZombieCount);

        Debug.Log("World generation complete.");
    }
}