using UnityEngine;
using UnityEngine.UI;
public class page : MonoBehaviour
{
    Camera cam;
    [SerializeField]
    Button myButton;
    Vector3 b = new Vector3(18.0f, 0.0f, 0.0f);
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
       if (cam.transform.position.x == 0)
        {
            myButton.interactable = false;
        }
         if (cam.transform.position.x > 0)
        {
            myButton.interactable = true;

        }
    }
}
