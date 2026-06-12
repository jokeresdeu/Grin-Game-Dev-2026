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
        [SerializeField] private int _damageAmount = 20; 

        private Vector2 _moveDirection;
        private Vector2 _baseDirection;
        private float _speed;
        
        private Animator _animator;
        private bool _isDead = false;
     
        public event Action<int> OnDeath;

        private void Start()
        {
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
            if (_isDead) return;
            Fly();
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

            if (_animator != null)
            {
                _animator.SetTrigger("Die");
            }

            Collider2D col = GetComponent<Collider2D>();
            if (col != null) col.enabled = false;

            Destroy(gameObject, 0.5f);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (_isDead) return;

            if (collision.CompareTag("Player"))
            {
                ChickensManager manager = FindObjectOfType<ChickensManager>();
                if (manager != null)
                {
                    manager.TakeDamage(_damageAmount);
                }
            }
        }
    }
}