using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UkraineVsZombies
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private float _maxHealth = 50f;
        [SerializeField] private float _moveSpeed = 2f;
        [SerializeField] private float _attackDamage = 10f;
        [SerializeField] private float _attackRate = 1f;

        [SerializeField] private Slider _hpSlider;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Animator _animator;

        [SerializeField] private Color _hitColor = Color.red;
        [SerializeField] private float _hitDuration = 0.15f;

        [SerializeField] private float _deathScaleMultiplier = 1.5f;
        [SerializeField] private float _deathDuration = 0.3f;

        [SerializeField] private float _missXPosition = -10f;

        private float _currentHealth;
        private float _attackTimer;
        private Tower _targetTower;
        private bool _wasKilled;

        private Color _originalColor;

        public event Action OnDeath;
        public bool IsAlive => _currentHealth > 0;

        public void Initialize()
        {
            _currentHealth = _maxHealth;
            UpdateHPBar();

            if (_spriteRenderer != null)
                _originalColor = _spriteRenderer.color;
        }

        private void Update()
        {
            if (!IsAlive) return;

            CheckMiss();

            if (_targetTower != null && _targetTower.IsAlive)
                Attack();
            else
                Move();
        }

        private void CheckMiss()
        {
            if (_wasKilled) return;

            if (transform.position.x <= _missXPosition)
            {
                _wasKilled = true;
                GameManager.Instance.EnemyMissed();
                Destroy(gameObject);
            }
        }

        private void Move()
        {
            if (_targetTower != null)
            {
                SetAnim(0f);
                return;
            }

            transform.position += Vector3.left * _moveSpeed * Time.deltaTime;
            SetAnim(_moveSpeed);
        }

        private void Attack()
        {
            SetAnim(0f);

            _attackTimer -= Time.deltaTime;

            if (_attackTimer > 0f) return;

            if (_targetTower != null && _targetTower.IsAlive)
                _targetTower.TakeDamage(_attackDamage);

            _attackTimer = 1f / _attackRate;
        }

        private void SetAnim(float speed)
        {
            if (_animator != null)
                _animator.SetFloat("Speed", Mathf.Abs(speed));
        }

        public void TakeDamage(float damage)
        {
            if (!IsAlive) return;

            _currentHealth -= damage;
            _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);

            UpdateHPBar();
            StartCoroutine(HitFlash());

            if (_currentHealth <= 0f)
            {
                _wasKilled = true;
                OnDeath?.Invoke();
                StartCoroutine(DeathAnimation());
            }
        }

        private IEnumerator HitFlash()
        {
            if (_spriteRenderer == null) yield break;

            _spriteRenderer.color = _hitColor;
            yield return new WaitForSeconds(_hitDuration);
            _spriteRenderer.color = _originalColor;
        }

        private IEnumerator DeathAnimation()
        {
            _moveSpeed = 0f;

            Vector3 startScale = transform.localScale;
            Vector3 targetScale = startScale * _deathScaleMultiplier;

            float time = 0f;
            Color startColor = _spriteRenderer != null ? _spriteRenderer.color : Color.white;

            while (time < _deathDuration)
            {
                float t = time / _deathDuration;

                transform.localScale = Vector3.Lerp(startScale, targetScale, t);

                if (_spriteRenderer != null)
                {
                    Color c = startColor;
                    c.a = Mathf.Lerp(1f, 0f, t);
                    _spriteRenderer.color = c;
                }

                time += Time.deltaTime;
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
            if (other.TryGetComponent<Tower>(out var tower))
            {
                if (tower.IsAlive)
                {
                    _targetTower = tower;
                    _attackTimer = 0f;
                }
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.TryGetComponent<Tower>(out var tower))
            {
                if (_targetTower == tower)
                    _targetTower = null;
            }
        }
    }
}