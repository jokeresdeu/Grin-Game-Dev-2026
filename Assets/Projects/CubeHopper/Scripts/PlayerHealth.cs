using System;
using UnityEngine;

namespace Projects.CubeHopper.Scripts
{
    public class PlayerHealth : MonoBehaviour
    {
        public event Action<int> LivesChanged;
        public event Action Damaged;
        public event Action Died;

        [SerializeField] private int _maxLives = 3;
        [SerializeField] private float _invincibilitySeconds = 1.2f;
        [SerializeField] private SpriteRenderer _renderer;
        [SerializeField] private float _flashFrequency = 12f;

        public int Lives { get; private set; }
        public int MaxLives => _maxLives;
        public bool IsInvincible => _invincibilityRemaining > 0f;

        private float _invincibilityRemaining;

        private void Awake()
        {
            Lives = _maxLives;
        }

        private void Start()
        {
            LivesChanged?.Invoke(Lives);
        }

        private void Update()
        {
            if (_invincibilityRemaining > 0f)
            {
                _invincibilityRemaining -= Time.deltaTime;
                if (_renderer != null)
                {
                    float a = Mathf.Abs(Mathf.Sin(Time.time * _flashFrequency)) * 0.6f + 0.4f;
                    Color c = _renderer.color;
                    c.a = a;
                    _renderer.color = c;
                }

                if (_invincibilityRemaining <= 0f && _renderer != null)
                {
                    Color c = _renderer.color;
                    c.a = 1f;
                    _renderer.color = c;
                }
            }
        }

        public bool TryDamage()
        {
            if (IsInvincible || Lives <= 0)
                return false;

            Lives = Mathf.Max(0, Lives - 1);
            LivesChanged?.Invoke(Lives);
            Damaged?.Invoke();

            if (Lives <= 0)
            {
                Died?.Invoke();
                return true;
            }

            _invincibilityRemaining = _invincibilitySeconds;
            return true;
        }
    }
}
