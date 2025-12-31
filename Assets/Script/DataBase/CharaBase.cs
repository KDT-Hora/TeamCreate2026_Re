using UnityEngine;

namespace Data
{
    public class CharaBase : MonoBehaviour
    {
        // マスタデータ
        // 生成時に外部から持ってくる
        protected StatusBase m_statusBase;
        // レベル計算後の基礎ステータス
        // レベルアップ時は再計算する
        protected StatusCalculated m_statusCalculated;
        // バトル中の変動ステータス
        // 常に変動する
        [SerializeField] // Inspectorでデバッグ確認できるようにする
        protected StatusRuntime m_statusRuntime;
        // 現在のレベル
        protected int m_level = 1;
        // ------------------------ 初期化とか ------------------------
/*        public CharaBase(StatusBase data, int level)
        {
            m_statusBase = data;
            m_level = level;

            m_statusCalculated = new StatusCalculated();
            m_statusRuntime = new StatusRuntime();

            CalculateStatus();
            FullRecovery();
        }*/
        public void Initialize(StatusBase data, int level)
        {
            m_statusBase = data;
            m_level = level;

            m_statusCalculated = new StatusCalculated();
            m_statusRuntime = new StatusRuntime();

            CalculateStatus();
            FullRecovery();
        }
        protected virtual void CalculateStatus()
        {
            if (m_statusBase == null) return;

            // Calculated
            // レベル換算用
            m_statusCalculated.maxHp = Mathf.FloorToInt(m_statusBase.baseHp * m_level);
            m_statusCalculated.maxMp = Mathf.FloorToInt(m_statusBase.baseMp * m_level);
            m_statusCalculated.atk = Mathf.FloorToInt(m_statusBase.baseAtk * m_level);
            m_statusCalculated.def = Mathf.FloorToInt(m_statusBase.baseDef * m_level);
            m_statusCalculated.matk = Mathf.FloorToInt(m_statusBase.baseMatk * m_level);
            m_statusCalculated.mdef = Mathf.FloorToInt(m_statusBase.baseMdef * m_level);
            m_statusCalculated.speed = Mathf.FloorToInt(m_statusBase.baseSpeed * m_level);
            // 値を変化させる用
            m_statusRuntime.hp = m_statusCalculated.maxHp;
            m_statusRuntime.mp = m_statusCalculated.maxMp;
            m_statusRuntime.atk = m_statusCalculated.atk;
            m_statusRuntime.def = m_statusCalculated.def;
            m_statusRuntime.matk = m_statusCalculated.matk;
            m_statusRuntime.mdef = m_statusCalculated.mdef;
            m_statusRuntime.speed = m_statusCalculated.speed;
        }
        // ------------------------ 取得用 ------------------------
        public StatusCalculated GetStatusCalculated() { return m_statusCalculated; }
        public StatusRuntime GetStatusRuntime() { return m_statusRuntime; }

        /// <summary>
        /// 名前の取得
        /// </summary>
        public string GetName()
        {
            if (m_statusBase != null)
            {
                return m_statusBase.name;
            }
            return "Unknown";
        }
        /// <summary>
        /// ステータスの取得
        /// </summary>
        /// <returns></returns>
        public StatusRuntime GetStatus()
        {
            return m_statusRuntime;
        }
        /// <summary>
        /// 死亡判定
        /// </summary>
        /// <returns></returns>
        public bool IsDead()
        {
            return m_statusRuntime.hp <= 0;
        }

        // ------------------------ バトル用アクション ------------------------
        /// <summary>
        /// ダメージを受ける
        /// </summary>
        /// <param name="damage">ダメージの数</param>
        public void TakeDamage(int damage)
        {
            if (IsDead()) return;
            m_statusRuntime.hp -= damage;
            if (m_statusRuntime.hp < 0) m_statusRuntime.hp = 0;
        }
        /// <summary>
        /// 回復
        /// </summary>
        /// <param name="amount">回復指せる数</param>
        public void Heal(int amount)
        {
            if (IsDead()) return;
            m_statusRuntime.hp += amount;
            if (m_statusRuntime.hp > m_statusCalculated.maxHp)
            {
                m_statusRuntime.hp = m_statusCalculated.maxHp;
            }
        }
        /// <summary>
        /// HPとMPを最大値回復
        /// </summary>
        public void FullRecovery()
        {
            m_statusRuntime.hp = m_statusCalculated.maxHp;
            m_statusRuntime.mp = m_statusCalculated.maxMp;
        }
    }
}