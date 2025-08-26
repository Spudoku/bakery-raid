
using UnityEngine;

public class RandomizedSpawner : MonoBehaviour
{
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] GameObject prefab;

    public void SpawnBetty()
    {
        Vector2 position = spawnPoints[Random.Range(0, spawnPoints.Length)].position;
        GameObject newBetty = Instantiate(prefab, position, Quaternion.identity);
    }
}
