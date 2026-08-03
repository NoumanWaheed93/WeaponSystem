using UnityEngine;

namespace WeaponSystem
{
    public class ProjectileLauncher : Weapon
    {
        private IProjectileFactory projectileFactory;
        
        public ProjectileLauncher(Transform barrel, ITimeProvider timeProvider, int maximumAmmo, float bulletsPerSecond, IProjectileFactory projectileFactory):base(barrel, timeProvider, maximumAmmo, bulletsPerSecond)
        {
            this.projectileFactory = projectileFactory;
        }

        public override bool Fire()
        {
            if (base.Fire())
            {
                IProjectile newProjectile = projectileFactory.GetProjectile(); // GameObject.Instantiate<Projectile>(projectile);
                newProjectile.Launch(Barrel);
                return true;
            }
            return false;
        }
    }
}
