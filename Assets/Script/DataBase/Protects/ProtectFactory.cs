using UnityEngine;
namespace Protect
{
    public static class ProtectSystemFactory
    {
        public static ProtectSystem Create(int protectId)
        {
            switch (protectId)
            {
                case 1: return new ProtectCounter();
            }
    
            return null; // デフォルトなし
        }
    }
}