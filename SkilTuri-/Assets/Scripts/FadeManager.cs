using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class FadeManager : MonoBehaviour
{
    public Image fadeImage;
    public float fadeSpeed = 1f;

    private bool isTransitioning = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isTransitioning)
        {
            StartCoroutine(FadeAndLoadScene());
        }
    }

    IEnumerator FadeAndLoadScene()
    {
        isTransitioning = true;

        Color color = fadeImage.color;

        while (color.a < 1f)
        {
            color.a += Time.deltaTime * fadeSpeed;
            fadeImage.color = color;

            yield return null;
        }

        SceneManager.LoadScene("MainGame");
    }
}