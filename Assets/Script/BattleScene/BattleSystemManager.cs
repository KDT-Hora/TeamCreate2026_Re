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
    Win, Lose 
 }

public class BattleSystemManager : MonoBehaviour
{

    [SerializeField] private PlayEffect effectPlayer;

    [Header("Units")]
    public List<UnitController> players;
    public List<UnitController> enemies;

    [Header("UI Manager")]
    public BattleUIManager uiManager;

    [Header("Enemy Unit Factory")]
    public EnemyUnitFactory enemyFactory;

    [Header("Settings")]
    public List<SkillData> commonSkills; // 謾ｻ謦��亟蠕｡縺ｪ縺ｩ

    // 蜀�Κ繧ｹ繝��繝
    private BattleState state;
    private int currentPlayerIndex = 0; // 現在コマンド選択中のキャラ
    private SkillData currentSelectedSkill; // 現在選択中のスキル
    private bool isCoverSelectedThisTurn = false; // チーム全体で「庇う」が選択されたか
    private UnitController protectTarget;
    private UnitController protectActor;
    private bool hasCoverThisTurn = false;


    // 陦悟虚繝ｪ繧ｹ繝
    private List<BattleAction> turnActions = new List<BattleAction>();

    void Start()
    {
        //  繝�ヰ繝�げ逕ｨ縺ｮ蛻晄悄蛹
        DebugInit();


        state = BattleState.Start;
        //  繧ｭ繝｣繝ｩ繧ｯ繧ｿ繝ｼ繧ｹ繝��繧ｿ繧ｹ險ｭ螳
        CharactorStateSet();
        //  謨ｵ縺ｮ逕滓�
        EnemyCreate();

        //  髢句ｧ句�逅
        StartCoroutine(SetupBattle());
    }

    void DebugInit()
    {
        //縲繝��繧ｿ繝槭ロ繝ｼ繧ｸ繝｣繝ｼ縺ｮ蛻晄悄蛹
        //    DataManager.Instance.SetupParty(3, DataManager.Instance.PlayerPrefab);

    }

    //  繧ｭ繝｣繝ｩ繧ｯ繧ｿ繝ｼ縺ｮ繧ｹ繝��繧ｿ繧ｹ險ｭ螳
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

    //  謨ｵ縺ｮ逕滓�蜃ｦ逅
    void EnemyCreate()
    {
        Debug.Log("EnemyCreate Start");

        //  逕滓�謨ｰ豎ｺ螳
        //  繝ｩ繝ｳ繝繝縺ｫ1�3菴鍋函謌
        enemies.Clear();
        int enemyCount = Random.Range(1, 4);

        Debug.Log("EnemyCount: " + enemyCount);

        //  莉翫′繝懊せ邱壹°蛻､螳
        if (DataManager.Instance.isBossBattle)
        {
            Debug.Log("BossUnit Create");

            DataManager.Instance.isBossBattle = false; // 繝輔Λ繧ｰ繝ｪ繧ｻ繝�ヨ
            int id = DataManager.Instance.currentBossID;

            Debug.Log("BossID: " + id);

            // 繝懊せ謌ｦ縺ｮ蝣ｴ蜷医�逕滓�蜃ｦ逅
            enemies.Add(enemyFactory.SpownBossEnemies(id, enemies.Count));

            enemyCount--;
        }


        //  谿九ｊ縺ｮenemycount蛻�函謌
        for (int i = 0; i < enemyCount; i++)
        {
            Debug.Log("NormalEnemy Create");
            enemies.Add(enemyFactory.SpownNormalEnemies(enemies.Count));
        }

        Debug.Log("EnemyCreate End");
    }

    IEnumerator SetupBattle()
    {
        // 蛻晄悄蛹門ｾ�■縺ｪ縺ｩ縺ゅｌ縺ｰ縺薙％
        yield return new WaitForSeconds(0.5f);
        StartPlayerTurn();
    }

    // --- 繧ｿ繝ｼ繝ｳ騾ｲ陦後ヵ繝ｭ繝ｼ ---

    public void Update()
    {


    }

