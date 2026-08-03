using UnityEngine;
using HealthSystem;

namespace WeaponSystem
{
    public class GunRaycastBased : Weapon
    {
        private float range;
        private float damageAmount;

        public GunRaycastBased(Transform barrel, ITimeProvider timeProvider, int maximumAmmo, float bulletsPerSecond, float range, float damageAmount) : base(barrel, timeProvider, maximumAmmo, bulletsPerSecond)
        {
            this.range = range;
            this.damageAmount = damageAmount;
        }

        //Should be called in FixedUpdate
        public override bool Fire()
        {
            if (base.Fire())
            {
                RaycastHit hitInfo;
                if (Physics.Raycast(Barrel.position, Barrel.forward, out hitInfo, range))
                {
                    IDamageable damageable;
                    if (hitInfo.collider.TryGetComponent<IDamageable>(out damageable))
                    {
                        Debug.Log($"Raycast hit {hitInfo.collider.name} at distance {hitInfo.distance}, applying {damageAmount} damage.");
                        damageable.Damage(damageAmount);
                    }
                }
                return true;
            }
            return false;
        }

    }
}
