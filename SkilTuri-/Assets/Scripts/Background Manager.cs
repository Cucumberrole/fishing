using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    public GameObject[] backgroundPrefabs;

    void Start()
    {
        int stage = GManager.instance.pierLevel;

        Instantiate(backgroundPrefabs[stage - 0], Vector3.zero, Quaternion.identity);
    }
}
