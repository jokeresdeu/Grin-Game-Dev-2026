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

        [Header("Armor (Pan)")]
        [SerializeField] private GameObject _panObject;
        [SerializeField] private bool _hasArmor = false;

        private Vector2 _moveDirection;
        private float _speed;

        public event Action<int> OnDeath;

        public void Initialize(Vector2 flyDirection)
        {
            _speed = Random.Range(_minSpeed, _maxSpeed);
            _moveDirection = flyDirection.normalized;

            if (_spriteRenderer != null)
            {
                _spriteRenderer.enabled = true;
                _spriteRenderer.flipX = _moveDirection.x < 0;
            }

            if (_panObject != null)
                _panObject.SetActive(_hasArmor);
        }

        private void Update()
        {
            transform.Translate(_moveDirection * _speed * Time.deltaTime);
        }

        public void OnShoot()
        {
            if (_hasArmor)
            {
                _hasArmor = false;

                if (_panObject != null)
                    _panObject.SetActive(false);

                return;
            }

            OnDeath?.Invoke(_points);
            Destroy(gameObject);
        }
    }
}