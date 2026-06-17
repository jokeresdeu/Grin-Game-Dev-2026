using UnityEngine;

namespace Projects.OrbitGunner.Scripts
{

    public class Weapon : MonoBehaviour
    {
        [SerializeField] private float _fireInterval = 0.16f;
        [SerializeField] private float _bulletSpeed = 14f;
        [SerializeField] private int _damage = 1;
        [SerializeField] private float _bulletLife = 1.6f;
        [SerializeField] private float _muzzleDistance = 1.35f;

        private float _cooldown;

        public bool TryFire(Vector2 aimDirection)
        {
            if (_cooldown > 0f)
                return false;

            _cooldown = _fireInterval;

            Vector3 muzzle = transform.position + (Vector3)(aimDirection.normalized * _muzzleDistance);

            if (BulletPool.Instance != null)
                BulletPool.Instance.Spawn(muzzle, aimDirection, _bulletSpeed, _damage, _bulletLife);

            return true;
        }

        private void Update()
        {
            if (_cooldown > 0f)
                _cooldown -= Time.deltaTime;
        }
    }
}
