using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    public GameObject fishPrefab;

    // GManagerが存在しなかった場合に使う初期値
    public int spawnCount = 20;

    public Vector2 spawnMin;
    public Vector2 spawnMax;

    void Start()
    {
        // 強化済みのスポーン数を受け取る
        if (GManager.instance != null)
        {
            spawnCount = GManager.instance.spawnCount;
        }
        else
        {
            Debug.LogWarning("GManagerが見つかりません");
        }

        for (int i = 0; i < spawnCount; i++)
        {
            Vector2 pos = new Vector2(Random.Range(spawnMin.x, spawnMax.x), Random.Range(spawnMin.y, spawnMax.y));

            GameObject fishObj = Instantiate(fishPrefab, pos, Quaternion.identity);

            Fish fish = fishObj.GetComponent<Fish>();

            if (fish == null)
            {
                Debug.LogError(fishPrefab.name + "にFishが付いていません");

                Destroy(fishObj);
                continue;
            }

            if (GManager.instance != null)
            {
                // 桟橋レベルに応じた確率でサイズを決める
                fish.size = GManager.instance.GetRandomFishSize();
            }
            else
            {
                // GManagerがない場合は均等抽選
                fish.size = (FishSize)Random.Range(0, 3);
            }
        }
    }
}