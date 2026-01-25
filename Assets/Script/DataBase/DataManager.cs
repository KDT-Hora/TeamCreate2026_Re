using System.Collections.Generic;
using UnityEngine;
using Data;
using Unity.VisualScripting;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; }

    // パーティーリスト
    public PartyData currentParty;

    [Header("基本設定")]
    public GameObject PlayerPrefab;
    public GameObject EnemyPrefab;
    [Header("配置設定")]
    public float spacing = 2.0f;
    public float teamDistance = 5.0f;
    public float height = 1.0f;
    [Header("次回のバトル設定（シーン遷移用）")]
    public int nextEnemyID = 0;
    public int nextEnemyLevel = 1;
    public bool isBossBattle = false;   //  ボス戦かどうかの判定フラグ
    public int currentBossID = -1;      //  現在のボスのID

    private void Awake()
    {
        Debug.Log("DataManager Awake");

        //  シングルトンパターン
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        //  自身の参照を設定
        Instance = this;
        //  シーン切り替え時に破棄しない
        DontDestroyOnLoad(gameObject);

        Debug.Log("DataManager Awake Initialized");

        //  パーティーデータに初期値を設定
        if (currentParty == null) currentParty = new PartyData();

        //  
        
    }

    void Start()
    {
        Debug.Log("DataManager Start");

        // パーティが空なら生成しておく
        if (currentParty.members.Count == 0 && PlayerPrefab != null)
        {
            Debug.Log("パーティが空なので生成します");
            SetupParty(3, PlayerPrefab);
        }
        // いったん全員非表示にしとく
        foreach (var member in currentParty.members)
        {
            member.gameObject.SetActive(false);
        }

        /*        if (PlayerPrefab != null && EnemyPrefab != null)
                {
                    StartBattle(0, 1);
                }*/
    }
    // バトル開始用
    // 一応レベルも入れとくね
    /*public void StartBattle(int targetEnemyID, int enemyLevel)
    {
        SetupParty(3, PlayerPrefab);
        SpawnEnemies(targetEnemyID, enemyLevel);
    }*/

    //  バトル開始用
    //  キャラクターの
    //  もう読んでないよー( ´∀｀ 
    public void StartBattle(int targetEnemyID, int enemyLevel)
    {

        ArrangeUnits(currentParty.members, true);

   //     SpawnEnemies(targetEnemyID, enemyLevel);
    }

    // パーティ生成
    public void SetupParty(int memberCount, GameObject prefab)
    {
        if (currentParty.members.Count == 0)
        {
            int level = 1;
            for (int i = 0; i < memberCount; i++)
            {
                int dataId = i;

                Player newPlayer = PlayerFactory.CreatePlayer(dataId, level, prefab);
                if (newPlayer == null) continue;

                newPlayer.transform.SetParent(transform);
                newPlayer.name = newPlayer.GetName();
                newPlayer.transform.localPosition = Vector3.zero;

                currentParty.members.Add(newPlayer);
            }
    
            Debug.Log($"パーティ生成完了: {currentParty.members.Count}人");
        }
        ArrangeUnits(currentParty.members, true);
    }

    // パーティ配置
    /*public void SetPartyPosition(Vector3 centerPos, float spacing, float teamDistance)
    {
        List<Player> aliveMembers = new List<Player>();

        foreach (var p in currentParty.members)
        {
            if (p.IsDead())
            {
                // 死んでるキャラは非表示
                p.gameObject.SetActive(false);
            }
            else
            {
                // 生きてるキャラは表示
                p.gameObject.SetActive(true);
                aliveMembers.Add(p);
            }
        }

        int count = aliveMembers.Count;
        float startZ = -((count - 1) * spacing) / 2.0f;

        for (int i = 0; i < count; i++)
        {
            Player player = aliveMembers[i];
            float z = startZ + (i * spacing);

            player.transform.position = centerPos + new Vector3(teamDistance, 1, z);
            player.transform.rotation = Quaternion.Euler(0, -90, 0);
        }
    }*/

    // 敵生成
    //public void SpawnEnemies(int targetID, int baseLevel)
    //{
    //    List<Enemy> enemiesToSpawn = new List<Enemy>();
        
    //    Enemy mainEnemy = EnemyFactory.CreateEnemy(targetID, baseLevel, EnemyPrefab);
    //    if (mainEnemy != null)
    //    {
    //        mainEnemy.name += $" Lv.{baseLevel}";
    //        enemiesToSpawn.Add(mainEnemy);
    //    }

    //    bool isBoss = bossIDs.Contains(targetID);

    //    if (!isBoss)
    //    {
    //        // いったんランダムやけどあとで調整してね
    //        int additionalCount = Random.Range(0, 3);
    //        for (int i = 0; i < additionalCount; i++)
    //        {
    //            int randomID = Random.Range(0, 3);

    //            int minionLevel = Mathf.Max(1, baseLevel - Random.Range(0, 2));

    //            Enemy mob = EnemyFactory.CreateEnemy(randomID, minionLevel, EnemyPrefab);
    //            if (mob != null)
    //            {
    //                mob.name += $" Lv.{minionLevel}";
    //                enemiesToSpawn.Add(mob);
    //            }
    //        }
    //    }

    //    foreach (var enemy in enemiesToSpawn)
    //    {
    //        enemy.transform.SetParent(transform);
    //    }

    //    ArrangeUnits(enemiesToSpawn, false);
    //}
    // ユニット配置共通処理
    private void ArrangeUnits<T>(List<T> units, bool isPlayerSide) where T : MonoBehaviour
    {
        List<T> activeUnits = new List<T>();
        foreach (var unit in units)
        {
            if (unit is Player p)
            {
                if (p.IsDead())
                {
                    p.gameObject.SetActive(false);
                    continue;
                }
                else
                {
                    p.gameObject.SetActive(true);
                }
            }
            activeUnits.Add(unit);
        }

        int count = activeUnits.Count;
        float startZ = -((count - 1) * spacing) / 2.0f;
        float xPos = isPlayerSide ? teamDistance : -teamDistance;
        float yRot = isPlayerSide ? -90f : 90f;

        for (int i = 0; i < count; i++)
        {
            var unit = activeUnits[i];
            float z = startZ + (i * spacing);

            unit.transform.position = new Vector3(xPos, height, z);
            unit.transform.rotation = Quaternion.Euler(0, yRot, 0);
        }
    }

    // 全滅判定（フィールドなどで使う用）
    public bool IsAllDead()
    {
        foreach (var member in currentParty.members)
        {
            if (!member.IsDead()) return false;
        }
        return true;
    }

    // 初期化
    public void ResetData()
    {
        currentParty = new PartyData();
    }
}