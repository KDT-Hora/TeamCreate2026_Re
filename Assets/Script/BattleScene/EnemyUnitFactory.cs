using UnityEngine;
using System.Collections.Generic; 

public class EnemyUnitFactory : MonoBehaviour
{
    [Header("Prefubs")]
    public List<GameObject> enemyUnitPrefubs;
    public List<GameObject> BossUnitPrefubs;
    public Transform spownPos;
    public float offsetX;
    public float offsetY;

    [Header("EnemyUI")]
    public Transform enemyUIParent;
    public GameObject enemyUIPrefub;
    


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //  ボスの生成処理
    public void SpownBossEnemies(int bossID)
    {

    }

    //  通常敵の生成処理
    public void SpownNormalEnemies()         
    {
        Debug.Log("敵生成処理開始");

        int enemyCount = Random.Range(1, 4); // 1～3体の敵を生成
        Debug.Log($"生成する敵の数: {enemyCount}");

        for (int i = 0; i < enemyCount; i++)
        {
            Debug.Log($"敵生成処理: {i + 1}体目");

            // ランダムに敵のプレハブを選択
            int randomIndex = Random.Range(0, enemyUnitPrefubs.Count);
            Debug.Log($"選択された敵のインデックス: {randomIndex}");
            GameObject enemyPrefab = enemyUnitPrefubs[randomIndex];
            // 敵の生成位置を計算
            Vector3 spawnPosition = spownPos.position + new Vector3(i * offsetX, 0, i * offsetY);
            // 敵を生成
            GameObject enemyInstance = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            // 敵UIの生成
            GameObject enemyUIInstance = Instantiate(enemyUIPrefub, enemyUIParent);

            // 敵UIの初期化（例: 名前やHPの設定）
            UnitController enemyController = enemyInstance.GetComponent<UnitController>();
            if (enemyController != null)
            {
                enemyController.unitName = enemyPrefab.name; // 敵の名前を設定
                //  ステータスの初期化
            }
        }
    }
}
