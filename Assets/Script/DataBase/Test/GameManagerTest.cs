using Data;
using UnityEngine;

public class GameManagerTest : MonoBehaviour
{
    [Header("設定")]
    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    public float spacing = 2.0f;
    void Start()
    {
        SpawnPlayers(); // 複数出す関数に変更
        SpawnEnemies();
    }

    // プレイヤーを複数生成する
    void SpawnPlayers()
    {
        int count = GameData.playerCount;
        float startX = -((count - 1) * spacing) / 2.0f;
        
        for (int i = 0; i < count; i++)
        {
            float x = startX + (i * spacing);
            Vector3 pos = new Vector3(x, 1, 0);
            // 生成した瞬間のオブジェクトを変数に入れる
            GameObject obj = Instantiate(playerPrefab, pos, Quaternion.identity);
            // UnitControllerを取得してIDを渡す
            UnitController unit = obj.GetComponent<UnitController>();
            if (unit != null)
            {
                unit.unitID = i;
                obj.name = "Player_" + i;
            }
        }
    }

    // エネミーを複数生成する
    void SpawnEnemies()
    {
        int count = GameData.enemyCount;
        float startX = -((count - 1) * spacing) / 2.0f;

        for (int i = 0; i < count; i++)
        {
            float x = startX + (i * spacing);
            Vector3 pos = new Vector3(x, 1, 3);
            // 生成した瞬間のオブジェクトを変数に入れる
            GameObject obj = Instantiate(enemyPrefab, pos, Quaternion.identity);
            // UnitControllerを取得してIDを渡す
            UnitController unit = obj.GetComponent<UnitController>();
            if (unit != null)
            {
                unit.unitID = i;
                obj.name = "Enemy_" + i;
            }
        }
    }
}
