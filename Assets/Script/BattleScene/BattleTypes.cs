using UnityEngine;

public enum ActionType 
{
    Attack, 
    Defend, 
    Avirity, 
    Heal,
    Buff,
    Debuff,
    Cover, 
    Item 
}
public enum TargetSide { Player, Enemy }
public enum  Element
{
    None,       //  無
    Fire,       //  炎
    Water,      //  水
    Grass,      //  草
}

public enum targetType
{
    Single,
    All,
    Random,
}

[System.Serializable]
public class SkillData
{
    public string skillName;
    public int power;               // ダメージや回復量
    public int hateIncrease;        // 使用時のヘイト上昇量
    public ActionType type;         // Skill, Cover, Item等の分類
    public Element element;         // 属性
    public targetType targetType;   // 単体、全体、ランダム
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