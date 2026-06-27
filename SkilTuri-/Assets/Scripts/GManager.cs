using System.Collections.Generic;
using UnityEngine;

public class GManager : MonoBehaviour
{
    public static GManager instance { get; private set; }

    [Header("お金")]
    public int roundMoney = 0; // 直前のゲームで獲得したお金
    public int totalMoney = 0; // 現在の総所持金

    [Header("ゲーム時間")]
    public float gameTimeLimit = 10f; // 1ゲームの制限時間

    [Header("魚関連")]
    public int spawnCount = 20;          // 魚のスポーン数
    public float detectRange = 3f;       // 魚がルアーを検知する範囲
    public float biteDistance = 0.3f;    // 魚が食いつく距離
    public FishData[] fishList;          // ゲームに登場する魚データ一覧

    [Header("魚の大きさ")]
    public float smallFishScale = 0.3f;
    public float mediumFishScale = 0.5f;
    public float largeFishScale = 0.7f;

    [Header("魚の釣り上げ演出")]
    public float fishLaunchSpeed = 10f;
    public float fishReturnDuration = 1.2f;
    public float fishReturnCurveHeight = 0.5f;
    public float fishReturnSideOffset = 4f;

    [Header("桟橋")]
    [Range(0, 12)]
    public int pierLevel = 0;

    [Header("投げる力")]
    public float minThrowPower = 5f;
    public float maxThrowPower = 20f;
    public float maxChargeTime = 2f;

    [Header("巻き取り")]
    public float reelSpeed = 5f;

    [Header("直前の釣果")]
    public List<FishData> caughtFishList = new();

    [Header("スキルレベル")]
    public int gameTimeLevel = 0;
    public int spawnCountLevel = 0;
    public int detectRangeLevel = 0;
    public int throwPowerLevel = 0;
    public int reelSpeedLevel = 0;

    private readonly int[] smallRates =
    {
        80, 70, 60, 50, 40, 30, 20,
        10, 0, 0, 0, 0, 0
    };

    private readonly int[] mediumRates =
    {
        20, 25, 30, 35, 40, 45, 50,
        55, 50, 40, 30, 20, 0
    };

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public FishSize GetRandomFishSize()
    {
        int level = Mathf.Clamp(pierLevel, 0, 12);
        int randomValue = Random.Range(0, 100);

        int smallRate = smallRates[level];
        int mediumRate = mediumRates[level];

        if (randomValue < smallRate)
        {
            return FishSize.Small;
        }

        if (randomValue < smallRate + mediumRate)
        {
            return FishSize.Medium;
        }

        return FishSize.Large;
    }

    public FishData GetRandomFish(FishSize size)
    {
        if (fishList == null || fishList.Length == 0)
        {
            Debug.LogError("GManagerのFish Listが設定されていません！");
            return null;
        }

        List<FishData> candidates = new();

        foreach (FishData fishData in fishList)
        {
            if (fishData == null)
            {
                Debug.LogWarning("Fish Listに空の項目があります！");
                continue;
            }

            if (fishData.size == size)
            {
                candidates.Add(fishData);
            }
        }

        if (candidates.Count == 0)
        {
            Debug.LogWarning(size + "サイズの魚がFish Listに登録されていません！");
            return null;
        }

        return candidates[Random.Range(0, candidates.Count)];
    }

    public void SaveRoundResult(int earnedMoney, List<FishData> caughtFishes)
    {
        roundMoney = earnedMoney;
        totalMoney += earnedMoney;

        caughtFishList = caughtFishes != null
            ? new List<FishData>(caughtFishes)
            : new List<FishData>();
    }

    public void LevelUpPier()
    {
        if (pierLevel >= 12)
        {
            Debug.Log("桟橋は最大レベルです");
            return;
        }

        pierLevel++;
        Debug.Log("桟橋レベル：" + pierLevel);
    }
}
