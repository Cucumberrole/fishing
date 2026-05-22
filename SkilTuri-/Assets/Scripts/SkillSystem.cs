using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//　スキルのタイプ
public enum SkillType
{
    Hook1,
    Hook2,
    Rod1,
    Rod2,
    Bait1,
    Bait2,
    Pier1,
    Pier2,
    Shop1,
    Shop2,
    Master
};

public class SkillSystem : MonoBehaviour
{
    //　スキルを覚える為のスキルポイント
    [SerializeField] private int skillPoint;
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
    public void LearnSkill(SkillType type, int point)
    {
        skills[(int)type] = true;
        SetSkillPoint(point);
        SetText();
        CheckOnOff();
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
    //　スキルポイントを取得
    public int GetSkillPoint()
    {
        return skillPoint;
    }
    //　スキルを覚えられるかチェック
    public bool CanLearnSkill(SkillType type, int spendPoint = 0)
    {
        //　持っているスキルポイントが足りない
        if (skillPoint < spendPoint)
        {
            return false;
        }
        //　釣り針2は釣り針1を覚えていなければダメ
        if (type == SkillType.Hook2)
        {
            return skills[(int)SkillType.Hook1];
            //　防御UP2は防御UP1を覚えていなければダメ
        }
        else if(type == SkillType.Rod2)
        {
            return skills[(int)SkillType.Rod1];
        }
        else if (type == SkillType.Pier2)
        {
            return skills[(int)SkillType.Pier1];
            //　速さUP2は速さUP1を覚えていなければダメ
        }
        else if (type == SkillType.Bait2)
        {
            return skills[(int)SkillType.Bait1];
        }
        else if (type == SkillType.Shop2)
        {
            return skills[(int)SkillType.Shop1];
            //　コンボは攻撃UP2と防御２を覚えていなければダメ
        }

        else if (type == SkillType.Master)
        {
            return skills[(int)SkillType.Hook2] && skills[(int)SkillType.Rod2] && skills[(int)SkillType.Pier2] && skills[(int)SkillType.Bait2];
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