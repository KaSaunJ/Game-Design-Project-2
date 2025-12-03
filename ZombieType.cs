using UnityEngine;

[CreateAssetMenu(fileName = "Data_NewZombie", menuName = "Zombies/Zombie Type")]
public class ZombieType : ScriptableObject
{
    [Header("Visuals")]
    public string typeName;      // Matches your existing data
    public GameObject prefab;    // Matches your existing data

    [Header("Stats")]
    public float maxHealth = 100f; // Matches your existing data
    public float moveSpeed = 3.5f; // Matches your existing data
    public int scoreValue = 10;    // Matches your existing data
    
    [Header("Combat")]
    public float damage = 10f;               // Added for Player Damage logic
    public float penetrationResistance = 5f; // Added for your Gun System

    [Header("Spawning")]
    [Range(0f, 100f)] 
    public float spawnChanceWeight = 50f; // Matches your existing data
}