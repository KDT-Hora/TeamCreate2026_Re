/*using Data;
using System.Collections.Generic;
using UnityEngine;

public class BattleFormation : MonoBehaviour
{
    [Header("配置設定")]
    public float spacing = 2.0f;
    public float teamDistance = 5.0f;
    public float height = 1.0f;

    public void ArrangeParty()
    {
        if (DataManager.Instance == null) return;

        var partyMembers = DataManager.Instance.currentParty.members;
        List<Player> aliveMembers = new List<Player>();

        foreach (var p in partyMembers)
        {
            if (p.IsDead())
            {
                p.gameObject.SetActive(false);
            }
            else
            {
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

            Debug.Log($"[{i}] {player.name} を Z:{z} に配置します (Spacing値: {spacing})");

            player.transform.position = new Vector3(teamDistance, height, z);
            *//*            Vector3 targetPos = new Vector3(-teamDistance, 1, z);
                        player.transform.position = targetPos;*//*
            player.transform.rotation = Quaternion.Euler(0, -90, 0);
        }
    }
}*/