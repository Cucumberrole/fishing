using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.Scripting;

public class FishSlot : MonoBehaviour
{
    GManager gameManager;

    [Header("UI")]
    public SpriteRenderer[] spriteRenderer;
    public TextMeshProUGUI[] countText;
    int i = 5;

    private void Start()
    {

        gameManager = GManager.instance;
        RefreshUI();
    }

    public void RefreshUI()
    {
        // 魚のカウントを作る
        Dictionary<Sprite, int> fishCount = new Dictionary<Sprite, int>();

        foreach (var fish in gameManager.fishList)
        {
            if (fishCount.ContainsKey(fish.fishSprite))
                fishCount[fish.fishSprite]++;
            else
                fishCount.Add(fish.fishSprite, 1);
        }
        Array.Resize(ref spriteRenderer, fishCount.Count);

        Array.Resize(ref countText, fishCount.Count);

        // いったんUIをリセット
        for (int i = 0; i < spriteRenderer.Length; i++)
        {
            spriteRenderer[i].sprite = null;
            if (countText != null && i < countText.Length)
                countText[i].text = "";

        }

        

        // 表示
        int index = 0;
        foreach (var pair in fishCount)
        {
            if (index >= spriteRenderer.Length)
                break;

            spriteRenderer[index].sprite = pair.Key;

            if (countText != null && index < countText.Length)
                countText[index].text = "×" + pair.Value;

            index++;
        }
    }
}