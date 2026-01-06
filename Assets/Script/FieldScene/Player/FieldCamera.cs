using UnityEngine;

public class FieldCamera : MonoBehaviour
{
    // 追従対象（Player）
    public Transform target;

    // プレイヤーとの距離
    public Vector3 offset = new Vector3(0f, 5f, -7f);

    // 毎フレーム呼ばれる
    void LateUpdate()
    {
        // プレイヤーの位置 + オフセットにカメラを移動
        transform.position = target.position + offset;

        // 常にプレイヤーの方向を見る
        transform.LookAt(target);
    }
}
