using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil.Cil;

public enum BattleState { Start, PlayerMenu, ActionSelect, TargetSelect, EnemyPhase, ExecutePhase, Win, Lose }

public class BattleSystemManager : MonoBehaviour
{
    [Header("Units")]
    public List<UnitController> players;
    public List<UnitController> enemies;

    [Header("UI Manager")]
    public BattleUIManager uiManager;

    [Header("Enemy Unit Factory")]
    public EnemyUnitFactory enemyFactory;

    [Header("Settings")]
    public List<SkillData> commonSkills; // 攻撃、防御など

    // 内部ステート
    private BattleState state;
    private int currentPlayerIndex = 0; // 現在コマンド選択中のキャラ
    private SkillData currentSelectedSkill; // 現在選択中のスキル
    private bool isCoverSelectedThisTurn = false; // チーム全体で「庇う」が選択されたか

    // 行動リスト
    private List<BattleAction> turnActions = new List<BattleAction>();

    void Start()
    {
        //  デバッグ用の初期化
        DebugInit();


        state = BattleState.Start;
        //  キャラクターステータス設定
        CharactorStateSet();
        //  敵の生成
     //   EnemyCreate();

        //  開始処理
        StartCoroutine(SetupBattle());
    }

    void DebugInit()
    {
        //　データマネージャーの初期化
        DataManager.Instance.SetupParty(3, DataManager.Instance.PlayerPrefab);

    }

    //  キャラクターのステータス設定
    void CharactorStateSet()
    {
        var plaerDataList = DataManager.Instance.currentParty.members;
        for (int i = 0; i < players.Count; i++)
        {
            var playerConstData = plaerDataList[i].GetStatusCalculated().maxHp;
            var playerData = plaerDataList[i].GetStatusRuntime();
            players[i].UnitInit(playerConstData,playerData);

        }

    }

    //  敵の生成処理
    void EnemyCreate()
    {
        Debug.Log("EnemyCreate Start");

        //  今がボス線か判定
        if (DataManager.Instance.isBossBattle)
        {
            Debug.Log("BossUnit Create");

            DataManager.Instance.isBossBattle = false; // フラグリセット
            int id = DataManager.Instance.currentBossID;

            // ボス戦の場合の生成処理
            enemyFactory.SpownBossEnemies(id);
        }
        else
        {
            Debug.Log("NormalEnemy Create");

            // 通常戦闘の場合の生成処理
            enemyFactory.SpownNormalEnemies();

        }

        Debug.Log("EnemyCreate End");
    }

    IEnumerator SetupBattle()
    {
        // 初期化待ちなどあればここ
        yield return new WaitForSeconds(0.5f);
        StartPlayerTurn();
    }

    // --- ターン進行フロー ---

    public void Update()
    {


    }

    //  ターン開始時のリセット処理
    void StartPlayerTurn()
    {
        // ターン開始処理
        isCoverSelectedThisTurn = false;
        turnActions.Clear();
        foreach (var p in players)
        {
            p.ResetTurnState();
        }
        foreach (var e in enemies)
        {
            e.ResetTurnState();
        }

        uiManager.ShowPhaseText("Player Turn");

        // 戦う or 逃げる の選択へ
        state = BattleState.PlayerMenu;
        uiManager.ShowRootMenu();
    }

    // UIボタン: 「戦う」選択
    public void OnFightButton()
    {
        state = BattleState.ActionSelect;
        currentPlayerIndex = 0;
        SelectActionForCharacter(currentPlayerIndex);
    }

    // UIボタン: 「逃げる」選択
    public void OnRunButton()
    {
        // 逃走処理（今回は省略、終了など）
        Debug.Log("逃げた！");
    }

    // キャラクターごとの行動選択開始
    void SelectActionForCharacter(int index)
    {
        // 死亡しているキャラはスキップ
        if (players[index].isDead)
        {
            NextCharSelection();
            return;
        }

        uiManager.ShowActionMenu(players[index], isCoverSelectedThisTurn);
    }

    // UIボタン: スキル/コマンド選択時
    public void OnSkillSelected(SkillData skill)
    {
        currentSelectedSkill = skill;

        // 対象選択が不要なもの（防御など）は即決定
        if (skill.type == ActionType.Defend)
        {
            RegisterAction(players[currentPlayerIndex], players[currentPlayerIndex], skill); // 自分対象
            NextCharSelection();
        }
        else
        {
            state = BattleState.TargetSelect;
            // ターゲットリスト作成
            List<UnitController> targets = skill.isTargetEnemy ? enemies : players;
            uiManager.ShowTargetMenu(targets, OnTargetSelected);
        }
    }

    // UIボタン: ターゲット選択時
    public void OnTargetSelected(UnitController target)
    {
        // 庇うを選択した場合のフラグ管理
        if (currentSelectedSkill.type == ActionType.Cover)
        {
            isCoverSelectedThisTurn = true;
        }

        RegisterAction(players[currentPlayerIndex], target, currentSelectedSkill);
        NextCharSelection();
    }

    //  アクションレジスター
    void RegisterAction(UnitController user, UnitController target, SkillData skill)
    {
        BattleAction action = new BattleAction();
        action.actor = user;
        action.target = target;
        action.skill = skill;
        action.speedPriority = user.speed;
        turnActions.Add(action);
    }

    void NextCharSelection()
    {
        currentPlayerIndex++;
        if (currentPlayerIndex < players.Count)
        {
            SelectActionForCharacter(currentPlayerIndex);
        }
        else
        {
            // 全員選択終了 -> 敵の思考 -> 実行フェーズ
            CalculateEnemyActions();
            StartCoroutine(ExecutePhase());
        }
    }

