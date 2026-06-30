using System.Collections.Generic;
using UnityEngine;

public class GManager : MonoBehaviour
{
    public static GManager instance { get; private set; }

    [Header("お金")]
    public int roundMoney = 0; // 直前のゲームで獲得したお金
    public int totalMoney = 0; // 現在の総所持金
    public float moneyMultiplier = 1f; // 魚の金額倍率

    [Header("ゲーム時間")]
    public float gameTimeLimit = 10f; // 1ゲームの制限時間

    [Header("魚関連")]
    public int spawnCount = 20; // 魚影の出現数
    public float fishRespawnTime = 5f; // 魚の再出現時間
    public float detectRange = 3f; // 魚のルアー検知範囲
    public float biteDistance = 0.3f; // 魚が食いつく距離
    public FishData[] fishList; // ゲームに登場する魚一覧

    [Header("魚の大きさ")]
    public float smallFishScale = 0.3f; // 小型魚の大きさ
    public float mediumFishScale = 0.5f; // 中型魚の大きさ
    public float largeFishScale = 0.7f; // 大型魚の大きさ

    [Header("魚の釣り上げ演出")]
    public float fishLaunchSpeed = 10f; // 魚影が飛び上がる速さ
    public float fishReturnDuration = 1.2f; // 魚が戻るまでの時間
    public float fishReturnCurveHeight = 0.5f; // 戻る弧の高さ
    public float fishReturnSideOffset = 4f; // 戻る弧の横幅

    [Header("桟橋")]
    [Range(0, 12)]
    public int pierLevel = 0; // 現在の桟橋レベル

    [Header("投げる力")]
    public float minThrowPower = 5f; // 最低投擲力
    public float maxThrowPower = 20f; // 最大投擲力
    public float maxChargeTime = 2f; // 最大までためる時間

    [Header("巻き取り")]
    public float reelSpeed = 5f; // ルアーの巻き取り速度

    [Header("直前の釣果")]
    public List<FishData> caughtFishList = new(); // 直前に釣った魚一覧

    [Header("スキルレベル")]
    public int gameTimeLevel = 0; // 制限時間スキルのレベル
    public int spawnCountLevel = 0; // 出現数スキルのレベル
    public int detectRangeLevel = 0; // 検知範囲スキルのレベル
    public int throwPowerLevel = 0; // 投擲力スキルのレベル
    public int reelSpeedLevel = 0; // 巻き取り速度スキルのレベル

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

        caughtFishList = caughtFishes != null ? new List<FishData>(caughtFishes) : new List<FishData>();
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
