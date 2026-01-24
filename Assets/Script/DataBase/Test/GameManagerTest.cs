using Data;
using UnityEngine;
//using static UnityEditor.Experimental.GraphView.GraphView;

public class GameManagerTest : MonoBehaviour
{
    // バトル配置の管理
//    public BattleFormation battleFormation;

/*    [Header("設定")]
    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    [Header("配置設定")]
    public float spacing = 2.0f;      // 隣との間隔
    public float teamDistance = 5.0f; // 中心からの距離（X座標）*/
    void Start()
    {
/*        if (battleFormation != null)
        {
            battleFormation.ArrangeParty();
        }
        else
        {
            Debug.LogError("BattleFormation がセットされていません！Inspectorを確認してください。");
        }*/

        if (DataManager.Instance != null)
        {
            int id = DataManager.Instance.nextEnemyID;
            int lv = DataManager.Instance.nextEnemyLevel;

            DataManager.Instance.StartBattle(id, lv);
            /*            int count = GameData.playerCount;
                        DataManager.Instance.SetupParty(count, playerPrefab);
                        DataManager.Instance.SetPartyPosition(Vector3.zero, spacing, teamDistance);*/
            //DataManager.Instance.StartBattle(0,2);
            foreach (var player in DataManager.Instance.currentParty.members)
            {
                string status = player.IsDead() ? "[死亡]" : "[生存]";
                Debug.Log($"パーティ情報: {player.name} HP:{player.GetStatusRuntime().hp} {status}");
            }
        }
        else
        {
            Debug.LogError("PartyManagerがシーンにない");
        }

/*        if (DataManager.Instance != null)
        {
            foreach (var player in DataManager.Instance.currentParty.members)
            {
                string status = player.IsDead() ? "[死亡]" : "[生存]";
                Debug.Log($"バトル開始: {player.name} HP:{player.GetStatusRuntime().hp} {status}");
            }
        }*/

        // SpawnPlayers();
        // SpawnEnemies();
        Debug.Log("生成");
    }

    // プレイヤーを複数生成する
/*    void SpawnPlayers()
    {
        int count = GameData.playerCount;
        int level = 1;
        float startZ = -((count - 1) * spacing) / 2.0f;

        for (int i = 0; i < count; i++)
        {
            // プレイヤーの生成
            Player player = PlayerFactory.CreatePlayer(i, level, playerPrefab);
            if (player != null) {
                float z = startZ + (i * spacing);
                player.transform.position = new Vector3(teamDistance, 1, z);
                player.transform.rotation = Quaternion.Euler(0, -90, 0);
                // 生成した瞬間のオブジェクトを変数に入れる
//                GameObject obj = Instantiate(playerPrefab, pos, Quaternion.identity);
                // UnitControllerを取得してIDを渡す
//                UnitController unit = obj.GetComponent<UnitController>();
//                if (unit != null)
//                {
//                    unit.unitID = i;
                player.name = player.GetName();
//                }
            }
        }
    }*/

    // エネミーを複数生成する
   /* void SpawnEnemies()
    {
        int count = GameData.enemyCount;
        int level = 1;
        float startZ = -((count - 1) * spacing) / 2.0f;

        for (int i = 0; i < count; i++)
        {
            Enemy enemy = EnemyFactory.CreateEnemy(i, level, enemyPrefab);
            if (enemy != null) {
                float z = startZ + (i * spacing);
                enemy.transform.position = new Vector3(-teamDistance, 1, z);
                enemy.transform.rotation = Quaternion.Euler(0, 90, 0);
                // 生成した瞬間のオブジェクトを変数に入れる
//                GameObject obj = Instantiate(enemyPrefab, pos, Quaternion.identity);
                // UnitControllerを取得してIDを渡す
//                UnitController unit = obj.GetComponent<UnitController>();
//                if (unit != null)
//                {
//                    unit.unitID = i;
                    enemy.name = enemy.GetName();
//                }
            }
        }
    }*/
}
