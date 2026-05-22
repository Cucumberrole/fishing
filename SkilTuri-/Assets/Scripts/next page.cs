using UnityEngine;

public class GetCamera : MonoBehaviour
{
    Camera cam;

    void Start()
    {
        cam = Camera.main;
    }
    public void OnClick()
    {
          cam.transform.position += new Vector3(18, 0, 0);
    }
}