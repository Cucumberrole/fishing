using UnityEngine;
using UnityEngine.UI;

public class SkillUI : MonoBehaviour
{
    [SerializeField] private Text skillText;

    void Start()
    {
        UpdateText();
    }

    public void UpdateText()
    {
        skillText.text = "      xÅF" + SkillSystem.Instance.SkillPoint;
    }
}