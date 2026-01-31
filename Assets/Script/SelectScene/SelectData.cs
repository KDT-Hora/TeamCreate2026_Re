using Unity.VisualScripting;
using UnityEngine;

public class SelectData : MonoBehaviour
{
    public static SelectData Instance { get; private set; }

    // ボス
    public enum Boss
    {
        Ittan,
        Syuten,
        Karasu,
    }
    // 選択したボスだけ保持する
    public Boss selectBoss;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Setter
    public void SetBoss(Boss boss)
    {
        Debug.Log("選択したボス: " + boss);
        DataManager.Instance.currentBossID = (int)boss;
        selectBoss = boss;
    }

    // Getter
    public int GetBoss()
    {
        Debug.Log("取得したボス: " + selectBoss.ToString());
        return (int)selectBoss;    
    }
}