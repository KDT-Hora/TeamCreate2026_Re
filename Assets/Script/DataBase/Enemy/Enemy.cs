using Data;
using Protect;
using UnityEngine;

namespace Data
{
    public class Enemy : CharaBase
    {
//        GameObject m_obj;
//        public GameObject obj => m_obj;
//        Transform m_transform;

        public void Initialize(StatusBase enemyStatus, int level)
        {
            base.Initialize(enemyStatus, level);
            Debug.Log("Enemy‰Šú‰»");
        }
    }
}