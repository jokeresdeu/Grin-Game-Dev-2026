using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UkraineVsZombies
{
    public class Enemy : MonoBehaviour
    {
        [Header("Stats")]
        [SerializeField] private float _maxHealth = 50f;
        [SerializeField] private float _moveSpeed = 2f;
        [SerializeField] private float _attackDamage = 10f;
        [SerializeField] private float _attackRate = 1f;
        [SerializeField] private int _scoreReward = 1;
        [SerializeField] private int _baseDamage = 1;

        [Header("HP Bar")]
        [SerializeField] private Slider _hpSlider;

        [Header("Animation")]
        [SerializeField] private float _moveBobAmount = 0.08f;
        [SerializeField] private float _moveBobSpeed = 8f;
        [SerializeField] private float _damageFlashTime = 0.12f;
        [SerializeField] private float _deathAnimationTime = 0.25f;

        private float _currentHealth;
        private float _attackTimer;
        private Tower _targetTower;
        private SpriteRenderer _spriteRenderer;
        private Vector3 _startScale;
        private Color _startColor = Color.white;
        private Coroutine _damageFlashRoutine;
        private bool _isDying;

        public event Action OnDeath;
        public bool IsAlive => _currentHealth > 0;

        private void Awake()
        {
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            _startScale = transform.localScale;

            if (_spriteRenderer != null)
                _startColor = _spriteRenderer.color;
        }

        public void Initialize()
        {
            _currentHealth = _maxHealth;
            _isDying = false;
            transform.localScale = _startScale;

            if (_spriteRenderer != null)
                _spriteRenderer.color = _startColor;

            UpdateHPBar();
        }

        private void Update()
        {
            if (!IsAlive) return;

            if (_targetTower != null && _targetTower.IsAlive)
                Attack();
            else
                Move();
        }

        private void Move()
        {
            transform.position += Vector3.left * _moveSpeed * Time.deltaTime;
            AnimateMove();

            if (transform.position.x < -10f)
            {
                if (GameManager.Instance != null)
                    GameManager.Instance.DamageBase(_baseDamage);

                OnDeath?.Invoke();
                Destroy(gameObject);
            }
        }

        private void Attack()
        {
            AnimateAttack();

            _attackTimer -= Time.deltaTime;
            if (_attackTimer <= 0f)
            {
                _targetTower.TakeDamage(_attackDamage);
                _attackTimer = 1f / _attackRate;
            }
        }

        public void TakeDamage(float damage)
        {
            if (!IsAlive || _isDying) return;

            _currentHealth -= damage;
            UpdateHPBar();

            if (_currentHealth <= 0f)
            {
                if (GameManager.Instance != null)
                    GameManager.Instance.AddScore(_scoreReward);

                StartCoroutine(PlayDeathAnimation());
            }
            else
            {
                if (_damageFlashRoutine != null)
                    StopCoroutine(_damageFlashRoutine);

                _damageFlashRoutine = StartCoroutine(PlayDamageFlash());
            }
        }

        private void AnimateMove()
        {
            float bob = Mathf.Sin(Time.time * _moveBobSpeed) * _moveBobAmount;
            transform.localScale = new Vector3(_startScale.x + bob, _startScale.y - bob, _startScale.z);
        }

        private void AnimateAttack()
        {
            float squash = Mathf.Sin(Time.time * 18f) * 0.04f;
            transform.localScale = new Vector3(_startScale.x + squash, _startScale.y - squash, _startScale.z);
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
            _isDying = true;
            _currentHealth = 0f;
            UpdateHPBar();
            OnDeath?.Invoke();

            float timer = 0f;
            Vector3 fromScale = transform.localScale;

            while (timer < _deathAnimationTime)
            {
                timer += Time.deltaTime;
                float t = timer / _deathAnimationTime;
                transform.localScale = Vector3.Lerp(fromScale, Vector3.zero, t);

                if (_spriteRenderer != null)
                    _spriteRenderer.color = Color.Lerp(_startColor, Color.clear, t);

                yield return null;
            }

            Destroy(gameObject);
        }

        private void UpdateHPBar()
        {
            if (_hpSlider != null)
                _hpSlider.value = _currentHealth / _maxHealth;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            var tower = other.GetComponent<Tower>();
            if (tower != null && tower.IsAlive)
                _targetTower = tower;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            var tower = other.GetComponent<Tower>();
            if (tower != null && tower == _targetTower)
                _targetTower = null;
        }
    }
}
