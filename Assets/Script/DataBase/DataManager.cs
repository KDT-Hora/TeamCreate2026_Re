using System.Collections.Generic;
using UnityEngine;
using Data;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; }

    // パーティーリスト
    public List<Player> partyMembers = new List<Player>();

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
        if (partyMembers.Count == 0 && testPlayerPrefab != null)
        {
            SetupParty(3, testPlayerPrefab);
        }
    }

    // パーティ生成
    public void SetupParty(int memberCount, GameObject prefab)
    {
        if (partyMembers.Count > 0) return;

        int level = 1;

        for (int i = 0; i < memberCount; i++)
        {
            int dataId = i;

            Player newPlayer = PlayerFactory.CreatePlayer(dataId, level, prefab);
            if (newPlayer == null) continue;

            newPlayer.transform.SetParent(transform);
            newPlayer.name = newPlayer.GetName();
            newPlayer.transform.localPosition = Vector3.zero;

            partyMembers.Add(newPlayer);
        }

        Debug.Log($"パーティ生成完了: {partyMembers.Count}人");
    }

    // パーティ配置
    public void SetPartyPosition(Vector3 centerPos, float spacing, float teamDistance)
    {
        int count = partyMembers.Count;
        float startZ = -((count - 1) * spacing) / 2.0f;

        for (int i = 0; i < count; i++)
        {
            Player player = partyMembers[i];
            float z = startZ + (i * spacing);

            player.transform.position = centerPos + new Vector3(teamDistance, 1, z);

            // ★追加: 死んでいるなら倒す（寝かせる）
            if (player.IsDead())
            {
                player.transform.rotation = Quaternion.Euler(0, -90, 90);
            }
            else
            {
                player.transform.rotation = Quaternion.Euler(0, -90, 0);
            }
        }
    }

    // ★追加: 全滅判定（フィールドなどで使う用）
    public bool IsAllDead()
    {
        foreach (var member in partyMembers)
        {
            if (!member.IsDead()) return false;
        }
        return true;
    }
}