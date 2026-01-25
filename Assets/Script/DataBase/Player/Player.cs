using Data;
using Protect;
using UnityEngine;

namespace Data
{
    public class Player : CharaBase
    {
//        GameObject m_obj;
//        public GameObject obj => m_obj;
//        Transform m_transform;

        // 庇う
        private ProtectSystem m_protectSystem;

/*        public Player(GameObject obj, StatusBase playerStatus, int level, ProtectSystem protectSystem) : base(playerStatus, level)
        {
            //  オブジェクト引き渡し
            m_obj = obj;
            m_protectSystem = protectSystem;
            Debug.Log("Player生成");
        }*/
        public void Initialize(StatusBase playerStatus, int level, ProtectSystem protectSystem)
        {
            base.Initialize(playerStatus, level);
            m_protectSystem = protectSystem;
            Debug.Log("初期化");
        }

        public ProtectSystem GetProtectSystem()
        {
            return m_protectSystem;
        }
        public void SetProtextSystem(ProtectSystem protectSystem)
        {
            m_protectSystem = protectSystem;
        }
    }
}