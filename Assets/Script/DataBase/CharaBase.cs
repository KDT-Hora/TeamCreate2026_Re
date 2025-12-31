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
        // ------------------------ デバック表示用 ------------------------
        /// <summary>
        /// ゲーム画面にステータスを文字で表示する (OnGUI)
        /// </summary>
        private void OnGUI()
        {
            // カメラがない、またはステータスが未設定なら何もしない
            if (Camera.main == null || m_statusCalculated == null) return;

            // キャラクターの頭上（Y軸 + 2.0f）をスクリーン座標に変換
            Vector3 worldPos = transform.position + Vector3.up * 2.0f;
            Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);

            // カメラの背後にある場合は描画しない
            if (screenPos.z <= 0) return;

            // GUI座標系はY軸が上から下なので、座標を反転させる
            screenPos.y = Screen.height - screenPos.y;

            // 表示する情報の作成
            string debugText = $"Name: {GetName()}\n";
/*                +
                               $"Lv: {m_level}\n" +
                               $"HP: {m_statusRuntime.hp} / {m_statusCalculated.maxHp}\n" +
                               $"ATK: {m_statusRuntime.atk}";*/

            // 文字スタイルの設定（白文字・太字）
            GUIStyle style = new GUIStyle();
            style.normal.textColor = Color.white;
            style.fontSize = 14;
            style.fontStyle = FontStyle.Bold;
            style.alignment = TextAnchor.MiddleCenter;

            // 影用のスタイル（黒文字）
            GUIStyle shadowStyle = new GUIStyle(style);
            shadowStyle.normal.textColor = Color.black;

            // 表示位置の矩形定義
            Rect rect = new Rect(screenPos.x - 100, screenPos.y, 200, 100);
            Rect shadowRect = new Rect(rect.x + 2, rect.y + 2, rect.width, rect.height);

            // 影 → 本体の順で描画して読みやすくする
            GUI.Label(shadowRect, debugText, shadowStyle);
            GUI.Label(rect, debugText, style);
        }
    }
}