using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public float gameTime = 10f;
    public TextMeshProUGUI timeText;

    public int money = 0;
    // public int totalmoney = 0;

    public List<FishData> caughtFishList = new();

    public TextMeshProUGUI moneyText;

    private bool isRoundRunning = true;

    void Awake()
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

    void Update()
    {
        if (!isRoundRunning)
        {
            return;
        }

        gameTime -= Time.deltaTime;

        if (timeText != null)
        {
            timeText.text = Mathf.Ceil(gameTime).ToString();
        }

        if (gameTime <= 0f)
        {
            gameTime = 0f;
            isRoundRunning = false;

            if (GManager.instance != null)
            {
                GManager.instance.totalMoney = money;
            }
            else
            {
                Debug.LogError("GManager‚ªŒ©‚Â‚©‚è‚Ü‚¹‚ñI");
            }

            SceneManager.LoadScene("Result");
        }
    }

    public void AddMoney(int amount)
    {
        money += amount;

        if (moneyText != null)
        {
            moneyText.text = "MONEY ~ " + money;
        }
    }

    public void AddFish(FishData fish)
    {
        if (fish == null)
        {
            return;
        }

        caughtFishList.Add(fish);
    }

    public void StartRound()
    {
        money = 0;
        gameTime = 10f;
        caughtFishList.Clear();

        isRoundRunning = true;

        if (moneyText != null)
        {
            moneyText.text = "MONEY ~ 0";
        }
    }
}