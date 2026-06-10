using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ChickenHunt
{
    public class Chicken : MonoBehaviour, IShootable
    {
        [Header("Health")]
        [SerializeField] private int _maxHp = 3;
        [SerializeField] private ChickenHealthBar _healthBar;

        [Header("Points")]
        [SerializeField] private int _points = 100;

        [Header("Movement")]
        [SerializeField] private float _minSpeed = 2f;
        [SerializeField] private float _maxSpeed = 5f;

        [Header("Visual")]
        [SerializeField] private SpriteRenderer _spriteRenderer;

        [Header("Animation")]
        [SerializeField] private ChickenAnimationController _animationController;
        [SerializeField] private float _deathDelay = 0.7f;

        private int _currentHp;
        private Vector2 _moveDirection;
        private Vector2 _baseDirection;
        private float _speed;
        private bool _isDead;

        public event Action<int> OnDeath;

        private void Awake()
        {
            if (_animationController == null)
                _animationController = GetComponent<ChickenAnimationController>();

            if (_spriteRenderer == null)
                _spriteRenderer = GetComponentInChildren<SpriteRenderer>();

            _currentHp = _maxHp;

            if (_healthBar != null)
                _healthBar.SetValue(_currentHp, _maxHp);
        }

        public void Initialize(Vector2 flyDirection)
        {
            _isDead = false;

            _speed = Random.Range(_minSpeed, _maxSpeed);
            _baseDirection = flyDirection.normalized;
            _moveDirection = _baseDirection;

            if (_spriteRenderer != null)
            {
                _spriteRenderer.enabled = true;
                _spriteRenderer.flipX = _moveDirection.x < 0;
            }

            Collider2D collider2D = GetComponent<Collider2D>();

            if (collider2D != null)
                collider2D.enabled = true;

            _currentHp = _maxHp;

            if (_healthBar != null)
                _healthBar.SetValue(_currentHp, _maxHp);
        }

        private void Update()
        {
            if (_isDead)
                return;

            if (GameManager.Instance != null && !GameManager.Instance.IsPlaying)
                return;

            Fly();
        }

        private void Fly()
        {
            transform.Translate(_moveDirection * _speed * Time.deltaTime);
        }

        public void OnShoot()
        {
            if (_isDead)
                return;

            if (GameManager.Instance != null && !GameManager.Instance.IsPlaying)
                return;

            _currentHp--;

            if (_currentHp < 0)
                _currentHp = 0;

            if (_healthBar != null)
                _healthBar.SetValue(_currentHp, _maxHp);

            if (_currentHp > 0)
            {
                if (_animationController != null)
                    _animationController.PlayHit();

                return;
            }

            Die();
        }

        private void Die()
        {
            if (_isDead)
                return;

            _isDead = true;
            _moveDirection = Vector2.zero;
            _speed = 0f;

            Collider2D collider2D = GetComponent<Collider2D>();

            if (collider2D != null)
                collider2D.enabled = false;

            if (_animationController != null)
                _animationController.PlayDeath();

            StartCoroutine(DeathRoutine());
        }

        private IEnumerator DeathRoutine()
        {
            yield return new WaitForSecondsRealtime(_deathDelay);

            if (_spriteRenderer != null)
                _spriteRenderer.enabled = false;

            if (GameManager.Instance != null)
                GameManager.Instance.AddScore(_points);

            OnDeath?.Invoke(_points);

            Destroy(gameObject);
        }
    }
}