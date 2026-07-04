using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    private void Start()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayGameBGM2();
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlayButtonSE();
            }

            SceneManager.LoadScene("MainGame");
        }
    }
}