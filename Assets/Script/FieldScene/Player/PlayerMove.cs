using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed = 5f;

    private Rigidbody rb;

   Animator m_anim;

    string next_anim;
    string now_anim;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
       m_anim = GetComponentInChildren<Animator>();
    }

    void FixedUpdate()
    {
        // ===== キーボード入力処理 =====
        float x = 0f;
        float z = 0f;

        if (Input.GetKey(KeyCode.A)) {
            x -= 1f; // 左
            next_anim = "LeftRun";
        }
        if (Input.GetKey(KeyCode.D)){
            x += 1f; // 右
            next_anim = "RightRun";
        }
        if (Input.GetKey(KeyCode.W)){
            z += 1f; // 前
            next_anim = "UpRun";
        }
        if (Input.GetKey(KeyCode.S)){
            z -= 1f; // 後
            next_anim = "DownRun";
        }

        if(next_anim != now_anim)
        {
            now_anim = next_anim;
            m_anim.CrossFade(now_anim, 0.1f,0);
            m_anim.Update(0);
            next_anim ="Idle";
        }
        

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
            FadeManager.FadeChangeScene("BattleScene", 1.0f);
        }
    }
}