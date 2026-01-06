using UnityEngine;

//  ユニットの操作コンポーネント
//  アニメーション再生などを担当
public class UnitController : MonoBehaviour
{
    [Haader("キャラクターのデータ")]
    [SerializeField] // ステータス
    [SerializeField] // モデル

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    //  攻撃アニメーション再生
    public void AttackAnim()
    {
        //  アニメーション再生
    }

    //  被弾アニメーション再生
    public void HitAnim()
    {
        //  アニメーション再生
    }

}