    // --- 敵AI ---

    void CalculateEnemyActions()
    {
        foreach (var enemy in enemies)
        {
            if (enemy.isDead) continue;

            // 簡易AI: 基本攻撃
            SkillData enemySkill = commonSkills.Find(s => s.type == ActionType.Attack); // Inspectorで設定しておく

            UnitController target = SelectTargetByHate();

            RegisterAction(enemy, target, enemySkill);
        }
    }

    UnitController SelectTargetByHate()
    {
        var alivePlayers = players.Where(p => !p.isDead).ToList();
        if (alivePlayers.Count == 0) return null;

        // ヘイト100のキャラがいれば確定
        var maxHateUnit = alivePlayers.Find(p => p.currentHate >= 100);
        if (maxHateUnit != null) return maxHateUnit;

        // ヘイトによる重みづけ抽選
        int totalHate = alivePlayers.Sum(p => p.currentHate + 10); // +10はヘイト0でも狙われる確率を残すため
        int randomValue = Random.Range(0, totalHate);
        int currentWeight = 0;

        foreach (var p in alivePlayers)
        {
            currentWeight += (p.currentHate + 10);
            if (randomValue < currentWeight) return p;
        }

        return alivePlayers[0];
    }


    // --- 実行フェーズ ---

    IEnumerator ExecutePhase()
    {
        state = BattleState.ExecutePhase;
        uiManager.HideAllMenus();

        // 速度順にソート
        turnActions = turnActions.OrderByDescending(a => a.speedPriority).ToList();

        foreach (var action in turnActions)
        {
            if (action.actor.isDead) continue; // 死んでたら行動できない
            if (CheckBattleEnd()) yield break;

            // ターゲット生存確認とリターゲット
            if (action.target.isDead)
            {
                action.target = Retarget(action.target, action.skill.isTargetEnemy);
                if (action.target == null) continue; // 相手全滅時は処理不要
            }

            // 「庇う」処理のチェック (攻撃行動かつターゲットが味方)
            // 敵の攻撃(Action) -> プレイヤー(Target) -> 誰か庇ってる？
            if (action.skill.isTargetEnemy && action.target.isPlayer)
            {
                UnitController coverUnit = players.FirstOrDefault(p => p.isCovering && !p.hasCoveredThisTurn && !p.isDead && p != action.target);
                if (coverUnit != null)
                {
                    // 庇う発動
                    //  ターゲット変更しか発動していないので、特有処理を入れてない
                    Debug.Log(coverUnit.unitName + "が庇った！");
                    action.target = coverUnit;
                    coverUnit.hasCoveredThisTurn = true; // 1回のみ
                    // エフェクトなど入れるならここ
                }
            }

            // 行動実行
            yield return StartCoroutine(PerformAction(action));
        }

        // 全行動終了後の処理
        yield return new WaitForSeconds(1f);

        // ターン終了判定
        if (!CheckBattleEnd())
        {
            StartPlayerTurn(); // 最初に戻る
        }
        else // 戦闘終了
        {
            System.FadeManager.FadeChangeScene("FieldScene", 1.0f);
        }
    }

    // リターゲットロジック（リストの次、最後なら最初）
    UnitController Retarget(UnitController deadTarget, bool isTargetEnemy)
    {
        List<UnitController> group = isTargetEnemy ? enemies : players;
        int originalIndex = group.IndexOf(deadTarget);
        if (originalIndex == -1) return null; // ありえないはずだが念のため

        // リストを走査して生きているキャラを探す
        for (int i = 1; i <= group.Count; i++)
        {
            int nextIndex = (originalIndex + i) % group.Count;
            if (!group[nextIndex].isDead)
            {
                return group[nextIndex];
            }
        }
        return null; // 全滅
    }

    //  
    IEnumerator PerformAction(BattleAction action)
    {

        UnitController actor = action.actor;
        UnitController target = action.target;

        // 行動者アニメーション（ジャンプ）
        //    yield return StartCoroutine(actor.AnimateActionJump());
        //  攻撃・スキルエフェクトなど
        yield return StartCoroutine(actor.AnimateActionAttack());

        //yield return new WaitForSeconds(target.animationLength);

        // 実際の効果処理
        string msg = $"{actor.unitName}の{action.skill.skillName}！";
        uiManager.ShowLog(msg);
        Debug.Log(msg);

        // 特殊行動のステートセット
        if (action.skill.type == ActionType.Defend) actor.isDefending = true;
        if (action.skill.type == ActionType.Cover) actor.isCovering = true;

        if (actor.isPlayer) // プレイヤーのみ
        {
            actor.AddHate(action.skill.hateIncrease);
        }

        // 攻撃・スキル・アイテムの場合
        if (action.skill.type == ActionType.Attack || action.skill.type == ActionType.Skill)
        {
            // 対象のアニメーション（点滅）
            yield return StartCoroutine(target.AnimateBlink(0.5f));

            int dmg = action.skill.power;
            // 防御などの計算
            if (target.isDefending) dmg /= 2;

            target.TakeDamage(dmg);
        }

        yield return new WaitForSeconds(1.0f); // 余韻
    }

    //  戦闘終了判定
    bool CheckBattleEnd()
    {
        bool allPlayersDead = players.All(p => p.isDead);
        bool allEnemiesDead = enemies.All(e => e.isDead);

        if (allEnemiesDead)
        {
            state = BattleState.Win;
            uiManager.ShowResult("Victory!", true);
            return true;
        }
        if (allPlayersDead)
        {
            state = BattleState.Lose;
            uiManager.ShowResult("Defeated...", false);
            return true;
        }
        return false;
    }
}

