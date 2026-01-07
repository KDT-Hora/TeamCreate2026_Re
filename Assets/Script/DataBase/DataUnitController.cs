using UnityEngine;

namespace Data {
    public class DataUnitController : MonoBehaviour
    {
        public int unitID; // ここにIDが入る
    
        // 確認用: 生成されたらログを出す
        void Start()
        {
            Debug.Log(gameObject.name + " (ID: " + unitID + ") generated");
        }
    }
}