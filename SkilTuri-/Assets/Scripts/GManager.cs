using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GManager : MonoBehaviour
{
    public static GManager instance = null;
    public int totalmoney; // お金の合計
    public int storeMoney; // 総額の金額

    public int spawnCount = 20;     // 魚のスポーン数
    public float gameTime = 10f;    // 制限時間
    public float detectRange = 3f;  // 魚がかかる範囲(ルアーの検知範囲)




    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}