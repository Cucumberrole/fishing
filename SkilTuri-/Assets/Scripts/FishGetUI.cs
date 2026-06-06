using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FishGetUI : MonoBehaviour
{
    public Image fishImage;
    public TextMeshProUGUI fishNameText;

    public void Setup(FishData data)
    {
        fishImage.sprite = data.fishSprite;

        fishNameText.text = data.fishName + " GET!!";
    }
}