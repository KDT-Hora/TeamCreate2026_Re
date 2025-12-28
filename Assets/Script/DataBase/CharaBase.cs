using UnityEngine;

namespace Data
{
    public class CharaBase
    {
        // 基本ステータスを入れる用
        private Data.StatusBase Status;


        // 基本ステータスの取得
        public Data.StatusBase GetStatus() { return Status; }
    }
}