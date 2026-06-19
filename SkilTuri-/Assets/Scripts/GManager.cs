using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GManager : MonoBehaviour
{
    public static GManager instance = null;
    public int totalmoney; // Ç®ã‡ÇÃçáåv
    public int storeMoney; // ëçäzÇÃã‡äz


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
