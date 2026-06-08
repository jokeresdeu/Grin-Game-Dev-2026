using System;
using System.Collections;
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

        [Header("Animation")]
        [SerializeField] private float _idlePulseAmount = 0.04f;
        [SerializeField] private float _idlePulseSpeed = 4f;
        [SerializeField] private float _recoilDistance = 0.12f;
        [SerializeField] private float _recoilTime = 0.08f;
        [SerializeField] private float _damageFlashTime = 0.12f;

        private float _currentHealth;
        private float _fireTimer;
        private Enemy _target;
        private SpriteRenderer _spriteRenderer;
        private Vector3 _startScale;
        private Vector3 _startPosition;
        private Color _startColor = Color.white;
        private Coroutine _recoilRoutine;
        private Coroutine _damageFlashRoutine;
        public bool IsAlive => _currentHealth > 0;
        public float Range => _range;

        private void Awake()
        {
            if (_firePoint == null)
                _firePoint = transform;

            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            _startScale = transform.localScale;
            _startPosition = transform.localPosition;

            if (_spriteRenderer != null)
                _startColor = _spriteRenderer.color;

            _currentHealth = _maxHealth;
            UpdateHpBar();
        }

        private void Update()
        {
            if (!IsAlive) return;

            AnimateIdle();
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
            PlayRecoil();

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
                StartCoroutine(PlayDeathAnimation());
            }
            else
            {
                if (_damageFlashRoutine != null)
                    StopCoroutine(_damageFlashRoutine);

                _damageFlashRoutine = StartCoroutine(PlayDamageFlash());
            }
        }

        private void AnimateIdle()
        {
            float pulse = Mathf.Sin(Time.time * _idlePulseSpeed) * _idlePulseAmount;
            transform.localScale = new Vector3(_startScale.x + pulse, _startScale.y + pulse, _startScale.z);
        }

        private void PlayRecoil()
        {
            if (_recoilRoutine != null)
                StopCoroutine(_recoilRoutine);

            _recoilRoutine = StartCoroutine(PlayRecoilAnimation());
        }

        private IEnumerator PlayRecoilAnimation()
        {
            Vector3 recoilPosition = _startPosition + Vector3.left * _recoilDistance;
            transform.localPosition = recoilPosition;

            float timer = 0f;
            while (timer < _recoilTime)
            {
                timer += Time.deltaTime;
                float t = timer / _recoilTime;
                transform.localPosition = Vector3.Lerp(recoilPosition, _startPosition, t);
                yield return null;
            }

            transform.localPosition = _startPosition;
        }

        private IEnumerator PlayDamageFlash()
        {
            if (_spriteRenderer != null)
                _spriteRenderer.color = Color.red;

            yield return new WaitForSeconds(_damageFlashTime);

            if (_spriteRenderer != null)
                _spriteRenderer.color = _startColor;
        }

        private IEnumerator PlayDeathAnimation()
        {
            _currentHealth = 0f;
            float timer = 0f;
            Vector3 fromScale = transform.localScale;

            while (timer < 0.2f)
            {
                timer += Time.deltaTime;
                float t = timer / 0.2f;
                transform.localScale = Vector3.Lerp(fromScale, Vector3.zero, t);

                if (_spriteRenderer != null)
                    _spriteRenderer.color = Color.Lerp(_startColor, Color.clear, t);

                yield return null;
            }

            Destroy(gameObject);
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
