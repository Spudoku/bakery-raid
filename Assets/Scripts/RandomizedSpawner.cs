
using UnityEngine;

public class RandomizedSpawner : MonoBehaviour
{
    [SerializeField] Transform[] spawnPoints;
    public GameObject prefab;

    public GameObject Spawn()
    {
        Vector2 position = spawnPoints[Random.Range(0, spawnPoints.Length)].position;
        GameObject newBetty = Instantiate(prefab, position, Quaternion.identity);
        return newBetty;
    }
}
