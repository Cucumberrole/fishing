using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("ゲーム中のUI")]
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI moneyText;

    [Header("今回のゲーム中だけ使う値")]
    public float gameTime;
    public int money = 0;
    public List<FishData> caughtFishList = new();

    private bool isRoundRunning;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        StartRound();
    }

    private void Update()
    {
        if (!isRoundRunning)
        {
            return;
        }

        gameTime -= Time.deltaTime;

        if (timeText != null)
        {
            timeText.text = Mathf.CeilToInt(Mathf.Max(gameTime, 0f)).ToString();
        }

        if (gameTime <= 0f)
        {
            EndRound();
        }
    }

    public void AddMoney(int amount)
    {
        money += amount;

        if (moneyText != null)
        {
            moneyText.text = "MONEY × " + money;
        }
    }

    public void AddFish(FishData fish)
    {
        if (fish != null)
        {
            caughtFishList.Add(fish);
        }
    }

    public void StartRound()
    {
        money = 0;
        caughtFishList.Clear();
        isRoundRunning = true;

        gameTime = GManager.instance != null
            ? GManager.instance.gameTimeLimit
            : 10f;

        if (moneyText != null)
        {
            moneyText.text = "MONEY × 0";
        }

        if (timeText != null)
        {
            timeText.text = Mathf.CeilToInt(gameTime).ToString();
        }
    }

    private void EndRound()
    {
        gameTime = 0f;
        isRoundRunning = false;

        if (GManager.instance != null)
        {
            GManager.instance.SaveRoundResult(money, caughtFishList);
        }
        else
        {
            Debug.LogError("GManagerが見つかりません！");
        }

        SceneManager.LoadScene("Result");
    }
}
