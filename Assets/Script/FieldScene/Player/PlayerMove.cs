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
        SoundManager.Instance.PlayBGM("BGM_Field");
        //rb = GetComponent<Rigidbody>();
       //m_anim = GetComponentInChildren<Animator>();
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = GetComponentInChildren<Rigidbody>();
        }
        if (rb == null)
        {
            rb = GetComponentInParent<Rigidbody>();
        }
        if (rb == null)
        {
            Debug.LogError("Rigidbody が見つかりません");
        }

        m_anim = GetComponentInChildren<Animator>();
        if (m_anim == null)
        {
            Debug.LogWarning("Animator が見つかりません");
        }
    }


    void FixedUpdate()
    {
        if (rb == null)
        {
            return;
        }
        // キーボード入力
        float x = 0f;
        float z = 0f;
        bool moving = false;

        if (Input.GetKey(KeyCode.A)) { x -= 1f; next_anim = "LeftRun"; moving = true; }
        if (Input.GetKey(KeyCode.D)) { x += 1f; next_anim = "RightRun"; moving = true; }
        if (Input.GetKey(KeyCode.W)) { z += 1f; next_anim = "UpRun"; moving = true; }
        if (Input.GetKey(KeyCode.S)) { z -= 1f; next_anim = "DownRun"; moving = true; }

        if (!moving)
        {
            next_anim = "Idle"; // 動いていなければ必ずIdleに
        }

        // Animatorに反映
        bool running = moving;
        if (m_anim != null)
        {
            m_anim.SetBool("isRunning", running);
            if (next_anim != now_anim)
            {
                now_anim = next_anim;
                m_anim.CrossFade(now_anim, 0.1f, 0);
            }
        }

        // 移動処理
        Vector3 movement = new Vector3(x, 0f, z).normalized * speed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + movement);
    }

    // ===== 敵との当たり判定 =====
    void OnCollisionEnter(Collision collision)
    {
        // 衝突した相手のタグが Enemy の場合
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("敵に当たった！");
            SoundManager.Instance.PlaySE("SE_Enemy_Hit");
            SoundManager.Instance.PlayBGM("BGM_Battle");
            FadeManager.FadeChangeScene("BattleScene", 1.0f);
        }
    }
}