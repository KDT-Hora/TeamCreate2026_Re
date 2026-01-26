using System;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed = 5f;

    private Rigidbody rb;

   Animator m_anim;

    string next_anim;
    string now_anim;

    public float minX = -100f;
    public float maxX = 100f;
    public float minZ = -50f;
    public float maxZ = 10f;

    // ボスと当たっているかどうかを保持する内部変数
    private bool isTouchingBoss = false;

    void Start()
    {
        SoundManager.Instance.PlayBGM("BGM_Field");
        //rb = GetComponent<Rigidbody>();
        //m_anim = GetComponentInChildren<Animator>();

        // シーン開始時、保存された座標があればそこに移動する
        if (FieldData.Instance != null && FieldData.Instance.playerPosition != Vector3.zero)
        {
            // 物理演算(Rigidbody)を使っている場合、positionを直接書き換える
            transform.position = FieldData.Instance.playerPosition;
            Debug.Log("保存された座標に復帰しました: " + FieldData.Instance.playerPosition);
        }
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
        // 2. 次の移動先を仮計算
        Vector3 nextPosition = rb.position + movement;

        // 3. XとZの値を制限（Clamp）する
        nextPosition.x = Mathf.Clamp(nextPosition.x, minX / 10, maxX / 100 * 15);
        nextPosition.z = Mathf.Clamp(nextPosition.z, minZ, maxZ);

        // 4. 制限された座標へ移動
        rb.MovePosition(nextPosition);

    }

    // シーンが切り替わる直前や、アプリを閉じる直前に呼ばれる
    private void OnDisable()
    {
        //if (FieldData.Instance != null)
        {
            FieldData.Instance.SetPlayerPos(transform.position);
            Debug.Log("シーン切り替えのため座標を保存しました: " + transform.position);
        }
    }

    // 外部から「今ボスと当たってる？」と聞かれたときに結果を返す関数
    public bool IsTouchingBoss()
    {
        return isTouchingBoss;
    }


    // ===== 敵との当たり判定 =====
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("BOSS"))
        {
            isTouchingBoss = true;
            DataManager.Instance.isBossBattle = true;
            Debug.Log("BOSSに接触！");
        }

        // 衝突した相手のタグが Enemy の場合
        if (collision.gameObject.CompareTag("Enemy"))
        {
            DataManager.Instance.isBossBattle = false;
            Debug.Log("敵に当たった！");
            SoundManager.Instance.PlaySE("SE_Enemy_Hit");
            SoundManager.Instance.PlayBGM("BGM_Battle");
            FadeManager.FadeChangeScene("BattleScene", 1.0f);
        }
    }

}