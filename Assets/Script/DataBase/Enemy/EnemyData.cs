using Data;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 敵用データリスト
[CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Objects/EnemyData")]
public class EnemyData : ScriptableObject
{
    public List<StatusBase> enemyDatas;
}