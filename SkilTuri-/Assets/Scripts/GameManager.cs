using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public float gameTime = 10f;

    void Update()
    {
        gameTime -= Time.deltaTime;

        if (gameTime <= 0)
        {
            SceneManager.LoadScene("Result");
        }
    }
}