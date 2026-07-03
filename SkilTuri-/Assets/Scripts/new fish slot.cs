using System.Collections.Generic;
using UnityEngine;

public class newfishslot : MonoBehaviour
{
    GManager gameManager;

    [Header("UI")]
    public Transform content; // 親（VerticalLayoutGroup）
    public FishItemUI prefab;

    private List<GameObject> createdUI = new List<GameObject>();

    private void Start()
    {
        gameManager = GManager.instance;
        RefreshUI();
    }

    public void RefreshUI()
    {
        // 既存UIを削除
        foreach (var obj in createdUI)
        {
            Destroy(obj);
        }
        createdUI.Clear();

        // 集計
        Dictionary<Sprite, int> fishCount = new Dictionary<Sprite, int>();

        foreach (var fish in gameManager.caughtFishList)
        {
            if (fishCount.ContainsKey(fish.fishSprite))
                fishCount[fish.fishSprite]++;
            else
                fishCount.Add(fish.fishSprite, 1);
        }

        // UI生成
        foreach (var pair in fishCount)
        {
            FishItemUI ui = Instantiate(prefab, content);
            ui.Set(pair.Key, pair.Value);

            createdUI.Add(ui.gameObject);
        }
    }
}
