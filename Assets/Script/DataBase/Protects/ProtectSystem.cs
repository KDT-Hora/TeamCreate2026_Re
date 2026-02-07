using Data;
using UnityEngine;

namespace Protect
{
    // 庇い処理の基底クラス
    public class ProtectSystem {
        /// <summary>
        /// 庇い処理実行メソッド
        /// </summary>
        /// <param name="protector">庇う人</param>
        /// <param name="target">庇われている人</param>
        /// <param name="Attacker">攻撃してきた相手</param>
        public virtual void ExecuteProtect(
            UnitController protector,
            BattleAction action
        ) 
        {
            //  攻撃対象を変更をデフォルトの仕組みに
            action.target = protector;
        }
    }
}