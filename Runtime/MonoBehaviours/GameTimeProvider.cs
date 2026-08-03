using UnityEngine;

namespace WeaponSystem
{
    public class GameTimeProvider : ITimeProvider
    {
        public float GetTime()
        {
            return Time.time;
        }
    
    }

}