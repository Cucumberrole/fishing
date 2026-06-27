using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    public GameObject fishPrefab;
    public Vector2 spawnMin;
    public Vector2 spawnMax;

    private void Start()
    {
        int spawnCount = 20;

        if (GManager.instance != null)
        {
            spawnCount = GManager.instance.spawnCount;
        }
        else
        {
            Debug.LogWarning("GManagerが見つからないため、魚を20匹生成します");
        }

        for (int i = 0; i < spawnCount; i++)
        {
            Vector2 position = new(
                Random.Range(spawnMin.x, spawnMax.x),
                Random.Range(spawnMin.y, spawnMax.y)
            );

            GameObject fishObject = Instantiate(fishPrefab, position, Quaternion.identity);
            Fish fish = fishObject.GetComponent<Fish>();

            if (fish == null)
            {
                Debug.LogError(fishPrefab.name + "にFishコンポーネントがありません！");
                Destroy(fishObject);
                continue;
            }

            fish.size = GManager.instance != null
                ? GManager.instance.GetRandomFishSize()
                : (FishSize)Random.Range(0, 3);
        }
    }
}
