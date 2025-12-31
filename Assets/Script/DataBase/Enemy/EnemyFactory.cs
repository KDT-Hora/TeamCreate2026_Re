using Data;
using UnityEngine;

namespace Data
{
    public class BattleEnemyFactory
    {
        public static Enemy CreateEnemy(int id, int level, GameObject prefab)
        {
            // リストデータからStatusBaseを取得
            EnemyData listData = Resources.Load<EnemyData>("EnemyData");
            // ID検索
            StatusBase chara = listData.enemyDatas.Find(c => c.id == id);
            // Enemy生成
            return new Enemy(Object.Instantiate(prefab), chara, level);
        }
    }
}