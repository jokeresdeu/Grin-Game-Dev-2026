using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ChickenHunt
{
    public class Chicken : MonoBehaviour, IShootable
    {
        [Header("Points")]
        [SerializeField] private int _points = 100;

        [Header("Health")]
        [SerializeField] private int _hitsToKill = 1;

        [Header("Movement")]
        [SerializeField] private float _minSpeed = 2f;
        [SerializeField] private float _maxSpeed = 5f;

        [Header("Visual")]
        [SerializeField] private SpriteRenderer _spriteRenderer;

        private Vector2 _moveDirection;
        private Vector2 _baseDirection;
        private float _speed;
        private int _currentHits;

        public event Action<int> OnDeath;

        public void Initialize(Vector2 flyDirection)
        {
            _speed = Random.Range(_minSpeed, _maxSpeed);
            _baseDirection = flyDirection.normalized;
            _moveDirection = _baseDirection;

            _currentHits = _hitsToKill;

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
            _currentHits--;

            if (_currentHits <= 0)
            {
                OnDeath?.Invoke(_points);
                Destroy(gameObject);
            }
        }
    }
}