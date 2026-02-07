using UnityEngine;
namespace Protect
{
    public static class ProtectSystemFactory
    {
        public static ProtectSystem Create(int protectId)
        {
            switch (protectId)
            {
                case 0: return new ProtectSystem();     //  主人公     デフォルト
                case 1: return new ProtectCounter();    //  駅員      カウンター
                case 2: return new ProtectSystem();     //  ヒロイン    まだデフォルトを実装
            }
    
            return null; // デフォルトなし
        }
    }
}