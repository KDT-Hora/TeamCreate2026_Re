using NUnit.Framework;
using UnityEngine;

/// <summary>
/// フィールドのデータ管理クラス
/// </summary>
public class FieldData : MonoBehaviour
{
    public static FieldData Instance { get; private set; }

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

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}