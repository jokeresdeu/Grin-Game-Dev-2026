using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ChickenHunt
{
    public class Chicken : MonoBehaviour, IShootable
    {
        [Header("Points")]
        [SerializeField] private int _points = 100;

        [Header("Movement")]
        [SerializeField] private float _minSpeed = 2f;
        [SerializeField] private float _maxSpeed = 5f;

        [Header("Wavy Flight Settings")]
        [SerializeField] private float _amplitude = 0.5f;
        [SerializeField] private float _frequency = 2f;

        [Header("Visual")]
        [SerializeField] private SpriteRenderer _spriteRenderer;

        private Vector2 _baseDirection;
        private float _speed;
        private float _startTime;
        private bool _isDead = false;
        private Animator _animator;
        private Rigidbody2D _rb;

        public event Action<int> OnDeath;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _rb = GetComponent<Rigidbody2D>();
        }
        public void Initialize(Vector2 flyDirection)
        {
            _speed = Random.Range(_minSpeed, _maxSpeed);
            _baseDirection = flyDirection.normalized;
            _startTime = Time.time;
            // Гравітація на 0, щоб пташка не падала під час польоту
            if (_rb != null) _rb.gravityScale = 0f;

            if (_spriteRenderer != null)
            {
                _spriteRenderer.enabled = true;
                _spriteRenderer.flipX = _baseDirection.x < 0;
            }
        }
        private void Update()
        {
            if (!_isDead)
            {
                Fly();
            }
        }
        private void Fly()
        {
            Vector3 forwardMove = (Vector3)_baseDirection * _speed * Time.deltaTime;
            float timePassed = Time.time - _startTime;
            float waveOffsetY = Mathf.Cos(timePassed * _frequency) * _amplitude * _frequency * Time.deltaTime;

            Vector3 finalMovement = forwardMove;
            finalMovement.y += waveOffsetY;

            transform.Translate(finalMovement);
        }
        public void OnShoot()
        {
            if (_isDead) return;
            _isDead = true;

            OnDeath?.Invoke(_points);
            // Активуємо тригер анімації смерті
            if (_animator != null)
            {
                _animator.SetTrigger("DoDie");
            }
            // Вмикаємо гравітацію для фізичного падіння
            if (_rb != null)
            {
                _rb.gravityScale = 3f;
            }
            // Знищуємо об'єкт через 0.6 сек
            Destroy(gameObject, 0.6f);
        }
    }
}