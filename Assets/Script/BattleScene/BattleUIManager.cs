using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using System;
using Unity.VisualScripting;
using UnityEngine.EventSystems;
using System.Numerics;
using UnityEditor.ShaderKeywordFilter;

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
    public GameObject backButton;          //  設定Cancelボタン


    [Header("選択コマンド")]
    public Button AttackButton;
    public Button guardButton;
    public Button CoverButton;
    public Button skillButton;

    private BattleSystemManager battleManager;

    class LogText
    {
        public string text;
        public int frame;   
    }

    private List<LogText> logTextList = new List<LogText>();
    private System.Text.StringBuilder sb = new System.Text.StringBuilder();

    void Start()
    {
        battleManager = FindObjectOfType<BattleSystemManager>();
        fightButton.onClick.AddListener(battleManager.OnFightButton);
        runButton.onClick.AddListener(battleManager.OnRunButton);
        backButton.GetComponent<Button>().onClick.AddListener(()
            => battleManager.OnBackButton());

        HideAllMenus();
    }

    private void Update()
    {
        logUpdate();
    }

    //  全メニュー非表示へ
    public void HideAllMenus()
    {
        rootMenuPanel.SetActive(false);
        actionMenuPanel.SetActive(false);
        skillmenuPanel.SetActive(false);
        targetMenuPanel.SetActive(false);
        resultPanel.SetActive(false);

        backButton.SetActive(false);

    }

    //  メイン切り替えウィンドウ
    public void ShowRootMenu()
    {
        HideAllMenus();
        rootMenuPanel.SetActive(true);

        backButton.SetActive(false);
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

        backButton.SetActive(true);

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

        backButton.SetActive(true);
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

        backButton.SetActive(true);
    }

    //  logのテキスト変更
    public void ShowLog(string text)
    {
        // logText.text = text;
        LogText log = new LogText();
        log.text = text;
        log.frame = 1000;
        logTextList.Add(log);
    }

    private void logUpdate()
    {
        foreach(var log in logTextList) {
            log.frame--;
        }
        logTextList.RemoveAll(log => log.frame <= 0);

        logText.text = "";
        sb.Clear();

        for(int i = 0; i < logTextList.Count; i++)
        {
//            var log = logTextList[i];
//            logText.text += log.text + "\n";
            sb.AppendLine(logTextList[i].text);
        }

       // logText.text = sb.ToString();
        logText.SetText(sb.ToString());
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
        SoundManager.Instance.PlaySE("SE_Confirm");
        FadeManager.FadeChangeScene("FieldScene",1.0f);
    }
}


