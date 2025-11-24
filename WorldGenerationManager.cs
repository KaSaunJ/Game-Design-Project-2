using UnityEngine;

public class WorldGenerationManager : MonoBehaviour
{
    [Header("Section Generators")]
    public CityGenerator cityGenerator;
    public ForestGenerator forestGenerator;
    public MountainGenerator mountainGenerator;

    [Header("World Settings")]
    public Vector3 worldStartPosition = Vector3.zero;

    void Start()
    {
        GenerateWorld();
    }

    public void GenerateWorld()
    {
        Vector3 cityEnd = cityGenerator.GenerateCity(worldStartPosition);
        Vector3 forestEnd = forestGenerator.GenerateForest(cityEnd);
        mountainGenerator.GenerateMountains(forestEnd);

        Debug.Log("World generation complete.");
    }
}
