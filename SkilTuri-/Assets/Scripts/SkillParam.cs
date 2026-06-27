using UnityEngine;
using UnityEngine.UI;

public class SkillParam : MonoBehaviour
{
    [SerializeField] private SkillType type;

    [SerializeField] private int spendPoint;
    [SerializeField] private int spendCount;

    [SerializeField] private string skillTitle;
    [SerializeField] private string skillInformation;

    [SerializeField] private Text text;

    private SkillSystem skillSystem;

  
        void Start()
        {
            Invoke(nameof(Init), 0.1f);
        }

        void Init()
        {
            skillSystem = SkillSystem.Instance;

            if (text == null)
                text = GetComponentInChildren<Text>();

            Refresh();
        }
    

    private void Refresh()
    {
        if (skillSystem == null) return;

        CheckButtonOnOff();

        if (skillSystem.IsSkill(type))
        {
            ChangeButtonColor(Color.blue);
        }
    }

    public void OnClick()
    {
        Debug.Log("クリックされた");

        if (skillSystem == null)
        {
            Debug.LogError("SkillSystemがnull");
            return;
        }

        if (skillSystem.IsSkill(type))
            return;

        if (skillSystem.CanLearnSkill(type, spendPoint, spendCount))
        {
            skillSystem.LearnSkill(type, spendPoint, spendCount);

            ChangeButtonColor(Color.blue);

            if (text != null)
                text.text = skillTitle + "を覚えた";
        }
        else
        {
            if (text != null)
                text.text = "スキルを覚えられません。";
        }

        RefreshAll();
    }

    private void RefreshAll()
    {
        SkillParam[] all = FindObjectsOfType<SkillParam>();

        foreach (var s in all)
        {
            if (s != null)
                s.RefreshInternal();
        }
    }

    private void RefreshInternal()
    {
        if (skillSystem == null) return;

        CheckButtonOnOff();

        if (skillSystem.IsSkill(type))
        {
            ChangeButtonColor(Color.blue);
        }
    }

    public void CheckButtonOnOff()
    {
        if (skillSystem == null) return;

        if (skillSystem.IsSkill(type))
        {
            ChangeButtonColor(Color.blue);
        }
        else if (!skillSystem.CanLearnSkill(type))
        {
            ChangeButtonColor(new Color(0.8f, 0.8f, 0.8f, 0.8f));
        }
        else
        {
            ChangeButtonColor(Color.white);
        }
    }

    public void SetText()
    {
        if (text == null) return;

        text.text =
            skillTitle +
            "：消費スキルポイント" +
            spendPoint +
            "\n" +
            skillInformation;
    }

    public void ResetText()
    {
        if (text == null) return;
        text.text = "";
    }

    public void ChangeButtonColor(Color color)
    {
        Button button = GetComponent<Button>();
        if (button == null) return;

        ColorBlock cb = button.colors;
        cb.normalColor = color;
        cb.pressedColor = color;
        button.colors = cb;
        button.interactable = true;
    }
}