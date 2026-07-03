using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FishItemUI : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI countText;

    public void Set(Sprite sprite, int count)
    {
        icon.sprite = sprite;
        countText.text = "Å~" + count;
    }
}