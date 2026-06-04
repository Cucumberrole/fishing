using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    public GameObject fishPrefab;
    public int spawnCount = 20;

    public Vector2 spawnMin;
    public Vector2 spawnMax;

    void Start()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            Vector2 pos = new Vector2(Random.Range(spawnMin.x, spawnMax.x), Random.Range(spawnMin.y, spawnMax.y));

            GameObject fishObj = Instantiate(fishPrefab, pos, Quaternion.identity);
            Fish fish = fishObj.GetComponent<Fish>();
            fish.size = (FishSize)Random.Range(0, 3);
        }
    }
}