using Data;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data {
    /// <summary>
    /// キャラクター全般の基本ステータス
    /// レベル１時点での基礎値
    [System.Serializable]
    public class CharaDatas
    {
        public int id;      // ID
        public string name; // 名前
        
        public int basehp;      // 基礎HP
        public int baseAtk;     // 攻撃力
        public int baseDef;     // 防御力
        public int baseMatk;    // 魔法攻撃力
        public int baseMdef;    // 魔法防御力
        public int baseMp;      // 魔力
        public int baseSpeed;   // 速さ
    
        public ElementType element; // 属性
    }
}