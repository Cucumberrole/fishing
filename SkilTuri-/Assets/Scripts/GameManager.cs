using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public float gameTime = 10f;
    public TextMeshProUGUI timeText;

    void Update()
    {
        gameTime -= Time.deltaTime;

        timeText.text = Mathf.Ceil(gameTime).ToString();

        if (gameTime <= 0)
        {
            SceneManager.LoadScene("Result");
        }
    }
}