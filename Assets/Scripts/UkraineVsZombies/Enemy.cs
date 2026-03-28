using System;
using UnityEngine;
using UnityEngine.UI;

namespace UkraineVsZombies
{
    public class Enemy : MonoBehaviour
    {
        [Header("Stats")]
        [SerializeField] private float _maxHealth = 150f;
        [SerializeField] private float _moveSpeed = 2f;
        [SerializeField] private float _attackDamage = 10f;
        [SerializeField] private float _attackRate = 1f;

        [Header("HP Bar")]
        [SerializeField] private Slider _hpSlider;

        private float _currentHealth;
        private float _attackTimer;
        private Tower _targetTower;
        private Transform _wallTransform;

        public event Action OnDeath;
        public bool IsAlive => _currentHealth > 0;

        public void Initialize()
        {
            _currentHealth = _maxHealth;
            UpdateHPBar();

            if (GameManager.Instance != null && GameManager.Instance.GameOverWallTransform != null)
            {
                _wallTransform = GameManager.Instance.GameOverWallTransform;
            }
        }

        private void Awake()
        {
            if (_currentHealth <= 0f) Initialize();
        }

        private void Update()
        {
            if (!IsAlive) return;

            if (_wallTransform != null && transform.position.x < _wallTransform.position.x)
            {
                GameManager.Instance.GameOver();
                return;
            }

            CheckForTower();

            if (_targetTower != null && _targetTower.IsAlive)
            {
                Attack();
            }
            else
            {
                Move();
            }
        }

        private void CheckForTower()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.5f);

            foreach (var col in colliders)
            {
                Tower tower = col.GetComponent<Tower>();
                if (tower != null && tower.IsAlive)
                {
                    _targetTower = tower;
                    return;
                }
            }
            _targetTower = null;
        }

        private void Move()
        {
            transform.position += Vector3.left * _moveSpeed * Time.deltaTime;
        }

        private void Attack()
        {
            _attackTimer -= Time.deltaTime;
            if (_attackTimer <= 0f)
            {
                _targetTower.TakeDamage(_attackDamage);
                _attackTimer = 1f / _attackRate;
            }
        }

        public void TakeDamage(float damage)
        {
            if (!IsAlive) return;

            _currentHealth -= damage;
            UpdateHPBar();

            if (_currentHealth <= 0f)
            {
                if (GameManager.Instance != null)
                    GameManager.Instance.AddScore(10);

                OnDeath?.Invoke();
                Destroy(gameObject);
            }
        }

        private void UpdateHPBar()
        {
            if (_hpSlider != null)
                _hpSlider.value = _currentHealth / _maxHealth;
        }
    }
}