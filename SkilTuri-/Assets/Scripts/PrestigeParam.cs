
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class PrestigeParam : MonoBehaviour
{
    [SerializeField] private PrestigeSystem PrestigeSystem;
    [SerializeField] private PrestigeType type;
    [SerializeField] private int spendPoint;
    [SerializeField] private string PrestigeTitle;
    [SerializeField] private string PrestigeInformation;
    [SerializeField] private Text text;
    public Renderer[] targetRenderers;

    void Start()
    {
        CheckButtonOnOff();
    }

    public void OnClick()
    {
        if (AudioManager.Instance != null) AudioManager.Instance.PlayButtonSE();

        foreach (Renderer renderer in targetRenderers)
        {
            if (renderer != null) renderer.material.color = Color.black;
        }

        if (PrestigeSystem.IsSkill(type)) return;

        if (PrestigeSystem.CanLearnSkill(type, spendPoint))
        {
            PrestigeSystem.LearnSkill(type, spendPoint);
            ButtonManager.Instance.buttonPressed = true;
            Debug.Log(ButtonManager.Instance.buttonPressed);
            ChangeButtonColor(new Color(0f, 0f, 1f, 1f));
            text.text = PrestigeTitle + "を覚えた";
        }
        else
        {
            text.text = "スキルを覚えられません。";
        }
    }

    public void CheckButtonOnOff()
    {
        if (!PrestigeSystem.CanLearnSkill(type)) ChangeButtonColor(new Color(0.8f, 0.8f, 0.8f, 0.8f));
        else if (!PrestigeSystem.IsSkill(type)) ChangeButtonColor(new Color(1f, 1f, 1f, 1f));
    }

    public void SetText()
    {
        text.text = PrestigeTitle + "：消費スキルポイント" + spendPoint + "\n" + PrestigeInformation;
    }

    public void ResetText()
    {
        text.text = "";
    }

    public void ChangeButtonColor(Color color)
    {
        Button button = gameObject.GetComponent<Button>();
        ColorBlock cb = button.colors;
        cb.normalColor = color;
        cb.pressedColor = color;
        button.colors = cb;
    }
}
