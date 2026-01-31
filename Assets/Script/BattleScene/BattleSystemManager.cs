using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
//using System;

public enum BattleState
{
    Start, 
    PlayerMenu, 
    ActionSelect, 
    AviritySelect,
    TargetSelect, 
    EnemyPhase, 
    ExecutePhase, 
    Win, Lose }

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
        EnemyCreate();

        //  開始処理
        StartCoroutine(SetupBattle());
    }

    void DebugInit()
    {
        //　データマネージャーの初期化
    //    DataManager.Instance.SetupParty(3, DataManager.Instance.PlayerPrefab);

    }

    //  キャラクターのステータス設定
    void CharactorStateSet()
    {
        var playerDataList = DataManager.Instance.currentParty.members;
        Debug.Log("CharactorStateSet Start");
        Debug.Log("Player Count: " + players.Count);
        for (int i = 0; i < players.Count; i++)
        {
            Debug.Log("Setting up player " + i + ": " + playerDataList[i].GetName());
            players[i].UnitInit(playerDataList[i]);

        }

    }

    //  敵の生成処理
    void EnemyCreate()
    {
        Debug.Log("EnemyCreate Start");

        //  生成数決定
        //  ランダムに1～3体生成
        enemies.Clear();
        int enemyCount = Random.Range(1, 4);

        Debug.Log("EnemyCount: " + enemyCount);

        //  今がボス線か判定
        if (DataManager.Instance.isBossBattle)
        {
            Debug.Log("BossUnit Create");

            DataManager.Instance.isBossBattle = false; // フラグリセット
            int id = DataManager.Instance.currentBossID;

            Debug.Log("BossID: " + id);

            // ボス戦の場合の生成処理
            enemies.Add(enemyFactory.SpownBossEnemies(id,enemies.Count));

            enemyCount--;
        }
    

        //  残りのenemycount分生成
        for (int i = 0; i < enemyCount; i++)
        {
            Debug.Log("NormalEnemy Create");
            enemies.Add(enemyFactory.SpownNormalEnemies(enemies.Count));
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
        SoundManager.Instance.PlaySE("SE_Confirm");

        state = BattleState.ActionSelect;
        currentPlayerIndex = 0;
        SelectActionForCharacter(currentPlayerIndex);
    }

    // UIボタン: 「逃げる」選択
    public void OnRunButton()
    {
        SoundManager.Instance.PlaySE("SE_Cancel");

        // 逃走処理（今回は省略、終了など）
        Debug.Log("逃げた！");
        System.FadeManager.FadeChangeScene("FieldScene", 1.0f);
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

        SoundManager.Instance.PlaySE("SE_Confirm");

        // 対象選択が不要なもの（防御など）は即決定
        if (skill.type == ActionType.Defend)
        {
            RegisterAction(players[currentPlayerIndex], players[currentPlayerIndex], skill); // 自分対象
            NextCharSelection();
        }
//        else if(skill.type == ActionType.Avirity)
//        {
//            state = BattleState.AviritySelect;
//
//            //  選択中のキャラクターを取得
//            UnitController currentChar = players[currentPlayerIndex];
//            uiManager.ShowSkillMenu(currentChar);
//        }
        else
        {
            state = BattleState.TargetSelect;
            // ターゲットリスト作成
            List<UnitController> targets = skill.isTargetEnemy ? enemies : players;
            uiManager.ShowTargetMenu(targets, OnTargetSelected);
        }
    }

    //  スキル選択へ
    public void OnAviritySkillSelected()
    {

        SoundManager.Instance.PlaySE("SE_Confirm");

        //       currentSelectedSkill = skill;
        //       state = BattleState.TargetSelect;
        //       // ターゲットリスト作成
        //       List<UnitController> targets = skill.isTargetEnemy ? enemies : players;
        //       uiManager.ShowTargetMenu(targets, OnTargetSelected);
        state = BattleState.AviritySelect;
        //  選択中のキャラクターを取得
        UnitController currentChar = players[currentPlayerIndex];
        uiManager.ShowSkillMenu(currentChar);
    }

    // UIボタン: ターゲット選択時
    public void OnTargetSelected(UnitController target)
    {
        SoundManager.Instance.PlaySE("SE_Confirm");


        // 庇うを選択した場合のフラグ管理
        if (currentSelectedSkill.type == ActionType.Cover)
        {
            isCoverSelectedThisTurn = true;
        }

        RegisterAction(players[currentPlayerIndex], target, currentSelectedSkill);
        NextCharSelection();
    }

    // UIボタン: キャラのスキル選択時
    public void OnSkillFromCharSelected(SkillData skill)
    {
        SoundManager.Instance.PlaySE("SE_Confirm");

        currentSelectedSkill = skill;
        state = BattleState.TargetSelect;
        // ターゲットリスト作成
        List<UnitController> targets = skill.isTargetEnemy ? enemies : players;
        uiManager.ShowTargetMenu(targets, OnTargetSelected);
    }

    //  アクションレジスター
    void RegisterAction(UnitController user, UnitController target, SkillData skill)
    {
        BattleAction action = new BattleAction();
        action.actor = user;
        action.target = target;
        action.skill = skill;
        action.speedPriority = user.GetSpeed();
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

    //  ヘイトに基づくターゲット選択
    UnitController SelectTargetByHate()
    {
        var alivePlayers = players.Where(p => !p.isDead).ToList();
        if (alivePlayers.Count == 0) return null;

        // ヘイト100のキャラがいれば確定
        var maxHateUnit = alivePlayers.Find(p => p.currentHate >= 100);
        if (maxHateUnit != null) 
        {
            maxHateUnit.currentHate = 0; // ヘイトリセット
            return maxHateUnit;
        }

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
                    //  ターゲット変更しか発動していないので、特有処理を入れてない
                    Debug.Log(coverUnit.GetUnitName() + "が庇った！");

                    SoundManager.Instance.PlaySE("SE_Kabau");

                    // 庇う発動
                    coverUnit.GetProtectSystem().
                        ExecuteProtect(coverUnit.GetUnitData(),
                        action.target.GetUnitData(),action.actor.GetUnitData());

                //    action.target = coverUnit;
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
        //    System.FadeManager.FadeChangeScene("FieldScene", 1.0f);
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

    //  行動の実行処理
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
        string msg = $"{actor.GetUnitName()}の{action.skill.skillName}！";
        uiManager.ShowLog(msg);
        Debug.Log(msg);

        // 特殊行動のステートセット
        if (action.skill.type == ActionType.Defend) actor.isDefending = true;
        if (action.skill.type == ActionType.Cover) actor.isCovering = true;

        if (actor.isPlayer) // プレイヤーのみ
        {
            actor.AddHate(action.skill.hateIncrease);
        }

        //  対象の数取得
        targetType targetCount = action.skill.targetType; 


        if(targetCount == targetType.All)
        {
            // 全体対象の場合
            List<UnitController> targets = action.skill.isTargetEnemy ? enemies : players;
            foreach (var t in targets)
            {
                if (t.isDead) continue;
                // 対象のアニメーション（点滅）
                yield return StartCoroutine(t.AnimateBlink(0.5f));
                //  ダメージ計算
                if (action.skill.type == ActionType.Attack ||
                    action.skill.type == ActionType.Avirity)
                {
                    Attack(new BattleAction
                    {
                        actor = action.actor,
                        target = t,
                        skill = action.skill,
                        speedPriority = action.speedPriority
                    });
                }
                //  回復スキルの場合
                else if (action.skill.type == ActionType.Heal)
                {
                    Heal(new BattleAction
                    {
                        actor = action.actor,
                        target = t,
                        skill = action.skill,
                        speedPriority = action.speedPriority
                    });
                }
                else if(action.skill.type == ActionType.Debuff)
                {
                    //  ステータスデバフ処理
                //    t.GetUnitData().GetStatusRuntime().ApplyDebuff(action.skill);
                }
                else if (action.skill.type == ActionType.Buff)
                {
                    //  ステータスバフ処理
                //    t.GetUnitData().GetStatusRuntime().ApplyBuff(action.skill);
                }
            }
        }
        else 
        {
            // 攻撃・スキル・アイテムの場合
            if (action.skill.type == ActionType.Attack ||
                action.skill.type == ActionType.Avirity)
            {
                // 対象のアニメーション（点滅）
                yield return StartCoroutine(target.AnimateBlink(0.5f));

                //  ダメージ計算
                Attack(action);
            }
            //  回復スキルの場合
            else if (action.skill.type == ActionType.Heal)
            {
                Heal(action);
            }
        }



        yield return new WaitForSeconds(1.0f); // 余韻
    }

    void Attack(BattleAction action)
    {
        // 基本ダメージ計算（単純化のため）
        //  スキルの威力 × 攻撃者の攻撃力
        int dmg = (action.skill.power *
            action.actor.GetUnitData().GetStatusRuntime().atk) / 100;
        Debug.Log("基本ダメージ計算: " + dmg);
        // 防御などの計算
        if (action.target.isDefending) dmg /= 2;
        //  防御力に応じて実数値で減少
        dmg = dmg - (action.target.GetUnitData().GetStatusRuntime().def / 4);
        Debug.Log("防御力考慮後ダメージ: " + dmg);
        if (dmg < 1)
        {
            dmg = 1; // 最低1ダメージは与える
            Debug.Log("最低ダメージ適用");
        }

        //  属性補正
        //    if (action.skill.element != ElementType.None)
        //    {
        //        //  属性補正処理
        //        float elementModifier = 
        //            action.target.GetUnitData().GetStatusRuntime().
        //            GetElementModifier(action.skill.element);
        //        dmg = Mathf.RoundToInt(dmg * elementModifier);
        //        Debug.Log("属性補正後ダメージ: " + dmg);
        //    }

        //  効果音再生
        if (action.skill.element == Element.Fire)
        {
            SoundManager.Instance.PlaySE("SE_Fire_Atk");
        }
        else if(action.skill.element == Element.Water)
        {
            SoundManager.Instance.PlaySE("SE_Water_Atk");
        }
        else if(action.skill.element == Element.Grass)
        {
            SoundManager.Instance.PlaySE("SE_Flower_Atk");
        }
        else
        {
            SoundManager.Instance.PlaySE("SE_Player_atk");
        }

        action.target.TakeDamage(dmg);
    }

    void Heal(BattleAction action)
    {
        // 回復量計算など
        int healAmount = (action.skill.power +
            action.actor.GetUnitData().GetStatusRuntime().atk / 2) / 100;
        action.target.HealDamage(healAmount);
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

    public void OnBackButton()
    {
        SoundManager.Instance.PlaySE("SE_Cancel");

        switch (state)
        {
            case BattleState.TargetSelect:
            case BattleState.AviritySelect:
                BackToActionSelect();
                break;
            case BattleState.ActionSelect:
                BackToPlayerMenu();
                break;
            case BattleState.PlayerMenu:
                // ここでは何もしない or キャンセル不可
                break;
        }
    }


    void BackToActionSelect()
    {
        currentSelectedSkill = null;

        UnitController currentChar = players[currentPlayerIndex];

        state = BattleState.ActionSelect;
        uiManager.ShowActionMenu(currentChar, isCoverSelectedThisTurn);
    }

    void BackToPlayerMenu()
    {
        //  現在選択中のキャラクターが０番の場合ルートメニューへ
        //  そうでない場合は前のキャラクター選択へ戻る
        if(currentPlayerIndex == 0)
        {
            state = BattleState.PlayerMenu;
            uiManager.ShowRootMenu();
            return;
        }
        currentPlayerIndex--;
        SelectActionForCharacter(currentPlayerIndex);
        //  登録済みのアクションを削除
        turnActions.RemoveAll(a => a.actor == players[currentPlayerIndex]);

    }
}