    //  繧ｿ繝ｼ繝ｳ髢句ｧ区凾縺ｮ繝ｪ繧ｻ繝�ヨ蜃ｦ逅
    void StartPlayerTurn()
    {
        // 繧ｿ繝ｼ繝ｳ髢句ｧ句�逅
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

        // 謌ｦ縺 or 騾�￡繧 縺ｮ驕ｸ謚槭∈
        state = BattleState.PlayerMenu;
        uiManager.ShowRootMenu();
    }

    // UI繝懊ち繝ｳ: 縲梧姶縺�埼∈謚
    public void OnFightButton()
    {
        SoundManager.Instance.PlaySE("SE_Confirm");

        state = BattleState.ActionSelect;
        currentPlayerIndex = 0;
        SelectActionForCharacter(currentPlayerIndex);
    }

    // UI繝懊ち繝ｳ: 縲碁�￡繧九埼∈謚
    public void OnRunButton()
    {
        SoundManager.Instance.PlaySE("SE_Cancel");

        // 騾�ｵｰ蜃ｦ逅�ｼ井ｻ雁屓縺ｯ逵∫払縲∫ｵゆｺ�↑縺ｩ�
        Debug.Log("騾�￡縺滂ｼ");
        System.FadeManager.FadeChangeScene("FieldScene", 1.0f);
    }

    // 繧ｭ繝｣繝ｩ繧ｯ繧ｿ繝ｼ縺斐→縺ｮ陦悟虚驕ｸ謚樣幕蟋
    void SelectActionForCharacter(int index)
    {
        // 豁ｻ莠｡縺励※縺�ｋ繧ｭ繝｣繝ｩ縺ｯ繧ｹ繧ｭ繝��
        if (players[index].isDead)
        {
            NextCharSelection();
            return;
        }

        uiManager.ShowActionMenu(players[index], isCoverSelectedThisTurn);
    }

    // UI繝懊ち繝ｳ: 繧ｹ繧ｭ繝ｫ/繧ｳ繝槭Φ繝蛾∈謚樊凾
    public void OnSkillSelected(SkillData skill)
    {
        currentSelectedSkill = skill;

        SoundManager.Instance.PlaySE("SE_Confirm");


        //  かばいを選択していたら
        if(skill.type == ActionType.Cover)
        {
     //       isCoverSelectedThisTurn = true;
        }

        // 対象選択が不要なもの（防御など）は即決定
        if (skill.type == ActionType.Defend)
        {
            RegisterAction(players[currentPlayerIndex], players[currentPlayerIndex], skill); // 閾ｪ蛻�ｯｾ雎｡
            NextCharSelection();
        }
        //        else if(skill.type == ActionType.Avirity)
        //        {
        //            state = BattleState.AviritySelect;
        //
        //            //  驕ｸ謚樔ｸｭ縺ｮ繧ｭ繝｣繝ｩ繧ｯ繧ｿ繝ｼ繧貞叙蠕
        //            UnitController currentChar = players[currentPlayerIndex];
        //            uiManager.ShowSkillMenu(currentChar);
        //        }
        else
        {
            state = BattleState.TargetSelect;
            // 繧ｿ繝ｼ繧ｲ繝�ヨ繝ｪ繧ｹ繝井ｽ懈�
            List<UnitController> targets = skill.isTargetEnemy ? enemies : players;
            uiManager.ShowTargetMenu(targets, OnTargetSelected);
        }
    }

    //  繧ｹ繧ｭ繝ｫ驕ｸ謚槭∈
    public void OnAviritySkillSelected()
    {

        SoundManager.Instance.PlaySE("SE_Confirm");

        //       currentSelectedSkill = skill;
        //       state = BattleState.TargetSelect;
        //       // 繧ｿ繝ｼ繧ｲ繝�ヨ繝ｪ繧ｹ繝井ｽ懈�
        //       List<UnitController> targets = skill.isTargetEnemy ? enemies : players;
        //       uiManager.ShowTargetMenu(targets, OnTargetSelected);
        state = BattleState.AviritySelect;
        //  驕ｸ謚樔ｸｭ縺ｮ繧ｭ繝｣繝ｩ繧ｯ繧ｿ繝ｼ繧貞叙蠕
        UnitController currentChar = players[currentPlayerIndex];
        uiManager.ShowSkillMenu(currentChar);
    }

