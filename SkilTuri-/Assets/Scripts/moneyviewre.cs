using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class moneyviewer : MonoBehaviour
{
    public TextMeshProUGUI moneyText;

    void Start()
    {
        // GameManagerからスコアを取得してテキストに反映
        int money = GManager.instance.money;
       moneyText.text = "MONEY: " + money.ToString();
    }
}
