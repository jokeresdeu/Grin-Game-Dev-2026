using System;
using UnityEditor.Animations;
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
        
        [Header("Visual")]
        [SerializeField] private SpriteRenderer _spriteRenderer;

        [SerializeField] private int _maxHp;

        private ChickenAnimationState _currentAnimation;
        
        private int _currentHp;
        
        private Vector2 _moveDirection;
        private Vector2 _baseDirection;
        private float _speed;
       
        public event Action<int> OnDeath;

        private void Start()
        {
            _currentHp = _maxHp;
            Initialize(Vector2.right);
        }
        
        public void Initialize(Vector2 flyDirection)
        {
            _currentHp = _maxHp;
            PlayAnimation(ChickenAnimationState.Fly);
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
            if(state <= _currentAnimation)
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
