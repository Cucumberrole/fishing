using TMPro;
using UnityEngine;

public class ResultManager : MonoBehaviour
{
    public TextMeshProUGUI resultMoneyText;
    public Transform fishListParent;
    public FishGetUI fishUIPrefab;

    private void Start()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.StopAmbient();
            AudioManager.Instance.PlayGameBGM2();
        }

        if (GManager.instance == null)
        {
            Debug.LogError("GManagerが見つかりません！");
            return;
        }

        ShowMoney();
        ShowCaughtFish();
    }

    private void ShowMoney()
    {
        if (resultMoneyText != null) resultMoneyText.text = "獲得金額：" + GManager.instance.roundMoney;
    }

    private void ShowCaughtFish()
    {
        if (fishListParent == null || fishUIPrefab == null) return;

        foreach (FishData fishData in GManager.instance.caughtFishList)
        {
            if (fishData == null) continue;
            FishGetUI fishUI = Instantiate(fishUIPrefab, fishListParent);
            fishUI.Setup(fishData);
        }
    }
}
