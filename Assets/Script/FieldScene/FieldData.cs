using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class EnemyStatus
{
    public string enemyName;
    public Vector3 position;
    public bool isAlive;

    public EnemyStatus(string name, Vector3 pos, bool alive)
    {
        enemyName = name;
        position = pos;
        isAlive = alive;
    }
}

/// <summary>
/// フィールドのデータ管理クラス
/// </summary>
public class FieldData : MonoBehaviour
{
    public static FieldData Instance { get; private set; }

    [Header("Player Data")]
    public Vector3 playerPosition;

    [Header("Enemy Data")]
    // 敵の名前をキーにして状態を保存する辞書
    public Dictionary<string, EnemyStatus> enemyDic = new Dictionary<string, EnemyStatus>();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // シーンが読み込まれた時のイベントを登録
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // タイトル画面に戻ったらリセット
        if (scene.name == "Title")
        {
            ResetData();
            Debug.Log("タイトルに戻ったためデータをリセットしました");
        }
    }

    public void ResetData()
    {
        playerPosition = Vector3.zero;
        enemyDic.Clear();
    }

    // プレイヤーの座標を更新するメソッド
    public void SetPlayerPos(Vector3 pos) => playerPosition = pos;

    // 敵の状態を更新するメソッド
    public void SetEnemyStatus(string name, Vector3 pos, bool alive)
    {
        if (enemyDic.ContainsKey(name))
        {
            enemyDic[name].position = pos;
            enemyDic[name].isAlive = alive;
        }
        else
        {
            enemyDic.Add(name, new EnemyStatus(name, pos, alive));
        }
    }
}