using NUnit.Framework;
using UnityEngine;

namespace Data
{
    // 属性
    public enum ElementType
    {
        Water,  // 水
        Fire,   // 炎
        Flower  // 花
    }
    // キャラクター全般のステータス
    public class StatusBase
    {
        public int id;
        public string name;

        public int atk;
        public int def;
        public int matk;
        public int mdef;
        public int mp;
        public int speed;

        public ElementType element;
    }
}