using Protect;
using UnityEngine;
namespace Data
{
    public class PlayerFactory
    {
        public static Player CreatePlayer(int id, int level, GameObject prefab)
        {
            // リストデータからStatusBaseを取得
            PlayerData listData = Resources.Load<PlayerData>("PlayerData");
            if (listData == null)
            {
                Debug.LogError("PlayerDataが見つかりません。Resourcesフォルダを確認してください。");
                return null;
            }
            // ID検索
            StatusBase chara = listData.playerDatas.Find(c => c.id == id);
            if (chara == null)
            {
                Debug.LogError($"ID: {id} のキャラクターデータが見つかりません。");
                return null;
            }
            // かばい処理生成
            ProtectSystem protectSystem = ProtectSystemFactory.Create(chara.id);
            // オブジェクトの動的生成します
            GameObject instance = Object.Instantiate(prefab);
            Player playerScript = instance.GetComponent<Player>();
            if (playerScript == null)
            {
                playerScript = instance.AddComponent<Player>();
            }
            // Player生成
            playerScript.Initialize(chara, level, protectSystem);
            Debug.Log("プレイヤー生成");
            return playerScript;
        }
    }
}