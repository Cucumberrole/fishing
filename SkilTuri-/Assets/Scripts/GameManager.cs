using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public float gameTime = 10f;
    public TextMeshProUGUI timeText;

    public int money = 0;
    public TextMeshProUGUI moneyText;

    void Awake()
    {
        Instance = this;
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

        moneyText.text = "MONEY ~ " + money;
    }
}