using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//　スキルのタイプ
public enum SkillType
{
    Hook1,
    Hook2,
    Hook3,
    GrowFish1,
    GrowFish2,
    GrowFish3,
    Money1,
    Money2,
    Money3,
    Time1,
    Time2,
    Time3,
    Repop1,
    Repop2,
    Repop3,
    Pier1,
    Pier2
};

public class SkillSystem : MonoBehaviour
{
    //　スキルを覚える為のスキルポイント
    [SerializeField] private int skillPoint;
    [SerializeField] private int skillCount;
    //　スキルを覚えているかどうかのフラグ
    [SerializeField] private bool[] skills;
    //　スキル毎のパラメータ
    [SerializeField] private SkillParam[] skillParams;
    //　スキルポイントを表示するテキストUI
    public Text skillText;

    void Awake()
    {
        //　スキル数分の配列を確保
        skills = new bool[skillParams.Length];
        SetText();
    }
    //　スキルを覚える
    public void LearnSkill(SkillType type, int point,int count)
    {
        skills[(int)type] = true;
        SetSkillPoint(point);
        SetText();
        CheckOnOff();
       
        SetSkillCount();
        //skillCount;
    }
    //　スキルを覚えているかどうかのチェック
    public bool IsSkill(SkillType type)
    {
        return skills[(int)type];
    }
    //　スキルポイントを減らす
    public void SetSkillPoint(int point)
    {
        skillPoint -= point;
    }
    public void SetSkillCount()
    {
        skillCount ++;
    }
    //　スキルポイントを取得
    public int GetSkillPoint()
    {
        return skillPoint;
    }
    //　スキルを覚えられるかチェック
    public bool CanLearnSkill(SkillType type, int spendPoint = 0,int spendCount = 0)
    {
        //　持っているスキルポイントが足りない
        if (skillPoint < spendPoint)
        {
            return false;
        }
        if (skillCount < spendCount)
        {
            return false;
        }
        //　攻撃UP2は攻撃UP1を覚えていなければダメ
        if (type == SkillType.Hook2)
        {
            return skills[(int)SkillType.Hook1];
            //　防御UP2は防御UP1を覚えていなければダメ
        }
        else if (type == SkillType.Hook3)
        {
            return skills[(int)SkillType.Hook2];
        }
        else if (type == SkillType.GrowFish2)
        {
            return skills[(int)SkillType.GrowFish1];
            //　速さUP2は速さUP1を覚えていなければダメ
        }
        else if(type == SkillType.GrowFish3)
        {
            return skills[(int)SkillType.GrowFish2];
        }
        else if (type == SkillType.Money2)
        {
            return skills[(int)SkillType.Money1];
            //　コンボは攻撃UP2と防御２を覚えていなければダメ
        }
        else if(type == SkillType.Money3)
        {
            return skills[(int)SkillType.Money2];
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
        else if (type == SkillType.Pier1)
        {
            return skills[(int)SkillType.Hook3] || skills[(int)SkillType.GrowFish3] || skills[(int)SkillType.Money3];
        }
        else if (type == SkillType.Pier2)
        {
            return skills[(int)SkillType.Time3] || skills[(int)SkillType.Repop3];
            //　マスタースキルは全てのスキルを覚えていなければダメ
        }
  
        return true;
    }
    //　スキル毎にボタンのオン・オフをする処理を実行させる
    void CheckOnOff()
    {
        foreach (var skillParam in skillParams)
        {
            skillParam.CheckButtonOnOff();
        }
    }

    void SetText()
    {
        skillText.text = "スキルポイント：" + skillPoint;
    }
}