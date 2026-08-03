using UnityEngine;

namespace WeaponSystem
{
    public class GuidedProjectileLauncher : Weapon
    {

        private IProjectileFactory projectileFactory;

        private Transform target;
        public Transform Target { get => target; set => target = value; }

        public GuidedProjectileLauncher(Transform barrel, ITimeProvider timeProvider, int maximumAmmo, float bulletsPerSecond, IProjectileFactory projectileFactory) : base(barrel, timeProvider, maximumAmmo, bulletsPerSecond)
        {
            this.projectileFactory = projectileFactory;
        }

        public override bool Fire()
        {
            if (base.Fire())
            {
                IGuidedProjectile newProjectile = projectileFactory.GetHomingProjectile();
                newProjectile.Launch(Barrel);
                newProjectile.Target = target;
                return true;
            }
            return false;
        }
    }
}
