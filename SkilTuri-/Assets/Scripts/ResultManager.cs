using UnityEngine;
using TMPro;

public class ResultManager : MonoBehaviour
{
    public TextMeshProUGUI resultMoneyText;

    public Transform fishListParent;
    public FishGetUI fishUIPrefab;

    void Start()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManagerÇ™å©Ç¬Ç©ÇËÇ‹ÇπÇÒ");
            return;
        }

        ShowMoney();
        ShowCaughtFish();
    }

    void ShowMoney()
    {
        int resultMoney = GameManager.Instance.money;

        resultMoneyText.text = "älìæã‡äzÅF" + resultMoney.ToString();
    }

    void ShowCaughtFish()
    {
        foreach (FishData fishData in GameManager.Instance.caughtFishList)
        {
            FishGetUI fishUI = Instantiate(fishUIPrefab, fishListParent);

            fishUI.Setup(fishData);
        }
    }
}