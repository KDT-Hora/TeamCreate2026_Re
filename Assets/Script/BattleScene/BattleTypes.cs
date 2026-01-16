using UnityEngine;

public enum ActionType { Attack, Defend, Skill, Cover, Item }
public enum TargetSide { Player, Enemy }

[System.Serializable]
public class SkillData
{
    public string skillName;
    public int power;               // ダメージや回復量
    public int hateIncrease;        // 使用時のヘイト上昇量
    public ActionType type;         // Skill, Cover, Item等の分類
    public bool isTargetEnemy;      // 敵対象ならtrue
}

// 行動決定後に保持するデータクラス
public class BattleAction
{
    public UnitController actor;    //  行動者
    public UnitController target;   //  対象
    public SkillData skill;　　　　　//  スキル

    // 行動速度（基本はキャラのSpeed + 乱数など）
    public int speedPriority;
}