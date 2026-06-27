using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    public static ButtonManager Instance;
    public bool buttonPressed;

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
}