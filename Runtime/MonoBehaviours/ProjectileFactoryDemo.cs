using UnityEngine;

namespace WeaponSystem
{
    public class ProjectileFactoryDemo : MonoBehaviour, IProjectileFactory
    {
        [SerializeField]
        private GameObject simpleProjectilePrefab;
        [SerializeField]
        private GameObject homingProjectilePrefab;

        public IGuidedProjectile GetHomingProjectile()
        {
            return Instantiate(homingProjectilePrefab).GetComponent<IGuidedProjectile>();
        }

        public IProjectile GetProjectile()
        {
            return Instantiate(simpleProjectilePrefab).GetComponent<IProjectile>();
        }
    }
}
