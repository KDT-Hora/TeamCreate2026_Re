using System.Collections.Generic;
using Data;
using UnityEngine;

public class PartyManager : MonoBehaviour
{
    // シングルトン
    public static PartyManager Instance { get; private set; }

    // パーティーリスト
    public List<Player> partyMembers = new List<Player>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // データ消しちゃだめよだめだめ～
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            // すでに存在する場合は新しい方削除して守るンゴ
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// パーティ生成
    /// </summary>
    public void SetupParty(int memberCount, GameObject prefab)
    {
        // すでにメンバーがいるなら生成しない（データ保持の役割）
        if (partyMembers.Count > 0) return;

        int level = 1;

        for (int i = 0; i < memberCount; i++)
        {
            // Factoryで生成
            Player newPlayer = PlayerFactory.CreatePlayer(i, level, prefab);

            if (newPlayer != null)
            {
                newPlayer.transform.SetParent(this.transform);

                // リストに追加
                partyMembers.Add(newPlayer);

                // 名前設定
                newPlayer.name = newPlayer.GetName();

                // 原点
                newPlayer.transform.localPosition = Vector3.zero;
            }
        }
        Debug.Log($"パーティ生成完了: {partyMembers.Count}人");
    }

    /// <summary>
    /// バトル開始時に移動
    /// </summary>
    public void SetPartyPosition(Vector3 centerPos, float spacing, float teamDistance)
    {
        int count = partyMembers.Count;
        float startZ = -((count - 1) * spacing) / 2.0f;

        for (int i = 0; i < count; i++)
        {
            Player player = partyMembers[i];

            // 配置計算
            float z = startZ + (i * spacing);
            Vector3 targetPos = centerPos + new Vector3(teamDistance, 1, z);

            // ワールド座標で移動
            player.transform.position = targetPos;

            // 向きを左（バトル用）に向ける
            player.transform.rotation = Quaternion.Euler(0, -90, 0);
        }
    }
}