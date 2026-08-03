using UnityEngine;

namespace WeaponSystem
{
    public interface IProjectileFactory
    {
        public IProjectile GetProjectile();
        public IGuidedProjectile GetHomingProjectile();
    }
}
