using UnityEngine;

public class pre : MonoBehaviour
{
    Camera cam;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = Camera .main;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Onclick()
    {
        cam.transform.position -= new Vector3(18, 0, 0);
    }
}
