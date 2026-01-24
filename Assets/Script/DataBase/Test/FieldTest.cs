using Data;
using JetBrains.Annotations;
using System;
using UnityEngine;
using UnityEngine.SceneManagement; // これが必要

public class FieldTest : MonoBehaviour
{
    // エンカウントした想定
    [Header("テスト用：次に戦う敵の設定")]
    public int encounterEnemyID = 0;
    public int encounterEnemyLevel = 5;
    void Update()
    {
        // スペースキーを押したらバトルシーンへ移動
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (DataManager.Instance != null)
            {
                DataManager.Instance.nextEnemyID = encounterEnemyID;
                DataManager.Instance.nextEnemyLevel = encounterEnemyLevel;
                Debug.Log($"戦闘開始設定: ID {encounterEnemyID}, Lv {encounterEnemyLevel} -> シーン移動");
                FadeManager.FadeChangeScene("SampleScene", 1.0f);
            }
        }
        // テスト用でダメージ与える
        if (Input.GetKeyDown(KeyCode.D))
        {
            if(DataManager.Instance != null && DataManager.Instance.currentParty.members.Count > 0)
            {
                Player leader = DataManager.Instance.currentParty.members[0];
                Debug.Log($"減少前HP {leader.GetStatusRuntime().hp}");
                leader.TakeDamage(10);
                Debug.Log($"減少後HP {leader.GetStatusRuntime().hp}");
            }
        }

    }

    void Start()
    {
        if (DataManager.Instance != null)
        {

        }
    }
}