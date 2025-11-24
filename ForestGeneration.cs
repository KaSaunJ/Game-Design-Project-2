using UnityEngine;
using System.Collections.Generic;

public class ForestGenerator : MonoBehaviour
{
    [Header("Forest Prefabs")]
    public List<GameObject> treePrefabs;
    public List<GameObject> bushPrefabs;
    public GameObject specialBuilding;

    [Header("Forest Settings")]
    public float forestLength = 250f;
    public float forestWidth = 300f;
    public int spawnCount = 200;

    public Vector3 GenerateForest(Vector3 startPos)
    {
        // Spawn trees and bushes randomly
        for (int i = 0; i < spawnCount; i++)
        {
            Vector3 randomOffset = new Vector3(
                Random.Range(0, forestWidth),
                0,
                Random.Range(0, forestLength)
            );

            Vector3 pos = startPos + randomOffset;

            if (Random.value < 0.8f)
                SpawnFromList(treePrefabs, pos);
            else
                SpawnFromList(bushPrefabs, pos);
        }

        // Spawn special building in the forest center
        Vector3 center = startPos + new Vector3(forestWidth / 2, 0, forestLength / 2);
        Instantiate(specialBuilding, center, Quaternion.identity, transform);

        return startPos + new Vector3(0, 0, forestLength);
    }

    private void SpawnFromList(List<GameObject> prefabs, Vector3 pos)
    {
        if (prefabs.Count == 0) return;
        GameObject prefab = prefabs[Random.Range(0, prefabs.Count)];
        Instantiate(prefab, pos, Quaternion.identity, transform);
    }
}
