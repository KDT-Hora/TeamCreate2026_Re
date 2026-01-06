using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed = 5f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // ===== キーボード入力処理 =====
        float x = 0f;
        float z = 0f;

        if (Input.GetKey(KeyCode.A)) x -= 1f; // 左
        if (Input.GetKey(KeyCode.D)) x += 1f; // 右
        if (Input.GetKey(KeyCode.W)) z += 1f; // 前
        if (Input.GetKey(KeyCode.S)) z -= 1f; // 後

        // 斜め移動の速度調整
        Vector3 direction = new Vector3(x, 0f, z).normalized;

        // ===== 移動処理 =====
        rb.linearVelocity = new Vector3(
            direction.x * speed,
            rb.linearVelocity.y,   // 重力処理は採用しない
            direction.z * speed
        );
    }

    // ===== 敵との当たり判定 =====
    void OnCollisionEnter(Collision collision)
    {
        // 衝突した相手のタグが Enemy の場合
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("敵に当たった！");
        }
    }
}