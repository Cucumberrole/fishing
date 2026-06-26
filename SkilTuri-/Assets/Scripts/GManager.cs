using System.Collections.Generic;
using UnityEngine;

public class GManager : MonoBehaviour
{
    public static GManager instance = null;

    [Header("お金")]
    public int roundMoney = 0; // 直前のゲームで獲得したお金
    public int totalMoney = 0; // 現在の総所持金

    [Header("ゲーム時間")]
    public float gameTimeLimit = 10f; // 1ゲームの制限時間

    [Header("魚関連")]
    public int spawnCount = 20;       // 魚のスポーン数
    public float detectRange = 3f;    // 魚がルアーを検知する範囲
    public FishData[] fishList;       // ゲームに登場する魚データ一覧

    [Header("桟橋")]
    [Range(0, 12)]
    public int pierLevel = 0; // 現在の桟橋レベル

    [Header("投げる力")]
    public float minThrowPower = 5f;  // 最低の投擲力
    public float maxThrowPower = 20f; // 最大の投擲力
    public float maxChargeTime = 2f;  // 最大までためる時間

    [Header("巻き取り")]
    public float reelSpeed = 5f; // ルアーの巻き取り速度

    [Header("直前の釣果")]
    public List<FishData> caughtFishList = new();

    [Header("スキルレベル")]
    public int gameTimeLevel = 0;
    public int spawnCountLevel = 0;
    public int detectRangeLevel = 0;
    public int throwPowerLevel = 0;
    public int reelSpeedLevel = 0;

    // 桟橋レベルごとの小型魚の出現確率
    private readonly int[] smallRates =
    {
        80, // レベル0
        70, // レベル1
        60, // レベル2
        50, // レベル3
        40, // レベル4
        30, // レベル5
        20, // レベル6
        10, // レベル7
        0,  // レベル8
        0,  // レベル9
        0,  // レベル10
        0,  // レベル11
        0   // レベル12
    };

    // 桟橋レベルごとの中型魚の出現確率
    private readonly int[] mediumRates =
    {
        20, // レベル0
        25, // レベル1
        30, // レベル2
        35, // レベル3
        40, // レベル4
        45, // レベル5
        50, // レベル6
        55, // レベル7
        50, // レベル8
        40, // レベル9
        30, // レベル10
        20, // レベル11
        0   // レベル12
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
        // 桟橋レベルが0～12を超えないようにする
        int level = Mathf.Clamp(pierLevel, 0, 12);

        int smallRate = smallRates[level];
        int mediumRate = mediumRates[level];

        // 0～99を抽選
        int randomValue = Random.Range(0, 100);

        // 小型魚の確率
        if (randomValue < smallRate)
        {
            return FishSize.Small;
        }

        // 中型魚の確率
        if (randomValue < smallRate + mediumRate)
        {
            return FishSize.Medium;
        }

        // 残りの確率は大型魚
        return FishSize.Large;
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