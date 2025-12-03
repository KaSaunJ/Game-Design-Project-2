using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [Header("References")]
    public WaveDirector director; // Drag your WaveDirector component here

    [Header("Difficulty Ramp")]
    public float difficultyIncreaseInterval = 15f; // Every 15 seconds, make it harder
    public float spawnRateDecreaseAmount = 0.1f; // Shave 0.1s off the spawn timer
    public float minimumSpawnRate = 0.5f; // Cap the speed so it doesn't crash the game (0.5s is chaos!)

    [Header("Debug")]
    public float currentSpawnRate;
    private float nextDifficultyTime;

    void Start()
    {
        if (director == null) director = GetComponent<WaveDirector>();
        
        // Initialize logic
        currentSpawnRate = director.spawnRate;
        nextDifficultyTime = Time.time + difficultyIncreaseInterval;
    }

    void Update()
    {
        if (Time.time >= nextDifficultyTime)
        {
            IncreaseDifficulty();
            nextDifficultyTime = Time.time + difficultyIncreaseInterval;
        }
    }

    void IncreaseDifficulty()
    {
        // Lower the spawn rate (making zombies appear faster)
        currentSpawnRate -= spawnRateDecreaseAmount;

        // Don't let it go below the chaos limit
        if (currentSpawnRate < minimumSpawnRate)
        {
            currentSpawnRate = minimumSpawnRate;
        }

        // Apply to director
        director.spawnRate = currentSpawnRate;

        Debug.Log($"DIFFICULTY INCREASED: Zombies now spawning every {currentSpawnRate} seconds.");
    }
}