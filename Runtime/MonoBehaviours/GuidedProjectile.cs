using UnityEngine;

namespace WeaponSystem
{
    public class GuidedProjectile : Projectile, IGuidedProjectile
    {
        private Transform target;
        public Transform Target { get => target; set => target = value; }

        public Transform Transform => m_transform;

        [SerializeField]
        private float turningSpeed;
         
        private new void Awake()
        {
            base.Awake();
        }

        private new void FixedUpdate()
        {
            Quaternion targetRotation = Quaternion.LookRotation(target.position - m_transform.position);
            m_transform.rotation = Quaternion.Slerp(m_transform.rotation, targetRotation, turningSpeed * Time.fixedDeltaTime);
            base.FixedUpdate();
        }
    }
}
