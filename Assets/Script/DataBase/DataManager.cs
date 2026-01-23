using System.Collections.Generic;
using UnityEngine;
using Data;
using Unity.VisualScripting;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; }

    // パーティーリスト
    public PartyData currentParty;

    // テスト生成用
    [Header("テスト設定")]
    public GameObject testPlayerPrefab;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // ★追加: テスト用に自動生成する機能（これがないとHierarchyで矢印が出ません）
    void Start()
    {
        if (currentParty.members.Count == 0 && testPlayerPrefab != null)
        {
            SetupParty(3, testPlayerPrefab);
        }
    }

    // パーティ生成
    public void SetupParty(int memberCount, GameObject prefab)
    {
        if (currentParty.members.Count > 0) return;

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

    // パーティ配置
    public void SetPartyPosition(Vector3 centerPos, float spacing, float teamDistance)
    {
        List<Player> aliveMembers = new List<Player>();

        foreach (var p in currentParty.members)
        {
            if (p.IsDead())
            {
                // 死んでるキャラは非表示にして、配置リストに入れない
                p.gameObject.SetActive(false);
            }
            else
            {
                // 生きてるキャラは表示する
                p.gameObject.SetActive(true);
                aliveMembers.Add(p);
            }
        }

        // ここから下は「aliveMembers（生きてる人）」だけで配置計算
        int count = aliveMembers.Count;
        float startZ = -((count - 1) * spacing) / 2.0f;

        for (int i = 0; i < count; i++)
        {
            Player player = aliveMembers[i];
            float z = startZ + (i * spacing);

            player.transform.position = centerPos + new Vector3(teamDistance, 1, z);
            player.transform.rotation = Quaternion.Euler(0, -90, 0);
        }
    }

    // ★追加: 全滅判定（フィールドなどで使う用）
    public bool IsAllDead()
    {
        foreach (var member in currentParty.members)
        {
            if (!member.IsDead()) return false;
        }
        return true;
    }
}