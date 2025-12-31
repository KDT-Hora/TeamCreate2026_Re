using Data;
using UnityEngine;
namespace Protect
{
    // 庇い処理 カウンタークラス
    public class ProtectCounter : ProtectSystem {
        public override void ExecuteProtect(
            CharaBase protector,
            CharaBase target,
            CharaBase attacker
        ) {
            // カウンター処理実装
            attacker.TakeDamage(protector.GetStatusRuntime().atk);
        }
    }
}