using UnityEngine;
using UnityEngine.UI;

public enum SkillType
{
    Hook1, Hook2, Hook3,
    GrowFish1, GrowFish2, GrowFish3,
    Money1, Money2, Money3,
    Time1, Time2, Time3,
    Repop1, Repop2, Repop3,
    Pier1, Pier2,
    Hook4, Hook5, Hook6,
    GrowFish4, GrowFish5, GrowFish6,
    Money4, Money5, Money6,
    Time4, Time5, Time6,
    Repop4, Repop5, Repop6,
    Pier3, Pier4
}

public class SkillSystem : MonoBehaviour
{
    public static SkillSystem Instance;

    [SerializeField] private int skillCount;

    private bool[] skills;
    public int SkillPoint => GManager.instance.totalMoney;
    public int SkillCount => skillCount;

    public Text skillText;

    void Awake()
    {
        PlayerPrefs.DeleteAll();
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        skills = new bool[System.Enum.GetValues(typeof(SkillType)).Length];

        // ロード
        for (int i = 0; i < skills.Length; i++)
        {
            skills[i] = PlayerPrefs.GetInt(((SkillType)i).ToString(), 0) == 1;
        }

        
        skillCount = PlayerPrefs.GetInt("SkillCount", 0);

        for (int i = 0; i < skills.Length; i++)
        {
            if (skills[i])
            {
                ApplySkillEffect((SkillType)i);
            }
        }
        SetText();
    }

    public void LearnSkill(SkillType type, int point, int count)
    {
        skills[(int)type] = true;

        PlayerPrefs.SetInt(type.ToString(), 1);
        GManager.instance.totalMoney -= point;
        PlayerPrefs.SetInt(
           "TotalMoney",
           GManager.instance.totalMoney);


       skillCount++;
        PlayerPrefs.SetInt("SkillCount", skillCount);

        PlayerPrefs.Save();
        ApplySkillEffect(type);
        SetText();
    }

    public bool IsSkill(SkillType type)
    {
        return skills[(int)type];
    }

    public bool CanLearnSkill(SkillType type, int spendPoint = 0, int spendCount = 0)
    {
        if (GManager.instance.totalMoney < spendPoint) return false;
        if (skillCount < spendCount) return false;

        // ここはそのままでOK（条件ツリー）
        if (type == SkillType.Hook2)
        {
            return skills[(int)SkillType.Hook1];
        }
        else if (type == SkillType.Hook3)
        {
            return skills[(int)SkillType.Hook2];
        }
        else if (type == SkillType.Hook4)
        {
            return skills[(int)SkillType.Pier2] && skills[(int)SkillType.Hook3];
        }
        else if (type == SkillType.Hook5)
        {
            return skills[(int)SkillType.Hook4];
        }
        else if (type == SkillType.Hook6)
        {
            return skills[(int)SkillType.Hook5];
        }
        else if (type == SkillType.GrowFish2)
        {
            return skills[(int)SkillType.GrowFish1];
        }
        else if (type == SkillType.GrowFish3)
        {
            return skills[(int)SkillType.GrowFish2];
        }
        else if (type == SkillType.GrowFish4)
        {
            return skills[(int)SkillType.Pier2] && skills[(int)SkillType.GrowFish3];
        }
        else if (type == SkillType.GrowFish5)
        {
            return skills[(int)SkillType.GrowFish4];
        }
        else if (type == SkillType.GrowFish6)
        {
            return skills[(int)SkillType.GrowFish5];
        }
        else if (type == SkillType.Money2)
        {
            return skills[(int)SkillType.Money1];
        }
        else if (type == SkillType.Money3)
        {
            return skills[(int)SkillType.Money2];
        }
        else if (type == SkillType.Money4)
        {
            return skills[(int)SkillType.Pier2] && skills[(int)SkillType.Money3];
        }
        else if (type == SkillType.Money5)
        {
            return skills[(int)SkillType.Money4];
        }
        else if (type == SkillType.Money6)
        {
            return skills[(int)SkillType.Money5];
        }
        else if (type == SkillType.Time1)
        {
            return skills[(int)SkillType.Pier1];
        }
        else if (type == SkillType.Time2)
        {
            return skills[(int)SkillType.Time1];
        }
        else if (type == SkillType.Time3)
        {
            return skills[(int)SkillType.Time2];
        }
        else if (type == SkillType.Time4) 
        {
            return skills[(int)SkillType.Pier3] && skills[(int)SkillType.Time3];
        }
        else if (type == SkillType.Time5)
        {
            return skills[(int)SkillType.Time4];
        }
        else if (type == SkillType.Time6)
        {
            return skills[(int)SkillType.Time5];
        }
        else if (type == SkillType.Repop1)
        {
            return skills[(int)SkillType.Pier1];
        }
        else if (type == SkillType.Repop2)
        {
            return skills[(int)SkillType.Repop1];
        }
        else if (type == SkillType.Repop3)
        {
            return skills[(int)SkillType.Repop2];
        }
        else if (type == SkillType.Repop4)
        {
            return skills[(int)SkillType.Pier3] && skills[(int)SkillType.Repop3];
        }
        else if (type == SkillType.Repop5)
        {
            return skills[(int)SkillType.Repop4];
        }
        else if (type == SkillType.Repop6)
        {
            return skills[(int)SkillType.Repop5];
        }
        else if (type == SkillType.Pier1)
        {
            return skills[(int)SkillType.Hook3] || skills[(int)SkillType.GrowFish3] || skills[(int)SkillType.Money3];
        }
        if (type == SkillType.Pier2)
        {
            return skills[(int)SkillType.Time3] || skills[(int)SkillType.Repop3];
        }
        if (type == SkillType.Pier3)
        {
            return skills[(int)SkillType.Hook6] || skills[(int)SkillType.GrowFish6] || skills[(int)SkillType.Money6];
        }
        else if (type == SkillType.Pier4)
        {
            return skills[(int)SkillType.Time6] || skills[(int)SkillType.Repop6];
        }

           

        return true;
    }

