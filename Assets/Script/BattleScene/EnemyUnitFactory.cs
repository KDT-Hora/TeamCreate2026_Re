using Data;
using System.Collections.Generic;
using UnityEngine;
using static SelectData;

public class EnemyUnitFactory : MonoBehaviour
{
    [Header("Prefubs")]
    public List<GameObject> enemyUnitPrefubs;
    public List<GameObject> BossUnitPrefubs;
    public Transform spownPos;
    public float offsetX;
    public float offsetZ;

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
    public UnitController SpownBossEnemies(int bossID,int count)
    {
        Debug.Log("ボス生成処理開始");

        UnitController bossController = null;

        //  ボスIDに応じたボスのプレハブを選択して生成
        int id = DataManager.Instance.currentBossID;
        if (id >= 0 && id < BossUnitPrefubs.Count)
        {
            //  リストからステータス取得
            EnemyData listData = Resources.Load<EnemyData>("EnemyData");
            if(listData == null)
            {
                Debug.LogError("EnemyDataの読み込みに失敗");
                return null;
            }
            //  ID検索
            StatusBase chara = listData.enemyDatas.Find(c => c.id == bossID);
            if (chara == null)
            {
                Debug.LogError("指定されたIDのボスデータが存在しません: " + bossID);
                return null;
            }
            //  ボスのプレハブを生成
            GameObject bossPrefab = BossUnitPrefubs[id];
            //  生成位置の計算
            Vector3 spawnPosition = spownPos.position + 
                new Vector3(offsetX * count, 0, offsetZ * count);
            var rotate = bossPrefab.transform.rotation;
            bossPrefab = Instantiate(bossPrefab, spawnPosition, rotate);

            Player status = new Player();
            status.Initialize(chara, 1, null);

            //  ボスのプレハブから、UnitControllerの取得
            bossController = bossPrefab.GetComponent<UnitController>();
            bossController.UnitInit(status);

            Debug.Log("ボス生成処理完了: " + bossPrefab.name);
        }

        return bossController;
    }

    //  通常敵の生成処理
    public UnitController SpownNormalEnemies(int count)         
    {
        Debug.Log("敵生成処理開始");

        UnitController enemyController = null;

        //  敵のIDをランダムで決定
        int enemyid = Random.Range(0, enemyUnitPrefubs.Count);

        //  IDに応じたボスのプレハブを選択して生成
        if (enemyid >= 0 && enemyid < enemyUnitPrefubs.Count)
        {
            //  プレハブを生成
            GameObject enemyPrefab = enemyUnitPrefubs[enemyid];

            //  IDがズレているので調整
            enemyid += 3;

            Debug.Log("選択された敵ID: " + enemyid);

            //  リストからステータス取得
            EnemyData listData = Resources.Load<EnemyData>("EnemyData");
            Debug.Log("生成するキャラクター名：" + listData.enemyDatas[enemyid].name);
            if (listData == null)
            {
                Debug.LogError("EnemyDataの読み込みに失敗");
                return null;
            }
            //  ID検索
            StatusBase chara = listData.enemyDatas.Find(c => c.id == enemyid);
            if (chara == null)
            {
                Debug.LogError("指定されたIDのボスデータが存在しません: " + enemyid);
                return null;
            }

            //  生成位置の計算
            Vector3 spawnPosition = spownPos.position +
                new Vector3(offsetX * count, 0.3f, offsetZ * count);
            var rotate = enemyPrefab.transform.rotation;
            enemyPrefab = Instantiate(enemyPrefab, spawnPosition, rotate);

            Player status = new Player();
            status.Initialize(chara, 1, null);

            //  ボスのプレハブから、UnitControllerの取得
            Debug.Log("プレハブ取得");
            enemyController = enemyPrefab.GetComponent<UnitController>();
            Debug.Log("UnitInit開始");
            enemyController.UnitInit(status);

            Debug.Log("敵生成処理完了: " + enemyPrefab.name);
        }

        return enemyController;

    }
}
