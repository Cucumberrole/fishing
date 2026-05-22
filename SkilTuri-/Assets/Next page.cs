using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    private Camera cam;
    private Vector3 mousePos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnClick()
    {

        cam.transform.position += new Vector3(18, 0, 0);
      
    }
}
