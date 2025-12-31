using Data;
using UnityEngine;

public class GameManagerTest : MonoBehaviour
{
    // Inspectorからプレハブをセットする
    [SerializeField] private GameObject m_playerPrefab;
    public int playerCount = 3;
    [SerializeField] private GameObject m_enemyPrefab;
    public int enemyCount = 3;
    public float spacing = 2.0f; // 隣との間隔
    void Start()
    {
        SpawnPlayers(); // 複数出す関数に変更
        SpawnEnemies();
    }

    // プレイヤーを複数生成する
    void SpawnPlayers()
    {
        for (int i = 0; i < playerCount; i++)
        {
            float startX = -((playerCount - 1) * spacing) / 2.0f;
            float x = startX + (i * spacing);

            Vector3 pos = new Vector3(x, 1, 0);
            Instantiate(m_playerPrefab, pos, Quaternion.identity);
        }
    }

    // エネミーを複数生成する
    void SpawnEnemies()
    {
        for (int i = 0; i < enemyCount; i++)
        {
            float startX = -((enemyCount - 1) * spacing) / 2.0f;
            float x = startX + (i * spacing);

            Vector3 pos = new Vector3(x, 1, 3);
            Instantiate(m_enemyPrefab, pos, Quaternion.identity);
        }
    }
}
