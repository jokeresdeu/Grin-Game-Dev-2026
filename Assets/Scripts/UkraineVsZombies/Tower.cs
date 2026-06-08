using System;
using UnityEngine;
using UnityEngine.UI;

namespace UkraineVsZombies
{
    public class Tower : MonoBehaviour
    {
        [Header("Stats")]
        [SerializeField] private float _maxHealth = 100f;
        [SerializeField] private float _range = 5f;
        [SerializeField] private float _fireRate = 1f;
        [SerializeField] private float _damage = 10f;
        [SerializeField] private LayerMask _enemyLayerMask = -1;

        [Header("Projectile")]
        [SerializeField] private GameObject _projectilePrefab;
        [SerializeField] private Transform _firePoint;

        [Header("HP Bar")]
        [SerializeField] private Slider _hpSlider;

        private float _currentHealth;
        private float _fireTimer;
        private Enemy _target;
        public bool IsAlive => _currentHealth > 0;
        public float Range => _range;

        private void Awake()
        {
            if (_firePoint == null)
                _firePoint = transform;

            _currentHealth = _maxHealth;
            UpdateHpBar();
        }

        private void Update()
        {
            if (!IsAlive) return;
            FindTargetWithOverlap();
            TryFire();
        }

        public void SetTarget(Enemy target)
        {
            _target = target;
        }

        private void TryFire()
        {
            _fireTimer -= Time.deltaTime;

            if (_target == null || !_target.IsAlive || _fireTimer > 0f) return;

            Fire();
            _fireTimer = 1f / _fireRate;
        }

        private void Fire()
        {
            if (_projectilePrefab != null)
            {
                var obj = Instantiate(_projectilePrefab, _firePoint.position, Quaternion.identity);
                var projectile = obj.GetComponent<Projectile>();
                if (projectile != null)
                    projectile.Initialize(_target, _damage);
            }
            else
            {
                _target.TakeDamage(_damage);
            }
        }

        private void FindTargetWithOverlap()
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, _range, _enemyLayerMask);
            Enemy bestTarget = null;
            float closestDistance = float.MaxValue;

            foreach (var hit in hits)
            {
                Enemy enemy = hit.GetComponent<Enemy>();
                if (enemy == null || !enemy.IsAlive) continue;

                float distanceX = enemy.transform.position.x - transform.position.x;
                if (distanceX <= 0f || distanceX > _range) continue;

                if (distanceX < closestDistance)
                {
                    closestDistance = distanceX;
                    bestTarget = enemy;
                }
            }

            if (bestTarget != null)
            {
                _target = bestTarget;
            }
        }

        public void TakeDamage(float damage)
        {
            if (!IsAlive) return;

            _currentHealth -= damage;
            UpdateHpBar();

            if (_currentHealth <= 0f)
            {
                Destroy(gameObject);
            }
        }

        private void UpdateHpBar()
        {
            if (_hpSlider != null)
                _hpSlider.value = _currentHealth / _maxHealth;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + Vector3.right * _range);
            Gizmos.DrawWireSphere(transform.position, _range);
        }
    }
}
