using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class moneyviewer : MonoBehaviour
{
    public TextMeshProUGUI moneyText;

    void Start()
    {
        if (GManager.instance == null)
        {
            Debug.LogError("GManagerÇ™å©Ç¬Ç©ÇËÇ‹ÇπÇÒÅI");
            moneyText.text = "MONEY: 0";
            return;
        }

        int money = GManager.instance.totalMoney;
        moneyText.text = "MONEY: " + money;
    }
}
