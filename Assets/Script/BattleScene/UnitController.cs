using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using Data;
using Protect; // TextMeshProを使用する場合


public class UnitController : MonoBehaviour
{
    [Header("Status")]
//    public string unitName;
//    public int maxHp;
//    public int currentHp;
//    public int speed;
    public bool isPlayer;
    public bool isDead;

    Player unitData;        //  ユニットのデータ共有部への参照


    [Header("Hate System")]
    [Range(0, 100)] public int currentHate = 0; // 現在のヘイト値
    public int maxHate = 100;    // ヘイト最大値

    [Header("UI References")]
    public TextMeshProUGUI nameText;    // または Text
    public Slider hpSlider;
    public Image hpFillImage;           // 色変更用
    public Slider hateSlider;
    public Image hateFillImage;           // 色変更用

    // HP量の数値表示
    [Header("UI Value")]
    public TextMeshProUGUI hpValueText; // または Text
    public TextMeshProUGUI hateValueText; // または Text

    //   public UnitUI unitUI;


    [Header("State Flags")]
    public bool isDefending;
    public bool isCovering;         // 今ターン庇う待機中か
    public bool hasCoveredThisTurn; // 今ターン既に庇ったか


    public float animationLength;

    private Vector3 originalPosition;
    private Renderer[] renderers;

    private CharaAnim characterAnimator;

    protected virtual void Start()
    {
        //  位置の初期化
        originalPosition = transform.position;
        renderers = GetComponentsInChildren<Renderer>();
    //    currentHp = maxHp;
    //    isDead = false;

        characterAnimator = GetComponentInChildren<CharaAnim>();

        // UI初期化
    //    if (nameText) nameText.text = unitName;
    //    UpdateHPBar();
    //    AddHate(0);
    //    if (hpFillImage) hpFillImage.color = new Color(0.2f, 1f, 0.2f); // 明るい緑
    }

    //  ユニットの初期化
    //  戦闘開始時に呼び出し
    public void UnitInit(Player aUnitData)
    {
        Debug.Log($"UnitInit called for {aUnitData.GetName()}");

        unitData = aUnitData;

        //  ステータス初期化
        //  データ共有部からステータス初期化
    //    maxHp = aMaxHP;
    //    currentHp = maxHp;
    //    speed = aStatus.speed;


    //    isDead = false;
        // UI初期化
        if (nameText) nameText.text = unitData.GetName();
        UpdateHPBar();
        AddHate(0);

    }

    public void UpdateHPBar()
    {
        if (hpSlider)
        {
            hpSlider.value = 
                (float)unitData.GetStatusRuntime().hp /
                unitData.GetStatusCalculated().maxHp;
        }
        if (hpValueText)
        {
            hpValueText.text = $"{unitData.GetStatusRuntime().hp} / " +
                $"{unitData.GetStatusCalculated().maxHp}";
        }
    }

    public void TakeDamage(int damage)
    {
        unitData.GetStatusRuntime().hp -= damage;
        if (unitData.GetStatusRuntime().hp <= 0)
        {
            unitData.GetStatusRuntime().hp = 0;
            isDead = true;
            // 死亡時の見た目（例: 暗くする、倒れるなど）
            foreach (var r in renderers) r.material.color = Color.gray;

            //  死亡したキャラは非表示に
            gameObject.SetActive(false);
        }
        UpdateHPBar();
    }

    public void AddHate(int amount)
    {
        if (!isPlayer) return; // 敵はヘイトを持たない想定
        currentHate += amount;

        // 最大値を超えないように制限（クランプ）
        if (currentHate > maxHate) currentHate = maxHate;

        Debug.Log($"{gameObject.name} のヘイトが {amount} 上昇！ 現在: {currentHate}");

        // ここにヘイトゲージUIの更新処理があれば呼ぶ
        if (hateSlider)
        {
            hateSlider.value = (float)currentHate / 100;
        }
        if (hateValueText)
        {
            hateValueText.text = $"{currentHate} / {maxHate}";
        }

    }

    public ProtectSystem GetProtectSystem()
    {
        return unitData.GetProtectSystem();
    }

    public int GetSpeed()
    {
        return unitData.GetStatusCalculated().speed;
    }
    public string GetUnitName()
    {
        return unitData.GetName();
    }

    public Player GetUnitData()
    {
        return unitData;
    }

    // --- アニメーション関連 ---

    // 行動時のジャンプ演出
    //public IEnumerator AnimateActionJump()
    //{
    //    float duration = 0.2f;
    //    float height = 0.5f;
    //    float elapsed = 0f;

    //    // 上へ
    //    while (elapsed < duration)
    //    {
    //        transform.position = originalPosition + Vector3.up * Mathf.Lerp(0, height, elapsed / duration);
    //        elapsed += Time.deltaTime;
    //        yield return null;
    //    }
    //    // 下へ
    //    elapsed = 0f;
    //    while (elapsed < duration)
    //    {
    //        transform.position = originalPosition + Vector3.up * Mathf.Lerp(height, 0, elapsed / duration);
    //        elapsed += Time.deltaTime;
    //        yield return null;
    //    }
    //    transform.position = originalPosition;
    //}

    public IEnumerator AnimateActionAttack()
    {
        //  キャラクターのアニメーションを再生
        // キャラクターモデルについている、攻撃アニメーションを再生する
        if (characterAnimator != null)
        {
            characterAnimator.PlayAttack();
        }
        //  攻撃アニメーションの長さを取得
        animationLength = characterAnimator.GetAttackAnimLength(); // アニメーションの長さに合わせて調整
        // 攻撃アニメーションの長さに合わせて待機
        yield return new WaitForSeconds(animationLength); // アニメーションの長さに合わせて調整


    }


    // 対象選択時などの点滅演出
    public IEnumerator AnimateBlink(float duration)
    {
        float endTime = Time.time + duration;
        bool visible = true;
        while (Time.time < endTime)
        {
            foreach (var r in renderers) r.enabled = visible;
            visible = !visible;
            yield return new WaitForSeconds(0.1f);
        }
        foreach (var r in renderers) r.enabled = true;
    }

    // ターン開始時のリセット
    public void ResetTurnState()
    {
        isDefending = false;
        isCovering = false;
        hasCoveredThisTurn = false;
    }
}