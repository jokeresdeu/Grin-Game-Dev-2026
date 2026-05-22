using System;
using UnityEngine;

namespace ClassicPlatformer
{
    public class PlayerHealth : MonoBehaviour
    {
        [Header("HP")]
        [SerializeField] private int _maxHealth = 3;

        [Header("Invincibility Frames")]
        [SerializeField] private float _invincibilityDuration = 1.5f;

        private int _currentHealth;
        private float _invincibilityTimer;
        private bool _isInvincible;

        public int CurrentHealth => _currentHealth;
        public int MaxHealth => _maxHealth;
        public bool IsInvincible => _isInvincible;

        public event Action<int, int> OnHealthChanged; // current, max
        public event Action OnDeath;

        private void Awake()
        {
            _currentHealth = _maxHealth;
        }

        private void Start()
        {
            OnHealthChanged?.Invoke(_currentHealth, _maxHealth);
        }

        private void Update()
        {
            if (!_isInvincible) return;

            _invincibilityTimer -= Time.deltaTime;
            if (_invincibilityTimer <= 0f)
                _isInvincible = false;
        }

        // OnCollisionEnter2D / OnTriggerEnter2D з ворогом або небезпекою
        public void TakeDamage(int damage = 1)
        {
            if (_isInvincible || _currentHealth <= 0) return;

            _currentHealth = Mathf.Max(_currentHealth - damage, 0);
            OnHealthChanged?.Invoke(_currentHealth, _maxHealth);

            if (_currentHealth <= 0)
            {
                OnDeath?.Invoke();
                return;
            }

            // Hurt анімація через PlayerController
            if (TryGetComponent(out PlayerController ctrl))
                ctrl.TriggerHurtAnim();

            // Invincibility frames
            _isInvincible = true;
            _invincibilityTimer = _invincibilityDuration;
        }

        public void InstantKill()
        {
            _currentHealth = 0;
            OnHealthChanged?.Invoke(0, _maxHealth);
            OnDeath?.Invoke();
        }

        public void Heal(int amount = 1)
        {
            if (_currentHealth <= 0) return;
            _currentHealth = Mathf.Min(_currentHealth + amount, _maxHealth);
            OnHealthChanged?.Invoke(_currentHealth, _maxHealth);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.TryGetComponent(out Enemy enemy))
                TakeDamage(1);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            // Небезпечні зони (Obstacle тег або компонент)
            if (other.CompareTag("Hazard"))
                TakeDamage(1);
        }
    }
}
