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

        [Header("Налаштування шкоди")]
        [SerializeField] private int _damageAmount = 20; // Скільки HP зніме курка, коли пролетить крізь приціл

        private Vector2 _moveDirection;
        private Vector2 _baseDirection;
        private float _speed;
     
        public event Action<int> OnDeath;

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
            Fly();
        }

        private void Fly()
        {
            transform.Translate(_moveDirection * _speed * Time.deltaTime);
        }

        public void OnShoot()
        {
            OnDeath?.Invoke(_points);
            Destroy(gameObject);
        }

        // Взаємодія через Тригер (щоб курка пролітала крізь приціл, не відбиваючись, і наносила шкоду)
        private void OnTriggerEnter2D(Collider2D collision)
        {
            // Якщо курка перетинає об'єкт із тегом Player (наш приціл)
            if (collision.CompareTag("Player"))
            {
                // Шукаємо менеджер гри та знімаємо здоров'я
                ChickensManager manager = FindObjectOfType<ChickensManager>();
                if (manager != null)
                {
                    manager.TakeDamage(_damageAmount);
                }
            }
        }
    }
}