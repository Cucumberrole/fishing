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

    [Header("投げる力")]
    public float minThrowPower = 5f;  // 最低の投擲力
    public float maxThrowPower = 20f; // 最大の投擲力
    public float maxChargeTime = 2f;  // 最大までためる時間

    [Header("巻き取り")]
    public float reelSpeed = 5f;      // ルアーの巻き取り速度

    [Header("直前の釣果")]
    public List<FishData> caughtFishList = new();

    [Header("スキルレベル")]
    public int gameTimeLevel = 0;
    public int spawnCountLevel = 0;
    public int detectRangeLevel = 0;
    public int throwPowerLevel = 0;
    public int reelSpeedLevel = 0;

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
}