    // UI繝懊ち繝ｳ: 繧ｿ繝ｼ繧ｲ繝�ヨ驕ｸ謚樊凾
    public void OnTargetSelected(UnitController target)
    {
        SoundManager.Instance.PlaySE("SE_Confirm");


        // 蠎�≧繧帝∈謚槭＠縺溷ｴ蜷医�繝輔Λ繧ｰ邂｡逅
        if (currentSelectedSkill.type == ActionType.Cover)
        {
            isCoverSelectedThisTurn = true;
        }

        RegisterAction(players[currentPlayerIndex], target, currentSelectedSkill);
        NextCharSelection();
    }

    // UI繝懊ち繝ｳ: 繧ｭ繝｣繝ｩ縺ｮ繧ｹ繧ｭ繝ｫ驕ｸ謚樊凾
    public void OnSkillFromCharSelected(SkillData skill)
    {
        SoundManager.Instance.PlaySE("SE_Confirm");

        currentSelectedSkill = skill;
        state = BattleState.TargetSelect;
        // 繧ｿ繝ｼ繧ｲ繝�ヨ繝ｪ繧ｹ繝井ｽ懈�
        List<UnitController> targets = skill.isTargetEnemy ? enemies : players;
        uiManager.ShowTargetMenu(targets, OnTargetSelected);
    }

