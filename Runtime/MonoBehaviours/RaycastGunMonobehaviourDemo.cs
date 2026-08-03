using UnityEngine;

namespace WeaponSystem
{
    public class RaycastGunMonobehaviourDemo : WeaponMonoBehaviour
    {
        [SerializeField]
        private float range;

        [SerializeField]
        private float damageAmount;

        [SerializeField]
        private GameObject bulletLine;

        private float bulletLife = 0;

        private void Awake()
        {
            weapon = new GunRaycastBased(transform, new GameTimeProvider(), maxAmmo, bulletsPerSecond, range, damageAmount);
        }

        private void Update()
        {
            bulletLife -= Time.deltaTime;
            if (bulletLife <= 0)
            {
                bulletLine.SetActive(false);
            }
        }

        public void Fire()
        {
            Debug.Log("Firing Raycast Gun");
            if (weapon.Fire())
            {
                bulletLife = 0.035f;
                bulletLine.SetActive(true);
            }
        }
    }
}
