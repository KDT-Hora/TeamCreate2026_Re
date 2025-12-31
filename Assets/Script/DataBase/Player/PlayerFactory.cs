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
            // ID検索
            StatusBase chara = listData.playerDatas.Find(c => c.id == id);
            // かばい処理生成
            ProtectSystem protectSystem = ProtectSystemFactory.Create(chara.id);
            // Player生成
            return new Player(Object.Instantiate(prefab), chara, level, protectSystem);
        }
    }
}