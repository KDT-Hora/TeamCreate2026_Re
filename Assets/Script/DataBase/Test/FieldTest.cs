using Data;
using JetBrains.Annotations;
using System;
using UnityEngine;
using UnityEngine.SceneManagement; // これが必要

public class FieldTest : MonoBehaviour
{
    void Update()
    {
        // スペースキーを押したらバトルシーンへ移動
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (PartyManager.Instance != null)
            {
                Debug.Log("サンプルシーンに移行");
                FadeManager.FadeChangeScene("SampleScene", 1.0f);
            }
        }
        // テスト用でダメージ与える
        if (Input.GetKeyDown(KeyCode.D))
        {
            if(PartyManager.Instance != null && PartyManager.Instance.partyMembers.Count > 0)
            {
                Player leader = PartyManager.Instance.partyMembers[0];
                Debug.Log($"減少前HP {leader.GetStatusRuntime().hp}");
                leader.TakeDamage(10);
                Debug.Log($"減少後HP {leader.GetStatusRuntime().hp}");
            }
        }

    }

    void Start()
    {
        if (PartyManager.Instance != null)
        {

        }
    }
}