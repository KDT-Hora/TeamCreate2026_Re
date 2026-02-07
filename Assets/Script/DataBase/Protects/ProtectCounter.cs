using Data;
using UnityEngine;
namespace Protect
{
    // 庇い処理 カウンタークラス
    public class ProtectCounter : ProtectSystem {
        public override void ExecuteProtect(
            UnitController protector,
            BattleAction action
        ) {
            // カウンター処理実装
        //    attacker.TakeDamage(protector.GetStatusRuntime().atk);
            action.actor.TakeDamage(protector.GetUnitData().GetStatusRuntime().atk);
        }
    }
}