using Data;
using Protect;
using UnityEngine;

namespace Data
{
    public class Enemy : CharaBase
    {
        GameObject m_obj;
        public GameObject obj => m_obj;

        Transform m_transform;

        public Enemy(GameObject obj, StatusBase playerStatus, int level) : base(playerStatus, level)
        {
            //  オブジェクト引き渡し
            m_obj = obj;
            Debug.Log("Enemy生成");
        }
    }
}