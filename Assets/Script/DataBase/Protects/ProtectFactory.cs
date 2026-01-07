using UnityEngine;
namespace Data
{
    public static class ProtectSystemFactory
    {
        public static ProtectSystem Create(int protectId)
        {
            switch (protectId)
            {
                case 1: return new ProtectCounter();
                case 2: return new ProtectCounter();
                case 3: return new ProtectCounter();
            }
    
            return null; // デフォルトなし
        }
    }
}