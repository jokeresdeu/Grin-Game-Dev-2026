using System.Collections.Generic;
using UnityEngine;

namespace Projects.OrbitGunner.Scripts
{
    public class BulletPool : MonoBehaviour
    {
        public static BulletPool Instance { get; private set; }

        [SerializeField] private Bullet _bulletPrefab;
        [SerializeField] private Transform _container;
        [SerializeField] private int _initialSize = 64;

        private readonly Queue<Bullet> _available = new Queue<Bullet>();

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;

            for (int i = 0; i < _initialSize; i++)
            {
                Bullet bullet = CreateBullet();
                if (bullet != null)
                    _available.Enqueue(bullet);
            }
        }

        public void Spawn(Vector3 position, Vector2 direction, float speed, int damage, float life)
        {
            Bullet bullet = _available.Count > 0 ? _available.Dequeue() : CreateBullet();
            if (bullet != null)
                bullet.Activate(position, direction, speed, damage, life);
        }

        public void Return(Bullet bullet)
        {
            if (bullet == null)
                return;

            bullet.gameObject.SetActive(false);
            _available.Enqueue(bullet);
        }

        private Bullet CreateBullet()
        {
            if (_bulletPrefab == null)
                return null;

            Bullet bullet = Instantiate(_bulletPrefab, _container);
            bullet.gameObject.SetActive(false);
            return bullet;
        }

        private void OnDestroy()
        {
            if (Instance == this)
                Instance = null;
        }
    }
}
