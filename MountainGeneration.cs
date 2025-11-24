using UnityEngine;
using System.Collections.Generic;

public class MountainGenerator : MonoBehaviour
{
    [Header("Mountain Prefabs")]
    public List<GameObject> mountainPrefabs;

    [Header("Mountain Settings")]
    public int tilesX = 8;
    public int tilesZ = 10;
    public float spacing = 50f;
    public float heightIncreasePerRow = 4f;

    public Vector3 GenerateMountains(Vector3 startPos)
    {
        for (int x = 0; x < tilesX; x++)
        {
            for (int z = 0; z < tilesZ; z++)
            {
                Vector3 pos = startPos + new Vector3(
                    x * spacing,
                    z * heightIncreasePerRow,
                    z * spacing
                );

                SpawnFromList(mountainPrefabs, pos);
            }
        }

        return startPos + new Vector3(0, 0, tilesZ * spacing);
    }

    private void SpawnFromList(List<GameObject> prefabs, Vector3 pos)
    {
        if (prefabs.Count == 0) return;
        GameObject prefab = prefabs[Random.Range(0, prefabs.Count)];
        Instantiate(prefab, pos, Quaternion.identity, transform);
    }
}
