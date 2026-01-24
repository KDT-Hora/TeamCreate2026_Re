/*using UnityEngine;
using Data;

public class GameInitializer : MonoBehaviour
{
    [Header("生成設定")]
    public GameObject playerPrefab;
    public int initialMemberCount = 3;
    void Awake()
    {
        if (DataManager.Instance == null)
        {
            Debug.LogWarning("GameInitializer: DataManagerが見つかりません。実行順序を確認してください。");
            return;
        }

        if (DataManager.Instance.partyMembers.members.Count > 0) return;
        CreateInitialParty();
    }

    void CreateInitialParty()
    {
        int level = 1;

        for (int i = 0; i < initialMemberCount; i++)
        {
            int dataId = i;

            Player newPlayer = PlayerFactory.CreatePlayer(dataId, level, playerPrefab);

            if (newPlayer != null)
            {
                newPlayer.name = newPlayer.GetName();
                newPlayer.transform.localPosition = Vector3.zero;

                // ★作ったプレイヤーをDataManagerに預ける
                DataManager.Instance.AddPlayerToParty(newPlayer);
            }
        }
        Debug.Log("初期パーティ生成完了");
    }
}*/