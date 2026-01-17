using UnityEngine;

public class FloatingText : MonoBehaviour
{
    [Header("追従する対象")]
    public Transform target;

    [Header("位置のオフセット")]
    public Vector3 offset = new Vector3(10, 3, 0);

    // Updateだとガクつく場合があるため、カメラ移動後のLateUpdateを使用
    void LateUpdate()
    {
        if (target == null) return;

        // ターゲットの位置 + オフセットに移動
        transform.position = target.position + offset;

        // 文字が常にカメラの方を向くようにする（ビルボード処理）
        //transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
    }
}

