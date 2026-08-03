using UnityEngine;

namespace WeaponSystem
{
    public abstract class Weapon
    {
        private int maximumAmmo;
        public int MaximumAmmo { get => maximumAmmo; }
        
        protected int remainingAmmo;

        public int RemainingAmmo { get => remainingAmmo; }

        private float lastFireTime;

        private bool hasFired;

        private float shotInterval;
        public float ShotInterval { get => shotInterval; }

        private Transform barrel;
        protected Transform Barrel { get => barrel; }

        private ITimeProvider timeProvider;

        public Weapon(Transform barrel, ITimeProvider timeProvider, int maximumAmmo, float bulletsPerSecond)
        {
            this.barrel = barrel;
            this.timeProvider = timeProvider;
            this.maximumAmmo = maximumAmmo;
            remainingAmmo = maximumAmmo;
            shotInterval = 1f / bulletsPerSecond;
            lastFireTime = 0f;
            hasFired = false;
        }

        public virtual bool Fire()
        {
            if (HasAmmoRanOut())
            {
                return false;
            }
            if (!HasShotIntervalPassed())
            {
                return false;
            }
            remainingAmmo--;
            lastFireTime = timeProvider.GetTime();
            hasFired = true;
            return true;
        }

        public virtual void Reload()
        {
            remainingAmmo = maximumAmmo;
        }

        protected bool HasShotIntervalPassed()
        {
            //A weapon that never fired has no interval to wait for.
            if (!hasFired)
            {
                return true;
            }
            return lastFireTime + ShotInterval <= timeProvider.GetTime();
        }

        protected bool HasAmmoRanOut()
        {
            return remainingAmmo <= 0;
        }
    }
}