    //  繧｢繧ｯ繧ｷ繝ｧ繝ｳ繝ｬ繧ｸ繧ｹ繧ｿ繝ｼ
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
            // 蜈ｨ蜩｡驕ｸ謚樒ｵゆｺ -> 謨ｵ縺ｮ諤晁 -> 螳溯｡後ヵ繧ｧ繝ｼ繧ｺ
            CalculateEnemyActions();
            StartCoroutine(ExecutePhase());
        }
    }

    // --- 謨ｵAI ---

    void CalculateEnemyActions()
    {
        foreach (var enemy in enemies)
        {
            if (enemy.isDead) continue;

            // 邁｡譏鄭I: 蝓ｺ譛ｬ謾ｻ謦
            SkillData enemySkill = commonSkills.Find(s => s.type == ActionType.Attack); // Inspector縺ｧ險ｭ螳壹＠縺ｦ縺翫￥

            UnitController target = SelectTargetByHate();

            RegisterAction(enemy, target, enemySkill);
        }
    }

    //  繝倥う繝医↓蝓ｺ縺･縺上ち繝ｼ繧ｲ繝�ヨ驕ｸ謚
    UnitController SelectTargetByHate()
    {
        var alivePlayers = players.Where(p => !p.isDead).ToList();
        if (alivePlayers.Count == 0) return null;

        // 繝倥う繝100縺ｮ繧ｭ繝｣繝ｩ縺後＞繧後�遒ｺ螳
        var maxHateUnit = alivePlayers.Find(p => p.currentHate >= 100);
        if (maxHateUnit != null)
        {
            maxHateUnit.currentHate = 0; // 繝倥う繝医Μ繧ｻ繝�ヨ
            return maxHateUnit;
        }

        // 繝倥う繝医↓繧医ｋ驥阪∩縺･縺第歓驕ｸ
        int totalHate = alivePlayers.Sum(p => p.currentHate + 10); // +10縺ｯ繝倥う繝0縺ｧ繧ら漁繧上ｌ繧狗｢ｺ邇�ｒ谿九☆縺溘ａ
        int randomValue = Random.Range(0, totalHate);
        int currentWeight = 0;

        foreach (var p in alivePlayers)
        {
            currentWeight += (p.currentHate + 10);
            if (randomValue < currentWeight) return p;
        }

        return alivePlayers[0];
    }


    // --- 螳溯｡後ヵ繧ｧ繝ｼ繧ｺ ---

    IEnumerator ExecutePhase()
    {
        state = BattleState.ExecutePhase;
        uiManager.HideAllMenus();

        // 騾溷ｺｦ鬆�↓繧ｽ繝ｼ繝
        turnActions = turnActions.OrderByDescending(a => a.speedPriority).ToList();

        foreach (var action in turnActions)
        {
            if (action.actor.isDead) continue; // 豁ｻ繧薙〒縺溘ｉ陦悟虚縺ｧ縺阪↑縺
            if (CheckBattleEnd()) yield break;

            // 繧ｿ繝ｼ繧ｲ繝�ヨ逕溷ｭ倡｢ｺ隱阪→繝ｪ繧ｿ繝ｼ繧ｲ繝�ヨ
            if (action.target.isDead)
            {
                action.target = Retarget(action.target, action.skill.isTargetEnemy);
                if (action.target == null) continue; // 逶ｸ謇句�貊�凾縺ｯ蜃ｦ逅�ｸ崎ｦ
            }


            //  かばいセット
            if(action.skill.type == ActionType.Cover) {
                protectTarget = action.target;
                protectActor = action.actor;
                hasCoverThisTurn = true;
            }

            // 「庇う」処理のチェック (攻撃行動かつターゲットが味方)
            // 敵の攻撃(Action) -> プレイヤー(Target) -> 誰か庇ってる？
            if(action.target == protectTarget && isCoverSelectedThisTurn&&
                !action.actor.isPlayer)
            {
             //   UnitController coverUnit = 
             //       players.FirstOrDefault(p => p.isCovering && !p.hasCoveredThisTurn && !p.isDead && p != action.target);

                //  かばい者が生きていたら
                if (!protectActor.isDead)
                {

                    Debug.Log(protectActor.GetUnitName() + "が庇った！");
                    uiManager.AddLogText(protectActor.GetUnitName() + "が庇った！");

                    //  SE
                    SoundManager.Instance.PlaySE("SE_Kabau");

                    // 庇う発動
                    protectActor.GetProtectSystem().
                        ExecuteProtect(
                        protectActor,
                        action);



                    hasCoverThisTurn = true; // 1回のみ
                    // エフェクトなど入れるならここ
                }
            }

            // 陦悟虚螳溯｡
            yield return StartCoroutine(PerformAction(action));
        }

        // 蜈ｨ陦悟虚邨ゆｺ�ｾ後�蜃ｦ逅
        yield return new WaitForSeconds(1f);

        // 繧ｿ繝ｼ繝ｳ邨ゆｺ�愛螳
        if (!CheckBattleEnd())
        {
            StartPlayerTurn(); // 譛蛻昴↓謌ｻ繧
        }
        else // 謌ｦ髣倡ｵゆｺ
        {
            //    System.FadeManager.FadeChangeScene("FieldScene", 1.0f);
        }
    }

    // 繝ｪ繧ｿ繝ｼ繧ｲ繝�ヨ繝ｭ繧ｸ繝�け�医Μ繧ｹ繝医�谺｡縲∵怙蠕後↑繧画怙蛻晢ｼ
    UnitController Retarget(UnitController deadTarget, bool isTargetEnemy)
    {
        List<UnitController> group = isTargetEnemy ? enemies : players;
        int originalIndex = group.IndexOf(deadTarget);
        if (originalIndex == -1) return null; // 縺ゅｊ縺医↑縺��縺壹□縺悟ｿｵ縺ｮ縺溘ａ

        // 繝ｪ繧ｹ繝医ｒ襍ｰ譟ｻ縺励※逕溘″縺ｦ縺�ｋ繧ｭ繝｣繝ｩ繧呈爾縺
        for (int i = 1; i <= group.Count; i++)
        {
            int nextIndex = (originalIndex + i) % group.Count;
            if (!group[nextIndex].isDead)
            {
                return group[nextIndex];
            }
        }
        return null; // 蜈ｨ貊
    }

    //  陦悟虚縺ｮ螳溯｡悟�逅
    IEnumerator PerformAction(BattleAction action)
    {

        UnitController actor = action.actor;
        UnitController target = action.target;

        // 陦悟虚閠�い繝九Γ繝ｼ繧ｷ繝ｧ繝ｳ�医ず繝｣繝ｳ繝暦ｼ
        //    yield return StartCoroutine(actor.AnimateActionJump());
        //  謾ｻ謦��繧ｹ繧ｭ繝ｫ繧ｨ繝輔ぉ繧ｯ繝医↑縺ｩ
        yield return StartCoroutine(actor.AnimateActionAttack());

        //yield return new WaitForSeconds(target.animationLength);

        // 螳滄圀縺ｮ蜉ｹ譫懷�逅
        string msg = $"{actor.GetUnitName()}縺ｮ{action.skill.skillName}�";
        uiManager.ShowLog(msg);
        Debug.Log(msg);

        // 迚ｹ谿願｡悟虚縺ｮ繧ｹ繝��繝医そ繝�ヨ
        if (action.skill.type == ActionType.Defend) actor.isDefending = true;
        if (action.skill.type == ActionType.Cover) actor.isCovering = true;

        if (actor.isPlayer) // 繝励Ξ繧､繝､繝ｼ縺ｮ縺ｿ
        {
            actor.AddHate(action.skill.hateIncrease);
        }

        //  蟇ｾ雎｡縺ｮ謨ｰ蜿門ｾ
        targetType targetCount = action.skill.targetType;


        if (targetCount == targetType.All)
        {
            // 蜈ｨ菴灘ｯｾ雎｡縺ｮ蝣ｴ蜷
            List<UnitController> targets = action.skill.isTargetEnemy ? enemies : players;
            foreach (var t in targets)
            {
                if (t.isDead) continue;
                // 蟇ｾ雎｡縺ｮ繧｢繝九Γ繝ｼ繧ｷ繝ｧ繝ｳ�育せ貊�ｼ
                yield return StartCoroutine(t.AnimateBlink(0.5f));
                //  繝繝｡繝ｼ繧ｸ險育ｮ
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
                //  蝗槫ｾｩ繧ｹ繧ｭ繝ｫ縺ｮ蝣ｴ蜷
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
                else if (action.skill.type == ActionType.Debuff)
                {
                    //  ステータスデバフ処理
                    DeBuff(action,t);
                    
                }
                else if (action.skill.type == ActionType.Buff)
                {
                    //  ステータスバフ処理
                    Buff(action, t);
                }
            }
        }
        else
        {
            // 謾ｻ謦��繧ｹ繧ｭ繝ｫ繝ｻ繧｢繧､繝�Β縺ｮ蝣ｴ蜷
            if (action.skill.type == ActionType.Attack ||
                action.skill.type == ActionType.Avirity)
            {
                // 蟇ｾ雎｡縺ｮ繧｢繝九Γ繝ｼ繧ｷ繝ｧ繝ｳ�育せ貊�ｼ
                yield return StartCoroutine(target.AnimateBlink(0.5f));

                //  繝繝｡繝ｼ繧ｸ險育ｮ
                Attack(action);
            }
            //  蝗槫ｾｩ繧ｹ繧ｭ繝ｫ縺ｮ蝣ｴ蜷
            else if (action.skill.type == ActionType.Heal)
            {
                Heal(action);
            }
            else if (action.skill.type == ActionType.Debuff)
            {
                //  ステータスデバフ処理
                DeBuff(action,action.target);

            }
            else if (action.skill.type == ActionType.Buff)
            {
                //  ステータスバフ処理
                Buff(action, action.target);
            }
        }



        yield return new WaitForSeconds(1.0f); // 菴咎渊
    }

    void Attack(BattleAction action)
    {
        // 蝓ｺ譛ｬ繝繝｡繝ｼ繧ｸ險育ｮ暦ｼ亥腰邏泌喧縺ｮ縺溘ａ�
        //  繧ｹ繧ｭ繝ｫ縺ｮ螽∝鴨 ﾃ 謾ｻ謦���謾ｻ謦�鴨
        int dmg = (action.skill.power *
            action.actor.GetUnitData().GetStatusRuntime().atk) / 100;
        Debug.Log("蝓ｺ譛ｬ繝繝｡繝ｼ繧ｸ險育ｮ: " + dmg);
        // 髦ｲ蠕｡縺ｪ縺ｩ縺ｮ險育ｮ
        if (action.target.isDefending) dmg /= 2;
        //  髦ｲ蠕｡蜉帙↓蠢懊§縺ｦ螳滓焚蛟､縺ｧ貂帛ｰ
        dmg = dmg - (action.target.GetUnitData().GetStatusRuntime().def / 4);
        Debug.Log("髦ｲ蠕｡蜉幄��蠕後ム繝｡繝ｼ繧ｸ: " + dmg);
        if (dmg < 1)
        {
            dmg = 1; // 譛菴1繝繝｡繝ｼ繧ｸ縺ｯ荳弱∴繧
            Debug.Log("譛菴弱ム繝｡繝ｼ繧ｸ驕ｩ逕ｨ");
        }

        //  螻樊ｧ陬懈ｭ｣
        //    if (action.skill.element != ElementType.None)
        //    {
        //        //  螻樊ｧ陬懈ｭ｣蜃ｦ逅
        //        float elementModifier = 
        //            action.target.GetUnitData().GetStatusRuntime().
        //            GetElementModifier(action.skill.element);
        //        dmg = Mathf.RoundToInt(dmg * elementModifier);
        //        Debug.Log("螻樊ｧ陬懈ｭ｣蠕後ム繝｡繝ｼ繧ｸ: " + dmg);
        //    }

        //  蜉ｹ譫憺浹蜀咲函
        if (action.skill.element == Element.Fire)
        {
            SoundManager.Instance.PlaySE("SE_Fire_Atk");
        }
        else if (action.skill.element == Element.Water)
        {
            SoundManager.Instance.PlaySE("SE_Water_Atk");
        }
        else if (action.skill.element == Element.Grass)
        {
            SoundManager.Instance.PlaySE("SE_Flower_Atk");
        }
        else
        {
            SoundManager.Instance.PlaySE("SE_Player_atk");
        }



        if (effectPlayer != null)
        {
            effectPlayer.SpawnEffect(PlayEffect.EffectType.Damage, action.target.transform.position);
        }

        action.target.TakeDamage(dmg);
    }

    void Heal(BattleAction action)
    {
        // 蝗槫ｾｩ驥剰ｨ育ｮ励↑縺ｩ
        int healAmount = (action.skill.power +
            action.actor.GetUnitData().GetStatusRuntime().atk / 2) / 100;
        if (effectPlayer != null)
        {
            effectPlayer.SpawnEffect(PlayEffect.EffectType.Heal, action.target.transform.position);
        }
        action.target.HealDamage(healAmount);
    }

    void Buff(BattleAction action,UnitController target)
    {
        target.GetUnitData().GetStatusRuntime().AddBuff(action);

        Debug.Log(action.target.nameText + "のステータスが上昇");

        uiManager.AddLogText(action.target.nameText + "のステータスが上昇");

        if (effectPlayer != null)
        {
            effectPlayer.SpawnEffect(PlayEffect.EffectType.Buff, action.target.transform.position);
        }
    }

    void DeBuff(BattleAction action, UnitController target) 
    {
        target.GetUnitData().GetStatusRuntime().AddDeBuff(action);

        Debug.Log(action.target.nameText + "のステータスが減少");

        uiManager.AddLogText(action.target.nameText + "のステータスが上昇");

        if (effectPlayer != null)
        {
            effectPlayer.SpawnEffect(PlayEffect.EffectType.Debuff, action.target.transform.position);
        }
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
                // 縺薙％縺ｧ縺ｯ菴輔ｂ縺励↑縺 or 繧ｭ繝｣繝ｳ繧ｻ繝ｫ荳榊庄
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
        //  迴ｾ蝨ｨ驕ｸ謚樔ｸｭ縺ｮ繧ｭ繝｣繝ｩ繧ｯ繧ｿ繝ｼ縺鯉ｼ千分縺ｮ蝣ｴ蜷医Ν繝ｼ繝医Γ繝九Η繝ｼ縺ｸ
        //  縺昴≧縺ｧ縺ｪ縺�ｴ蜷医�蜑阪�繧ｭ繝｣繝ｩ繧ｯ繧ｿ繝ｼ驕ｸ謚槭∈謌ｻ繧
        if (currentPlayerIndex == 0)
        {
            state = BattleState.PlayerMenu;
            uiManager.ShowRootMenu();
            return;
        }
        currentPlayerIndex--;

        //  キャラクターの行動がかばうなら
        if (turnActions[currentPlayerIndex].skill.type == ActionType.Cover) {
            isCoverSelectedThisTurn = false;
        }

        SelectActionForCharacter(currentPlayerIndex);



        //  登録済みのアクションを削除
        turnActions.RemoveAll(a => a.actor == players[currentPlayerIndex]);

    }
}

