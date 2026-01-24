using Data;
using UnityEngine;

namespace Data
{
    public class EnemyFactory
    {
        public static Enemy CreateEnemy(int id, int level, GameObject prefab)
        {
            // リストデータからStatusBaseを取得
            EnemyData listData = Resources.Load<EnemyData>("EnemyData");
            if (listData == null)
            {
                Debug.LogError("EnemyDataが見つかりません。Resourcesフォルダを確認してください。");
                return null;
            }
            // ID検索
            StatusBase chara = listData.enemyDatas.Find(c => c.id == id);
            if (chara == null)
            {
                Debug.LogError($"ID: {id} のキャラクターデータが見つかりません。");
                return null;
            }
            // オブジェクトの動的生成します
            GameObject instance = Object.Instantiate(prefab);
            Enemy enemyScript = instance.GetComponent<Enemy>();
            if (enemyScript == null)
            {
                enemyScript = instance.AddComponent<Enemy>();
            }
            // Enemy生成
            enemyScript.Initialize(chara, level);
            Debug.Log($"敵生成ID: {id}");
            return enemyScript;
        }
    }
}