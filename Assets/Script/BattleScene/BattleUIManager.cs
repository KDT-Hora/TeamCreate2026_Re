using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using System;

public class BattleUIManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject rootMenuPanel;    // 「戦う」「逃げる」
    public GameObject actionMenuPanel;  // コマンド一覧
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
    public GameObject buttonPrefab;         // テキスト付きボタンのプレハブ
    public TextMeshProUGUI logText;
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI phaseText;
    public Button returnButton;

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
        HideAllMenus();
        actionMenuPanel.SetActive(true);
        phaseText.text = $"{unit.GetUnitName()} の行動";

        // 既存ボタン削除
        //    foreach (Transform child in actionButtonContainer) Destroy(child.gameObject);

        // 基本コマンド + キャラの持つスキル（今回はCommonSkills + キャラの個別スキル等を統合する想定）
        List<SkillData> skillsToShow = new List<SkillData>(battleManager.commonSkills);
        // キャラ固有スキル追加（例）
    //    skillsToShow.AddRange(unit.personalSkills);


        foreach (var skill in skillsToShow)
        {
            // 庇う制限
            if (skill.type == ActionType.Cover && isCoverUsed) continue;

            GameObject btnObj = Instantiate(buttonPrefab, actionButtonContainer);
            btnObj.GetComponentInChildren<TextMeshProUGUI>().text = skill.skillName;
            btnObj.GetComponent<Button>().onClick.AddListener(() => battleManager.OnSkillSelected(skill));
        }
    }

    // ターゲットボタン生成
    public void ShowTargetMenu(List<UnitController> targets, System.Action<UnitController> onSelect)
    {
        HideAllMenus();
        targetMenuPanel.SetActive(true);

        foreach (Transform child in actionButtonContainer) Destroy(child.gameObject); // ターゲット用パネルのコンテナを使用するか、共通化するかは設計次第。ここでは共通コンテナの例。

        // 実際はTargetMenu用のContainerを使ったほうが良いですが、簡易化のためactionButtonContainerを再利用する例とします（パネルのSetActive管理に注意）
        // ※TargetMenuPanel内にContainerがある想定で書きます
        Transform targetContainer = targetMenuPanel.transform.GetChild(0); // 仮
        foreach (Transform child in targetContainer) Destroy(child.gameObject);

        foreach (var target in targets)
        {
            if (target.isDead) continue;

            GameObject btnObj = Instantiate(buttonPrefab, targetContainer);
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


