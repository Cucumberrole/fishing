using UnityEngine;

public class pre : MonoBehaviour
{
    Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    public void Onclick()
    {
        if (AudioManager.Instance != null) AudioManager.Instance.PlayButtonSE();
        cam.transform.position -= new Vector3(18, 0, 0);
    }
}
