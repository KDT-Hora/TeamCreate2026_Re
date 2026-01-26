using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using System;
using Unity.VisualScripting;

public class BattleUIManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject rootMenuPanel;    // 「戦う」「逃げる」
    public GameObject actionMenuPanel;  // コマンド一覧
    public GameObject skillmenuPanel;   // スキル一覧
    public GameObject targetMenuPanel;  // 対象一覧
    public GameObject resultPanel;      //  リザルト

    [Header("UI")]
    public GameObject PlayerUIBox;       //  プレイヤーUI群 
    public GameObject EnemyUIBox;        //  敵UI群
    public GameObject PlayerUI;     //  プレイヤーUI
    public GameObject EnemyUI;      //  敵UI
   

    [Header("Buttons & Text")]
    public Button fightButton;
    public Button runButton;
    public Transform actionButtonContainer; // Grid layoutなどを想定
    public Transform avirityButtonContainer;
    public Transform targetButtonContainer;
    public GameObject buttonPrefab;         // テキスト付きボタンのプレハブ
    public TextMeshProUGUI logText;
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI phaseText;
    public Button returnButton;

    [Header("選択コマンド")]
    public Button AttackButton;
    public Button guardButton;
    public Button CoverButton;
    public Button skillButton;

    private BattleSystemManager battleManager;

    void Start()
    {
        battleManager = FindObjectOfType<BattleSystemManager>();
        fightButton.onClick.AddListener(battleManager.OnFightButton);
        runButton.onClick.AddListener(battleManager.OnRunButton);
        HideAllMenus();
    }

    //  全メニュー非表示へ
    public void HideAllMenus()
    {
        rootMenuPanel.SetActive(false);
        actionMenuPanel.SetActive(false);
        skillmenuPanel.SetActive(false);
        targetMenuPanel.SetActive(false);
        resultPanel.SetActive(false);
    }

    //  メイン切り替えウィンドウ
    public void ShowRootMenu()
    {
        HideAllMenus();
        rootMenuPanel.SetActive(true);
    }

    // コマンドボタン生成
    public void ShowActionMenu(UnitController unit, bool isCoverUsed)
    {
        Debug.Log("ShowActionMenu");

        HideAllMenus();
        actionMenuPanel.SetActive(true);
        phaseText.text = $"{unit.GetUnitName()} の行動";

        // 既存ボタン削除
        foreach (Transform child in actionButtonContainer) Destroy(child.gameObject);

        // 基本コマンド
        List<SkillData> skillsToShow = new List<SkillData>(battleManager.commonSkills);

        //  通常攻撃ボタン
        AttackButton.gameObject.SetActive(true);
        AttackButton.onClick.RemoveAllListeners();
        AttackButton.onClick.AddListener(() 
            => battleManager.OnSkillSelected(skillsToShow[0]));
        //  防御ボタン
        guardButton.gameObject.SetActive(true);
        guardButton.onClick.RemoveAllListeners();
        guardButton.onClick.AddListener(() 
            => battleManager.OnSkillSelected(skillsToShow[1]));
        //  庇うボタン
        // 庇う制限
        if(isCoverUsed)
        {
            CoverButton.gameObject.SetActive(false);
        }
        else
        { 
            CoverButton.gameObject.SetActive(true);
            CoverButton.onClick.RemoveAllListeners();
            CoverButton.onClick.AddListener(() 
            => battleManager.OnSkillSelected(skillsToShow[2]));
        }
        //  スキルボタン
        skillButton.gameObject.SetActive(true);
        skillButton.onClick.RemoveAllListeners();
        skillButton.onClick.AddListener(() 
            => battleManager.OnAviritySkillSelected());

        //  ふるい処理
     //   foreach (var skill in skillsToShow)
     //   {
     //       // 庇う制限
     //       if (skill.type == ActionType.Cover && isCoverUsed) continue;
     //
     //       GameObject btnObj = Instantiate(buttonPrefab, actionButtonContainer);
     //       btnObj.GetComponentInChildren<TextMeshProUGUI>().text = skill.skillName;
     //       btnObj.GetComponent<Button>().onClick.AddListener(() 
     //           => battleManager.OnSkillSelected(skill));
     //   }
    }

    //  スキル選択
    public void ShowSkillMenu(UnitController unit)
    {
        Debug.Log("ShowSkillMenu");

        HideAllMenus();
        skillmenuPanel.SetActive(true);

        //  キャラクター固有のスキルを一覧表示
        List<SkillData> skillsToShow = new List<SkillData>();
        var uniqueSkill = unit.skills;
        uniqueSkill.skillDatas.ForEach(skill => skillsToShow.Add(skill));

        Debug.Log($"スキル数: {skillsToShow.Count}");

        // 既存ボタン削除
        foreach (Transform child in avirityButtonContainer) Destroy(child.gameObject);
        // スキルボタン生成
        foreach (var skill in skillsToShow)
        {
            Debug.Log($"スキル名: {skill.skillName}");
            GameObject btnObj = Instantiate(buttonPrefab, avirityButtonContainer);
            btnObj.GetComponentInChildren<TextMeshProUGUI>().text = skill.skillName;
            btnObj.GetComponent<Button>().onClick.AddListener(() 
                => battleManager.OnSkillSelected(skill));
        }


    }

    // ターゲットボタン生成
    public void ShowTargetMenu(List<UnitController> targets, 
        System.Action<UnitController> onSelect)
    {
        Debug.Log("ShowTargetMenu");

        HideAllMenus();
        targetMenuPanel.SetActive(true);

        foreach (Transform child in targetButtonContainer) Destroy(child.gameObject); // ターゲット用パネルのコンテナを使用するか、共通化するかは設計次第。ここでは共通コンテナの例。

        foreach (var target in targets)
        {
            if (target.isDead) continue;

            GameObject btnObj = Instantiate(buttonPrefab, targetButtonContainer);
            btnObj.GetComponentInChildren<TextMeshProUGUI>().text = target.GetUnitName();
            btnObj.GetComponent<Button>().onClick.AddListener(() => onSelect(target));
        }
    }

    //  logのテキスト変更
    public void ShowLog(string text)
    {
        logText.text = text;
        // 数秒後に消すなどの処理を入れても良い
    }

    public void ShowPhaseText(string text)
    {
        if (phaseText) phaseText.text = text;
    }

    public void ShowResult(string message, bool isWin)
    {
        HideAllMenus();
        resultPanel.SetActive(true);
        resultText.text = message;
        resultText.color = isWin ? Color.yellow : Color.red;
        // リザルト後のボタン表示などもここで設定可能
        returnButton.onClick.AddListener(OnReturnToFieldButton);
    }

    // フィールドシーンに戻るボタン
    public void OnReturnToFieldButton()
    {
        FadeManager.FadeChangeScene("FieldScene",1.0f);
    }
}


