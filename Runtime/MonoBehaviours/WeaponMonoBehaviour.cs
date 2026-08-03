using UnityEngine;

namespace WeaponSystem
{
    public abstract class WeaponMonoBehaviour : MonoBehaviour
    {
        [SerializeField]
        protected int maxAmmo;
        [SerializeField]
        protected float bulletsPerSecond;
        [SerializeField]
        protected GameObject barrelGO;

        protected Weapon weapon;

    }
}
