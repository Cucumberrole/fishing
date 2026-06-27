using TMPro;
using UnityEngine;

public class moneyviewer : MonoBehaviour
{
    public TextMeshProUGUI moneyText;

    private void Start()
    {
        if (moneyText == null)
        {
            Debug.LogError("Money Textが設定されていません！");
            return;
        }

        if (GManager.instance == null)
        {
            Debug.LogError("GManagerが見つかりません！");
            moneyText.text = "MONEY: 0";
            return;
        }

        moneyText.text = "MONEY: " + GManager.instance.totalMoney;
    }
}
