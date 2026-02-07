using Unity.VisualScripting;
using UnityEngine;

public class PlayEffect : MonoBehaviour
{
    public enum EffectType
    {
        Damage,
        Heal,
        Buff,
        Debuff
    }

    [Header("Effect Prefabs")]
    [SerializeField] private GameObject effectDamage;
    [SerializeField] private GameObject effectHeal;
    [SerializeField] private GameObject effectBuff;
    [SerializeField] private GameObject effectDebuff;

    [Header("Settings")]
    [SerializeField] private float effectLifetime = 2.0f;

    // 元の関数（このオブジェクトの位置に生成）
    void SpawnEffect(EffectType type)
    {
        SpawnEffect(type, transform.position);
    }

    // 座標を指定してエフェクト生成
    public void SpawnEffect(EffectType type, Vector3 position)
    {
        GameObject targetPrefab = GetEffectPrefab(type);

        if (targetPrefab != null)
        {
            GameObject obj = Instantiate(targetPrefab, position, Quaternion.identity);
            Destroy(obj, effectLifetime);
        }
        else
        {
            Debug.LogWarning($"{type} のプレハブがアサインされていません！");
        }
    }

 



    // switch文をまとめた関数
    GameObject GetEffectPrefab(EffectType type)
    {
        switch (type)
        {
            case EffectType.Damage:
                return effectDamage;
            case EffectType.Heal:
                return effectHeal;
            case EffectType.Buff:
                return effectBuff;
            case EffectType.Debuff:
                return effectDebuff;
            default:
                return null;
        }
    }
}