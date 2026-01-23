using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    // 巡回距離（初期位置からどれだけ動くか）
    public float patrolRange = 3f;

    // 1フレームあたりの移動量
    public float patrolSpeed = 0.005f;
    public float chaseSpeed = 0.01f;

    // 追跡距離
    public float chaseStartDistance = 5f;
    public float chaseEndDistance = 7f;

    // プレイヤー
    public Transform player;

    // 内部状態
    private Vector3 startPosition;
    private Vector3 pointA;
    private Vector3 pointB;
    private Vector3 patrolTarget;
    private bool isChasing = false;

    void Start()
    {
        // 初期位置を記憶
        startPosition = transform.position;

        // 巡回ポイントを自動生成（左右移動）
        pointA = startPosition + Vector3.left * patrolRange;
        pointB = startPosition + Vector3.right * patrolRange;

        patrolTarget = pointA;
    }

    void Update()
    {
        float distanceToPlayer =
            Vector3.Distance(transform.position, player.position);

        // 追跡開始
        if (!isChasing && distanceToPlayer <= chaseStartDistance)
        {
            isChasing = true;
        }

        // 追跡解除 → 初期位置へテレポート
        if (isChasing && distanceToPlayer >= chaseEndDistance)
        {
            ResetEnemy();
            return;
        }

        // 行動
        if (isChasing)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }
    }

    // ===== 巡回 =====
    void Patrol()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            patrolTarget,
            patrolSpeed
        );

        if (Vector3.Distance(transform.position, patrolTarget) < 0.1f)
        {
            patrolTarget = (patrolTarget == pointA) ? pointB : pointA;
        }
    }

    // ===== 追跡 =====
    void ChasePlayer()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            player.position,
            chaseSpeed
        );
    }

    // ===== 当たり判定 =====
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("プレイヤーに当たった → 敵を初期位置へ戻す");
            ResetEnemy();
        }
    }

    // ===== 敵を初期状態に戻す =====
    void ResetEnemy()
    {
        isChasing = false;
        transform.position = startPosition;
        patrolTarget = pointA;
    }
}