using JetBrains.Annotations;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    /// <summary>
    /// 属性 0:水 1:炎 2:花
    /// </summary>
    public enum ElementType
    {
        Water,  // 水
        Fire,   // 炎
        Flower  // 花
    }
    /// <summary>
    /// キャラクター全般の基本ステータス
    /// レベル１時点での基礎値
    [System.Serializable]
    public class StatusBase
    {
        public int id;      // ID
        public string name; // 名前

        public int baseHp;      // 基礎HP
        public int baseMp;      // 魔力
        public int baseAtk;     // 攻撃力
        public int baseDef;     // 防御力
        public int baseMatk;    // 魔法攻撃力
        public int baseMdef;    // 魔法防御力
        public int baseSpeed;   // 速度

        public ElementType element; // 属性
    }
    /// <summary>
    /// レベル換算後ステータス
    /// </summary>
    public class StatusCalculated
    {
        public int maxHp;   // 最大HP
        public int maxMp;   // 魔力
        public int atk;     // 攻撃力
        public int def;     // 防御力
        public int matk;    // 魔法攻撃力
        public int mdef;    // 魔法防御力
        public int speed;   // 速度
    }
    /// <summary>
    /// 値を変化させる用のステータス
    /// </summary>
    public class StatusRuntime
    {
        public int hp;      // hp
        public int mp;      // 魔力
        public int atk;     // 攻撃力
        public int def;     // 防御力        
        public int matk;    // 魔法攻撃力        
        public int mdef;    // 魔法防御力
        public int speed;   // 速度
    }
}