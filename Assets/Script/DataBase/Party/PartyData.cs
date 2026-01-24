using System.Collections.Generic;
using UnityEngine;
using Data;

[System.Serializable] // これをつけるとインスペクターで見えるようになる
public class PartyData
{
    // パーティメンバーのリスト
    public List<Player> members = new List<Player>();

    /// <summary>
    /// 全滅しているかチェック
    /// </summary>
    public bool IsAllDead()
    {
        if (members.Count == 0) return false;

        foreach (var member in members)
        {
            if (!member.IsDead()) return false;
        }
        return true;
    }

}