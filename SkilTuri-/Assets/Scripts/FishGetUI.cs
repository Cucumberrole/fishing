using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FishGetUI : MonoBehaviour
{
    public Image fishImage;
    public TextMeshProUGUI fishNameText;

    public void Setup(FishData data)
    {
        if (data == null)
        {
            Debug.LogError("FishDataがありません！");
            return;
        }

        if (fishImage != null)
        {
            fishImage.sprite = data.fishSprite;
        }

        if (fishNameText != null)
        {
            fishNameText.text = data.fishName + " GET!!";
        }
    }
}
