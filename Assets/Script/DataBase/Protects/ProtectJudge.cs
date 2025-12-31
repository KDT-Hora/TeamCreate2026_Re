using Data;
using System;
using System.Diagnostics;
using UnityEngine;

namespace Protect
{
    // 庇い判定の確認システム
    // ジャッジの判定はGameManager？でする
    public class ProtectJudge {

        // 庇う人と庇われている人の情報を保持
        private CharaBase Protector;   // 庇う人
        private CharaBase Target;      // 庇われている人を取得

        /// <summary>
        /// 庇い情報セット
        /// </summary>
        /// <param name="protector">庇う人</param>
        /// <param name="target">庇われる人</param>
        public void SetProtect(CharaBase protector, CharaBase target) {
            Protector = protector;
            Target = target;
        }

        /// <summary>
        /// 庇う人を取得
        /// もし庇っていない場合 null を返すので要注意！！
        /// </summary>
        public CharaBase GetProtector() {
            return Protector;
        }
        /// <summary>
        /// 庇われている人を取得
        /// もし庇われていない場合 null を返すので要注意！！
        /// </summary>
        public CharaBase GetTarget() {
            return Target;
        }

        /// <summary>
        /// 庇われているか判定
        /// </summary>
        /// <param name="target">攻撃されたキャラが庇われているかの判定</param>
        /// <returns>true:庇われてる false:庇われていない</returns>
        public bool IsProtect(CharaBase target) {
            if (Target == target) {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 庇い情報クリア
        /// null チェックを行ってからクリアする
        /// </summary>
        public void ClearProtect() {
            SetProtect(null, null); // クリア
        }   
    }
}