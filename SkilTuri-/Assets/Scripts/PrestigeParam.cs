
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class PrestigeParam : MonoBehaviour
{
    //　スキル管理システム
    [SerializeField]
    private PrestigeSystem PrestigeSystem;
    //　このスキルの種類
    [SerializeField]
    private PrestigeType type;
    //　このスキルを覚える為に必要なスキルポイント
    [SerializeField]
    private int spendPoint;
    //　スキルのタイトル
    [SerializeField]
    private string PrestigeTitle;
    //　スキル情報
    [SerializeField]
    private string PrestigeInformation;
    //　スキル情報を載せるテキストUI
    [SerializeField]
    private Text text;
    public Renderer[] targetRenderers;

    // Use this for initialization
    void Start()
    {
        //　スキルを覚えられる状態でなければボタンを無効化
        CheckButtonOnOff();
    }

    //　スキルボタンを押した時に実行するメソッド
    public void OnClick()
    {
        foreach (Renderer renderer in targetRenderers)
        {
            if (renderer != null)
            {
                renderer.material.color = Color.black;
            }
        }
        //　スキルを覚えていたら何もせずreturn
        if (PrestigeSystem.IsSkill(type))
        {
            return;
        }
        //　スキルを覚えられるかどうかチェック
        if (PrestigeSystem.CanLearnSkill(type, spendPoint))
        {
            //　スキルを覚えさせる
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

    //　他のスキルを習得した後の自身のボタンの処理
    public void CheckButtonOnOff()
    {
        //　スキルを覚えられるかどうかチェック
        if (!PrestigeSystem.CanLearnSkill(type))
        {
            ChangeButtonColor(new Color(0.8f, 0.8f, 0.8f, 0.8f));
            //　スキルをまだ覚えていない
        }
        else if (!PrestigeSystem.IsSkill(type))
        {
            ChangeButtonColor(new Color(1f, 1f, 1f, 1f));
        }
    }
    //　スキル情報を表示
    public void SetText()
    {
        text.text = PrestigeTitle + "：消費スキルポイント" + spendPoint + "\n" + PrestigeInformation;
    }
    //　スキル情報をリセット
    public void ResetText()
    {
        text.text = "";
    }
    //　ボタンの色を変更する
    public void ChangeButtonColor(Color color)
    {
        //　ボタンコンポーネントを取得
        Button button = gameObject.GetComponent<Button>();
        //　ボタンのカラー情報を取得（一時変数を作成し、色情報を変えてからそれをbutton.colorsに設定しないとエラーになる）
        ColorBlock cb = button.colors;
        //　取得済みのスキルボタンの色を変える
        cb.normalColor = color;
        cb.pressedColor = color;
        //　ボタンのカラー情報を設定
        button.colors = cb;
    }
}