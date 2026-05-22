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
        
        [Header("Visual")]
        [SerializeField] private SpriteRenderer _spriteRenderer;
        // ДОДАНО: Посилання на аніматор
        [SerializeField] private Animator _animator;

        private Vector2 _moveDirection;
        private Vector2 _baseDirection;
        private float _speed;
        // ДОДАНО: Прапорець, щоб пташка не реагувала на постріли під час анімації смерті
        private bool _isDead;
     
        public event Action<int> OnDeath;

        // ДОДАНО: Автоматичне отримання аніматору, якщо він не призначений в інспекторі
        private void Awake()
        {
            if (_animator == null)
                _animator = GetComponent<Animator>();
        }

        public void Initialize(Vector2 flyDirection)
        {
            _speed = Random.Range(_minSpeed, _maxSpeed);
            _baseDirection = flyDirection.normalized;
            _moveDirection = _baseDirection;

            if (_spriteRenderer != null)
            {
                _spriteRenderer.enabled = true;
                _spriteRenderer.flipX = _moveDirection.x < 0;
            }
        }

        private void Update()
        {
            // Пташка летить тільки якщо вона ще жива
            if (!_isDead)
            {
                Fly();
            }
        }

        private void Fly()
        {
            transform.Translate(_moveDirection * _speed * Time.deltaTime);
        }

        public void OnShoot()
        {
            if (_isDead) return;

            _isDead = true;
            OnDeath?.Invoke(_points);

            // ЗАПУСК АНІМАЦІЇ:
            if (_animator != null)
            {
                _animator.SetTrigger("Die"); // Викликаємо тригер, що створили в Animator
            }

            // ЗНИЩЕННЯ:
            Destroy(gameObject, 0.3f); 
        }
    }
}