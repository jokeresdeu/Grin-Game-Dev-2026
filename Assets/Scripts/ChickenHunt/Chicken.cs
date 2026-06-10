using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ChickenHunt
{
    public class Chicken : MonoBehaviour, IShootable
    {
        [SerializeField] private Animator _animator;

        [Header("Points")]
        [SerializeField] private int _points = 100;

        [Header("Movement")]
        [SerializeField] private float _minSpeed = 2f;
        [SerializeField] private float _maxSpeed = 5f;

        [Header("Wiggle")]
        [SerializeField] private float _minWiggleAmplitude = 0.5f;
        [SerializeField] private float _maxWiggleAmplitude = 2f;
        [SerializeField] private float _minWiggleFrequency = 1f;
        [SerializeField] private float _maxWiggleFrequency = 3f;

        [Header("Visual")]
        [SerializeField] private SpriteRenderer _spriteRenderer;

        [SerializeField] private int _maxHp;

        private ChickenAnimationState _currentAnimation;

        private int _currentHp;

        private Vector2 _baseDirection;
        private Vector2 _perpendicularDirection;
        private float _speed;

        private float _wiggleAmplitude;
        private float _wiggleFrequency;
        private float _timeOffset;

        public event Action<int> OnDeath;

        private void Start()
        {
            _currentHp = _maxHp;
        }

        public void Initialize(Vector2 flyDirection)
        {
            _currentHp = _maxHp;

            _baseDirection = flyDirection.normalized;

            // Перпендикуляр (для вилянь)
            _perpendicularDirection = new Vector2(-_baseDirection.y, _baseDirection.x);

            _speed = Random.Range(_minSpeed, _maxSpeed);

            // Рандомні параметри виляння
            _wiggleAmplitude = Random.Range(_minWiggleAmplitude, _maxWiggleAmplitude);
            _wiggleFrequency = Random.Range(_minWiggleFrequency, _maxWiggleFrequency);

            // щоб всі курки не синхронно виляли
            _timeOffset = Random.Range(0f, 100f);

            PlayAnimation(ChickenAnimationState.Fly);

            if (_spriteRenderer != null)
            {
                _spriteRenderer.enabled = true;
                _spriteRenderer.flipX = _baseDirection.x < 0;
            }
        }

        private void Update()
        {
            Fly();
        }

        private void Fly()
        {
            float time = Time.time + _timeOffset;

            // синусоїдальне відхилення
            float wiggle = Mathf.Sin(time * _wiggleFrequency) * _wiggleAmplitude;

            Vector2 finalDirection = _baseDirection + _perpendicularDirection * wiggle;

            transform.Translate(finalDirection.normalized * _speed * Time.deltaTime);
        }

        public void OnShoot(int damage)
        {
            _currentHp -= damage;

            if (_currentHp <= 0)
            {
                OnDeath?.Invoke(_points);
                Destroy(gameObject);
                return;
            }

            PlayAnimation(ChickenAnimationState.Hurt);
        }

        private void PlayAnimation(ChickenAnimationState state)
        {
            if (state <= _currentAnimation)
                return;

            _currentAnimation = state;
            _animator.Play(state.ToString());
        }

        public void EndAnimation()
        {
            switch (_currentAnimation)
            {
                case ChickenAnimationState.Hurt:
                    _currentAnimation = _speed > 0 ? ChickenAnimationState.Fly : ChickenAnimationState.Idle;
                    break;

                case ChickenAnimationState.Fly:
                    _currentAnimation = ChickenAnimationState.Idle;
                    break;
            }

            _animator.Play(_currentAnimation.ToString());
        }
    }
}

public enum ChickenAnimationState
{
    None = 0,
    Idle = 1,
    Fly = 2,
    Hurt = 3
}