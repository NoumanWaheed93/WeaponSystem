using UnityEngine;

namespace WeaponSystem
{
    public interface IGuidedProjectile : IProjectile
    {
        public Transform Target { get; set; }
    }
}
