using UnityEngine;

/// <summary>
/// データ管理用クラス
/// ここで生成されたやつは消えないようにします。
/// </summary>
public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; }
    
    public PartyData partyData { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // パーティーデータ初期化
        partyData = new PartyData();
    }

    private void Start()
    {

    }
}
