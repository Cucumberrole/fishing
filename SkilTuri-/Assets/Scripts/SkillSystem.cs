using UnityEngine;
using UnityEngine.UI;

public enum SkillType
{
    Hook1, Hook2, Hook3, Hook4, Hook5, Hook6, Hook7, Hook8, Hook9, Hook10, Hook11, Hook12, Hook13, Hook14, Hook15, Hook16, Hook17, Hook18,
    GrowFish1, GrowFish2, GrowFish3, GrowFish4, GrowFish5, GrowFish6, GrowFish7, GrowFish8, GrowFish9, GrowFish10, GrowFish11, GrowFish12, GrowFish13, GrowFish14, GrowFish15, GrowFish16, GrowFish17, GrowFish18,
    Money1, Money2, Money3, Money4, Money5, Money6, Money7, Money8, Money9, Money10, Money11, Money12, Money13, Money14, Money15, Money16, Money17, Money18,
    Time1, Time2, Time3, Time4, Time5, Time6, Time7, Time8, Time9, Time10, Time11, Time12, Time13, Time14, Time15, Time16, Time17, Time18,
    Repop1, Repop2, Repop3, Repop4, Repop5, Repop6, Repop7, Repop8, Repop9, Repop10, Repop11, Repop12, Repop13, Repop14, Repop15, Repop16, Repop17, Repop18,
    Pier1, Pier2, Pier3, Pier4, Pier5, Pier6, Pier7, Pier8, Pier9, Pier10, Pier11, Pier12
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
        else if (type == SkillType.Hook7)
        {
            return skills[(int)SkillType.Hook6] && skills[(int)SkillType.Pier4];
        }
        else if (type == SkillType.Hook8)
        {
            return skills[(int)SkillType.Hook7];
        }
        else if (type == SkillType.Hook9)
        {
            return skills[(int)SkillType.Hook8];
        }
        else if (type == SkillType.Hook10)
        {
            return skills[(int)SkillType.Hook9] && skills[(int)SkillType.Pier6];
        }
        else if (type == SkillType.Hook11)
        {
            return skills[(int)SkillType.Hook10];
        }
        else if (type == SkillType.Hook12)
        {
            return skills[(int)SkillType.Hook11];
        }
        else if (type == SkillType.Hook13)
        {
            return skills[(int)SkillType.Hook12] && skills[(int)SkillType.Pier8];
        }
        else if (type == SkillType.Hook14)
        {
            return skills[(int)SkillType.Hook13];
        }
        else if (type == SkillType.Hook15)
        {
            return skills[(int)SkillType.Hook14];
        }
        else if (type == SkillType.Hook16)
        {
            return skills[(int)SkillType.Hook15] && skills[(int)SkillType.Pier10];
        }
        else if (type == SkillType.Hook17)
        {
            return skills[(int)SkillType.Hook16];
        }
        else if (type == SkillType.Hook18)
        {
            return skills[(int)SkillType.Hook17];
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
        else if (type == SkillType.GrowFish7)
        {
            return skills[(int)SkillType.GrowFish6] && skills[(int)SkillType.Pier4];
        }
        else if (type == SkillType.GrowFish8)
        {
            return skills[(int)SkillType.GrowFish7];
        }
        else if (type == SkillType.GrowFish9)
        {
            return skills[(int)SkillType.GrowFish8];
        }
        else if (type == SkillType.GrowFish10)
        {
            return skills[(int)SkillType.GrowFish9] && skills[(int)SkillType.Pier6];
        }
        else if (type == SkillType.GrowFish11)
        {
            return skills[(int)SkillType.GrowFish10];
        }
        else if (type == SkillType.GrowFish12)
        {
            return skills[(int)SkillType.GrowFish11];
        }
        else if (type == SkillType.GrowFish13)
        {
            return skills[(int)SkillType.GrowFish12] && skills[(int)SkillType.Pier8];
        }
        else if (type == SkillType.GrowFish14)
        {
            return skills[(int)SkillType.GrowFish13];
        }
        else if (type == SkillType.GrowFish15)
        {
            return skills[(int)SkillType.GrowFish14];
        }
        else if (type == SkillType.GrowFish16)
        {
            return skills[(int)SkillType.GrowFish15] && skills[(int)SkillType.Pier10];
        }
        else if (type == SkillType.GrowFish17)
        {
            return skills[(int)SkillType.GrowFish16];
        }
        else if (type == SkillType.GrowFish18)
        {
            return skills[(int)SkillType.GrowFish17];
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
        else if (type == SkillType.Money7)
        {
            return skills[(int)SkillType.Money6] && skills[(int)SkillType.Pier4];
        }
        else if (type == SkillType.Money8)
        {
            return skills[(int)SkillType.Money7];
        }
        else if (type == SkillType.Money9)
        {
            return skills[(int)SkillType.Money8];
        }
        else if (type == SkillType.Money10)
        {
            return skills[(int)SkillType.Money9] && skills[(int)SkillType.Pier6];
        }
        else if (type == SkillType.Money11)
        {
            return skills[(int)SkillType.Money10];
        }
        else if (type == SkillType.Money12)
        {
            return skills[(int)SkillType.Money11];
        }
        else if (type == SkillType.Money13)
        {
            return skills[(int)SkillType.Money12] && skills[(int)SkillType.Pier8];
        }
        else if (type == SkillType.Money14)
        {
            return skills[(int)SkillType.Money13];
        }
        else if (type == SkillType.Money15)
        {
            return skills[(int)SkillType.Money14];
        }
        else if (type == SkillType.Money16)
        {
            return skills[(int)SkillType.Money15] && skills[(int)SkillType.Pier10];
        }
        else if (type == SkillType.Money17)
        {
            return skills[(int)SkillType.Money16];
        }
        else if (type == SkillType.Money18)
        {
            return skills[(int)SkillType.Money17];
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
        else if (type == SkillType.Time7)
        {
            return skills[(int)SkillType.Pier5] && skills[(int)SkillType.Time6];
        }
        else if (type == SkillType.Time8)
        {
            return skills[(int)SkillType.Time7];
        }
        else if (type == SkillType.Time9)
        {
            return skills[(int)SkillType.Time8];
        }
        else if (type == SkillType.Time10)
        {
            return skills[(int)SkillType.Pier5] && skills[(int)SkillType.Time9];
        }
        else if (type == SkillType.Time11)
        {
            return skills[(int)SkillType.Time10];
        }
        else if (type == SkillType.Time12)
        {
            return skills[(int)SkillType.Time11];
        }
        else if (type == SkillType.Time13)
        {
            return skills[(int)SkillType.Pier7] && skills[(int)SkillType.Time12];
        }
        else if (type == SkillType.Time14)
        {
            return skills[(int)SkillType.Time13];
        }
        else if (type == SkillType.Time15)
        {
            return skills[(int)SkillType.Time14];
        }
        else if (type == SkillType.Time16)
        {
            return skills[(int)SkillType.Pier9] && skills[(int)SkillType.Time15];
        }
        else if (type == SkillType.Time17)
        {
            return skills[(int)SkillType.Time16];
        }
        else if (type == SkillType.Time18)
        {
            return skills[(int)SkillType.Time17];
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
        else if (type == SkillType.Repop7)
        {
            return skills[(int)SkillType.Pier5] && skills[(int)SkillType.Repop6];
        }
        else if (type == SkillType.Repop8)
        {
            return skills[(int)SkillType.Repop7];
        }
        else if (type == SkillType.Repop9)
        {
            return skills[(int)SkillType.Repop8];
        }
        else if (type == SkillType.Repop10)
        {
            return skills[(int)SkillType.Pier7] && skills[(int)SkillType.Repop9];
        }
        else if (type == SkillType.Repop11)
        {
            return skills[(int)SkillType.Repop10];
        }
        else if (type == SkillType.Repop12)
        {
            return skills[(int)SkillType.Repop11];
        }
        else if (type == SkillType.Repop13)
        {
            return skills[(int)SkillType.Pier9] && skills[(int)SkillType.Repop12];
        }
        else if (type == SkillType.Repop14)
        {
            return skills[(int)SkillType.Repop13];
        }
        else if (type == SkillType.Repop15)
        {
            return skills[(int)SkillType.Repop14];
        }
        else if (type == SkillType.Repop16)
        {
            return skills[(int)SkillType.Pier11] && skills[(int)SkillType.Repop15];
        }
        else if (type == SkillType.Repop17)
        {
            return skills[(int)SkillType.Repop16];
        }
        else if (type == SkillType.Repop18)
        {
            return skills[(int)SkillType.Repop17];
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
        else if (type == SkillType.Pier5)
        {
            return skills[(int)SkillType.Hook9] || skills[(int)SkillType.GrowFish9] || skills[(int)SkillType.Money9];
        }
        else if (type == SkillType.Pier6)
        {
            return skills[(int)SkillType.Time9] || skills[((int)SkillType.Repop9)];
        }
        else if (type == SkillType.Pier7)
        {
            return skills[(int)SkillType.Hook12] || skills[(int)SkillType.GrowFish12] || skills[(int)SkillType.Money12];
        }
        else if (type == SkillType.Pier8)
        {
            return skills[(int)SkillType.Time12] || skills[(int)SkillType.Repop12];
        }
        else if (type == SkillType.Pier9)
        {
            return skills[(int)SkillType.Hook15] || skills[(int)SkillType.GrowFish15] || skills[(int)SkillType.Money15];
        }
        else if (type == SkillType.Pier10)
        {
            return skills[(int)SkillType.Time15] || skills[((int)SkillType.Repop15)];
        }
        else if (type == SkillType.Pier11)
        {
            return skills[(int)SkillType.Hook18] || skills[(int)SkillType.GrowFish18] || skills[(int)SkillType.Money18];
        }
        else if (type == SkillType.Pier12)
        {
            return skills[(int)SkillType.Time18] || skills[(int)SkillType.Repop18];
        }



        return true;
    }

    public void SetText()
    {
        if (skillText != null)
            skillText.text = "消費金額：" + GManager.instance.totalMoney;
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
            case SkillType.Hook7:
                GManager.instance.detectRange += 2f;
                break;
            case SkillType.Hook8:
                GManager.instance.detectRange += 2f;
                break;
            case SkillType.Hook9:
                GManager.instance.detectRange += 2f;
                break;
            case SkillType.Hook10:
                GManager.instance.detectRange += 2f;
                break;
            case SkillType.Hook11:
                GManager.instance.detectRange += 2f;
                break;
            case SkillType.Hook12:
                GManager.instance.detectRange += 2f;
                break;
            case SkillType.Hook13:
                GManager.instance.detectRange += 2f;
                break;
            case SkillType.Hook14:
                GManager.instance.detectRange += 2f;
                break;
            case SkillType.Hook15:
                GManager.instance.detectRange += 2f;
                break;
            case SkillType.Hook16:
                GManager.instance.detectRange += 2f;
                break;
            case SkillType.Hook17:
                GManager.instance.detectRange += 2f;
                break;
            case SkillType.Hook18:
                GManager.instance.detectRange += 2f;
                break;
                GManager.instance.detectRange += 2f;
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
            case SkillType.GrowFish7:
                GManager.instance.spawnCount += 5;
                GManager.instance.spawnCountLevel += 1;
                break;
            case SkillType.GrowFish8:
                GManager.instance.spawnCount += 5;
                GManager.instance.spawnCountLevel += 1;
                break;
            case SkillType.GrowFish9:
                GManager.instance.spawnCount += 5;
                GManager.instance.spawnCountLevel += 1;
                break;
            case SkillType.GrowFish10:
                GManager.instance.spawnCount += 5;
                GManager.instance.spawnCountLevel += 1;
                break;
            case SkillType.GrowFish11:
                GManager.instance.spawnCount += 5;
                GManager.instance.spawnCountLevel += 1;
                break;
            case SkillType.GrowFish12:
                GManager.instance.spawnCount += 5;
                GManager.instance.spawnCountLevel += 1;
                break;
            case SkillType.GrowFish13:
                GManager.instance.spawnCount += 5;
                GManager.instance.spawnCountLevel += 1;
                break;
            case SkillType.GrowFish14:
                GManager.instance.spawnCount += 5;
                GManager.instance.spawnCountLevel += 1;
                break;
            case SkillType.GrowFish15:
                GManager.instance.spawnCount += 5;
                GManager.instance.spawnCountLevel += 1;
                break;
            case SkillType.GrowFish16:
                GManager.instance.spawnCount += 5;
                GManager.instance.spawnCountLevel += 1;
                break;
            case SkillType.GrowFish17:
                GManager.instance.spawnCount += 5;
                GManager.instance.spawnCountLevel += 1;
                break;
            case SkillType.GrowFish18:
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
            case SkillType.Time7:
                GManager.instance.gameTimeLimit += 5;
                GManager.instance.gameTimeLevel += 1;
                break;
            case SkillType.Time8:
                GManager.instance.gameTimeLimit += 5;
                GManager.instance.gameTimeLevel += 1;
                break;
            case SkillType.Time9:
                GManager.instance.gameTimeLimit += 5;
                GManager.instance.gameTimeLevel += 1;
                break;
            case SkillType.Time10:
                GManager.instance.gameTimeLimit += 5;
                GManager.instance.gameTimeLevel += 1;
                break;
            case SkillType.Time11:
                GManager.instance.gameTimeLimit += 5;
                GManager.instance.gameTimeLevel += 1;
                break;
            case SkillType.Time12:
                GManager.instance.gameTimeLimit += 5;
                GManager.instance.gameTimeLevel += 1;
                break;
            case SkillType.Time13:
                GManager.instance.gameTimeLimit += 5;
                GManager.instance.gameTimeLevel += 1;
                break;
            case SkillType.Time14:
                GManager.instance.gameTimeLimit += 5;
                GManager.instance.gameTimeLevel += 1;
                break;
            case SkillType.Time15:
                GManager.instance.gameTimeLimit += 5;
                GManager.instance.gameTimeLevel += 1;
                break;
            case SkillType.Time16:
                GManager.instance.gameTimeLimit += 5;
                GManager.instance.gameTimeLevel += 1;
                break;
            case SkillType.Time17:
                GManager.instance.gameTimeLimit += 5;
                GManager.instance.gameTimeLevel += 1;
                break;
            case SkillType.Time18:
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
            case SkillType.Pier5:
                GManager.instance.pierLevel += 1;
                break;
            case SkillType.Pier6:
                GManager.instance.pierLevel += 1;
                break;
            case SkillType.Pier7:
                GManager.instance.pierLevel += 1;
                break;
            case SkillType.Pier8:
                GManager.instance.pierLevel += 1;
                break;
            case SkillType.Pier9:
                GManager.instance.pierLevel += 1;
                break;
            case SkillType.Pier10:
                GManager.instance.pierLevel += 1;
                break;
            case SkillType.Pier11:
                GManager.instance.pierLevel += 1;
                break;
            case SkillType.Pier12:
                GManager.instance.pierLevel += 1;
                break;

            case SkillType.Money1:
                GManager.instance.moneyMultiplier += 1;
                break;
            case SkillType.Money2:
                GManager.instance.moneyMultiplier += 1;
                break;
            case SkillType.Money3:
                GManager.instance.moneyMultiplier += 1;
                break;
            case SkillType.Money4:
                GManager.instance.moneyMultiplier += 1;
                break;
            case SkillType.Money5:
                GManager.instance.moneyMultiplier += 1;
                break;
            case SkillType.Money6:
                GManager.instance.moneyMultiplier += 1;
                break;
            case SkillType.Money7:
                GManager.instance.moneyMultiplier += 1;
                break;
            case SkillType.Money8:
                GManager.instance.moneyMultiplier += 1;
                break;
            case SkillType.Money9:
                GManager.instance.moneyMultiplier += 1;
                break;
            case SkillType.Money10:
                GManager.instance.moneyMultiplier += 1;
                break;
            case SkillType.Money11:
                GManager.instance.moneyMultiplier += 1;
                break;
            case SkillType.Money12:
                GManager.instance.moneyMultiplier += 1;
                break;
            case SkillType.Money13:
                GManager.instance.moneyMultiplier += 1;
                break;
            case SkillType.Money14:
                GManager.instance.moneyMultiplier += 1;
                break;
            case SkillType.Money15:
                GManager.instance.moneyMultiplier += 1;
                break;
            case SkillType.Money16:
                GManager.instance.moneyMultiplier += 1;
                break;
            case SkillType.Money17:
                GManager.instance.moneyMultiplier += 1;
                break;
            case SkillType.Money18:
                GManager.instance.moneyMultiplier += 1;
                break;
            case SkillType.Repop1:
                GManager.instance.fishRespawnTime -= 0.5f;
                break;
            case SkillType.Repop2:
                GManager.instance.fishRespawnTime -= 0.5f;
                break;
            case SkillType.Repop3:
                GManager.instance.fishRespawnTime -= 0.5f;
                break;
            case SkillType.Repop4:
                GManager.instance.fishRespawnTime -= 0.5f;
                break;
            case SkillType.Repop5:
                GManager.instance.fishRespawnTime -= 0.5f;
                break;
            case SkillType.Repop6:
                GManager.instance.fishRespawnTime -= 0.05f;
                break;
            case SkillType.Repop7:
                GManager.instance.fishRespawnTime -= 0.05f;
                break;
            case SkillType.Repop8:
                GManager.instance.fishRespawnTime -= 0.05f;
                break;
            case SkillType.Repop9:
                GManager.instance.fishRespawnTime -= 0.05f;
                break;
            case SkillType.Repop10:
                GManager.instance.fishRespawnTime -= 0.05f;
                break;
            case SkillType.Repop11:
                GManager.instance.fishRespawnTime -= 0.05f;
                break;
            case SkillType.Repop12:
                GManager.instance.fishRespawnTime -= 0.05f;
                break;
            case SkillType.Repop13:
                GManager.instance.fishRespawnTime -= 0.05f;
                break;
            case SkillType.Repop14:
                GManager.instance.fishRespawnTime -= 0.05f;
                break;
            case SkillType.Repop15:
                GManager.instance.fishRespawnTime -= 0.05f;
                break;
            case SkillType.Repop16:
                GManager.instance.fishRespawnTime -= 0.05f;
                break;
            case SkillType.Repop17:
                GManager.instance.fishRespawnTime -= 0.05f;
                break;
            case SkillType.Repop18:
                GManager.instance.fishRespawnTime -= 0.05f;
                break;

        }
    }
}