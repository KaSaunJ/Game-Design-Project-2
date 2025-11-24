using UnityEngine;
using System.Collections.Generic;

public class CityGenerator : MonoBehaviour
{
    [Header("City Prefabs")]
    public List<GameObject> buildingPrefabs;
    public List<GameObject> naturePrefabs;
    public List<GameObject> roadPrefabs;

    [Header("City Settings")]
    public int gridX = 10;
    public int gridZ = 10;
    public float spacing = 25f;

    /// <summary>
    /// Generates the city at the given start position
    /// and returns the end Z-position for chaining world sections.
    /// </summary>
    public Vector3 GenerateCity(Vector3 startPos)
    {
        // Generate Buildings & Nature
        for (int x = 0; x < gridX; x++)
        {
            for (int z = 0; z < gridZ; z++)
            {
                Vector3 pos = startPos + new Vector3(x * spacing, 0, z * spacing);

                if (Random.value < 0.7f)
                    SpawnFromList(buildingPrefabs, pos);
                else
                    SpawnFromList(naturePrefabs, pos);
            }
        }

        // Generate Roads
        GenerateRoads(startPos);

        // Return the end position so we know where the next biome starts
        return startPos + new Vector3(0, 0, gridZ * spacing);
    }

    private void GenerateRoads(Vector3 startPos)
    {
        for (int x = 0; x < gridX; x++)
        {
            Vector3 pos = startPos + new Vector3(x * spacing, 0, (gridZ / 2) * spacing);
            SpawnFromList(roadPrefabs, pos);
        }
    }

    private void SpawnFromList(List<GameObject> prefabs, Vector3 pos)
    {
        if (prefabs.Count == 0) return;
        GameObject prefab = prefabs[Random.Range(0, prefabs.Count)];
        Instantiate(prefab, pos, Quaternion.identity, transform);
    }
}
