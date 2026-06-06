using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public float gameTime = 10f;
    public TextMeshProUGUI timeText;

    public int money = 0; // ç°âÒÇÃÉâÉEÉìÉhÇÃÇ®ã‡
    public int totalmoney = 0; // Ç®ã‡ÇÃçáåv
    public List<FishData> caughtFishList = new();

    public TextMeshProUGUI moneyText;




    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }




    void Update()
    {
        gameTime -= Time.deltaTime;

        timeText.text = Mathf.Ceil(gameTime).ToString();

        if (gameTime <= 0)
        {
            SceneManager.LoadScene("Result");
        }
    }




    public void AddMoney(int amount)
    {
        money += amount;
        totalmoney += amount;

        moneyText.text = "MONEY Å~ " + money;
    }




    public void AddFish(FishData fish)
    {
        caughtFishList.Add(fish);
    }




    public void StartRound()
    {
        money = 0;
        caughtFishList.Clear();

        if (moneyText != null)
        {
            moneyText.text = "MONEY Å~ 0";
        }
    }
}