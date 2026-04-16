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

        [Header("Projectile")]
        [SerializeField] private GameObject _projectilePrefab;
        [SerializeField] private Transform _firePoint;

        [Header("HP Bar")]
        [SerializeField] private Slider _hpSlider;

        [Header("Hit Effect")]
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Color _hitColor = Color.red;
        [SerializeField] private float _hitDuration = 0.1f;

        [Header("Spawn Animation")]
        [SerializeField] private float _spawnDuration = 0.3f;

        private float _currentHealth;
        private float _fireTimer;
        private Enemy _target;

        private Color _originalColor;

        public bool IsAlive => _currentHealth > 0;
        public float Range => _range;

        private void Awake()
        {
            if (_firePoint == null)
                _firePoint = transform;

            _currentHealth = _maxHealth;

            if (_spriteRenderer != null)
                _originalColor = _spriteRenderer.color;

            UpdateHpBar();
        }

        private void Start()
        {
            StartCoroutine(SpawnAnimation());
        }

        private void Update()
        {
            if (!IsAlive) return;
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

        public void TakeDamage(float damage)
        {
            if (!IsAlive) return;

            _currentHealth -= damage;
            _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);

            UpdateHpBar();
            StartCoroutine(HitFlash());

            if (_currentHealth <= 0f)
                Destroy(gameObject);
        }

        private IEnumerator HitFlash()
        {
            if (_spriteRenderer == null) yield break;

            _spriteRenderer.color = _hitColor;
            yield return new WaitForSeconds(_hitDuration);
            _spriteRenderer.color = _originalColor;
        }

        private IEnumerator SpawnAnimation()
        {
            Vector3 targetScale = transform.localScale;
            float time = 0f;

            transform.localScale = Vector3.zero;

            while (time < _spawnDuration)
            {
                float t = time / _spawnDuration;
                transform.localScale = Vector3.Lerp(Vector3.zero, targetScale, t);
                time += Time.deltaTime;
                yield return null;
            }

            transform.localScale = targetScale;
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
        }
    }
}