    public void SetText()
    {
        if (skillText != null)
            skillText.text = "スキルポイント：" + GManager.instance.totalMoney;
    }
    private void ApplySkillEffect(SkillType type)
    {
        Debug.Log("スキル効果実行：" + type);
        switch (type)
        {
            case SkillType.Hook1:
                GManager.instance.detectRange += 1f;
                GManager.instance.detectRangeLevel += 1;
                break;
            case SkillType.Hook2:
                GManager.instance.detectRange += 2f;
                GManager.instance.detectRangeLevel += 1;
                break;
            case SkillType.Hook3:
                GManager.instance.detectRange += 2f;
                GManager.instance.detectRangeLevel += 1;
                break;
            case SkillType.Hook4:
                GManager.instance.detectRange += 2f;
                GManager.instance.detectRangeLevel += 1;
                break;
            case SkillType.Hook5:
                GManager.instance.detectRange += 2f;
                GManager.instance.detectRangeLevel += 1;
                break;
            case SkillType.Hook6:
                GManager.instance.detectRange += 2f;
                GManager.instance.detectRangeLevel += 1;
                break;

            case SkillType.GrowFish1:
                GManager.instance.spawnCount += 5;
                GManager.instance.spawnCountLevel += 1;
                break;
            case SkillType.GrowFish2:
                GManager.instance.spawnCount += 5;
                GManager.instance.spawnCountLevel += 1;
                break;
            case SkillType.GrowFish3:
                GManager.instance.spawnCount += 5;
                GManager.instance.spawnCountLevel += 1;
                break;
            case SkillType.GrowFish4:
                GManager.instance.spawnCount += 5;
                GManager.instance.spawnCountLevel += 1;
                break;
            case SkillType.GrowFish5:
                GManager.instance.spawnCount += 5;
                GManager.instance.spawnCountLevel += 1;
                break;
                case SkillType.GrowFish6:
                GManager.instance.spawnCount += 5;
                GManager.instance.spawnCountLevel += 1;
                break;

            case SkillType.Time1:
                GManager.instance.gameTimeLimit += 5;
                GManager.instance.gameTimeLevel += 1;
                break;

            case SkillType.Time2:
                GManager.instance.gameTimeLimit += 5;
                GManager.instance.gameTimeLevel += 1;
                break;
            case SkillType.Time3:
                GManager.instance.gameTimeLimit += 5;
                GManager.instance.gameTimeLevel += 1;
                break;
            case SkillType.Time4:
                GManager.instance.gameTimeLimit += 5;
                GManager.instance.gameTimeLevel += 1;
                break;
            case SkillType.Time5:
                GManager.instance.gameTimeLimit += 5;
                GManager.instance.gameTimeLevel += 1;
                break;
            case SkillType.Time6:
                GManager.instance.gameTimeLimit += 5;
                GManager.instance.gameTimeLevel += 1;
                break;

            case SkillType.Pier1:
                GManager.instance.pierLevel += 1;
                break;
            case SkillType.Pier2:
                GManager.instance.pierLevel += 1;
                break;
            case SkillType.Pier3:
                GManager.instance.pierLevel += 1;
                break;
                case SkillType.Pier4:
                GManager.instance.pierLevel += 1;
                break;
            case SkillType.Money1:
                
                break;

        }
    }
}