using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//　スキルのタイプ
public enum PrestigeType
{
   Prestige1,
   Prestige2,
   Prestige3,
   Prestige4
};

public class PrestigeSystem : MonoBehaviour
{
    //　スキルを覚える為のスキルポイント
    [SerializeField] private int PrestigePoint;
    //　スキルを覚えているかどうかのフラグ
    [SerializeField] private bool[] Prestigeskills;
    //　スキル毎のパラメータ
    [SerializeField] private PrestigeParam[] PrestigeParams;
    //　スキルポイントを表示するテキストUI
    public Text PrestigeText;

    void Awake()
    {
        //　スキル数分の配列を確保
        Prestigeskills = new bool[PrestigeParams.Length];
        SetText();
    }
    //　スキルを覚える
    public void LearnSkill(PrestigeType type, int point)
    {
        Prestigeskills[(int)type] = true;
        SetSkillPoint(point);
        SetText();
        CheckOnOff();
    }
    //　スキルを覚えているかどうかのチェック
    public bool IsSkill(PrestigeType type)
    {
        return Prestigeskills[(int)type];
    }
    //　スキルポイントを減らす
    public void SetSkillPoint(int point)
    {
        PrestigePoint -= point;
    }
    //　スキルポイントを取得
    public int GetSkillPoint()
    {
        return PrestigePoint;
    }
    //　スキルを覚えられるかチェック
    public bool CanLearnSkill(PrestigeType type, int spendPoint = 0)
    {
        //　持っているスキルポイントが足りない
        if (PrestigePoint < spendPoint)
        {
            return false;
        }
        //　攻撃UP2は攻撃UP1を覚えていなければダメ
        if (type == PrestigeType.Prestige2)
        {
            return Prestigeskills[(int)PrestigeType.Prestige1];
            //　防御UP2は防御UP1を覚えていなければダメ
        }
        else if (type == PrestigeType.Prestige3)
        {
            return Prestigeskills[(int)PrestigeType.Prestige2];
            //　速さUP2は速さUP1を覚えていなければダメ
        }
        else if (type == PrestigeType.Prestige4)
        {
            return Prestigeskills[(int)PrestigeType.Prestige3];
            //　コンボは攻撃UP2と防御２を覚えていなければダメ
        }
      
        return true;
    }
    //　スキル毎にボタンのオン・オフをする処理を実行させる
    void CheckOnOff()
    {
        foreach (var PrestigeParam in PrestigeParams)
        {
            PrestigeParam.CheckButtonOnOff();
        }
    }

    void SetText()
    {
        PrestigeText.text = "スキルポイント：" + PrestigePoint;
    }
}
