using System;
using UnityEngine;

namespace Projects.TowerDefense.Scripts
{
    /// <summary>
    /// The target the player defends (the Citadel). Loses HP when an enemy leaks through;
    /// at zero it triggers a loss. Reset to full at the start of each level. Mirrors
    /// OrbitGunner's CoreHealth.
    /// </summary>
    public class BaseHealth : MonoBehaviour
    {
        public static BaseHealth Instance { get; private set; }

        public event Action<int, int> HealthChanged;

        [SerializeField] private SpriteRenderer _renderer;
        [SerializeField] private int _maxHealth = 20;
        [SerializeField] private float _flashDuration = 0.18f;

        public int CurrentHealth { get; private set; }
        public int MaxHealth => _maxHealth;
        public float Normalized => _maxHealth > 0 ? (float)CurrentHealth / _maxHealth : 0f;

        private Color _baseColor = Color.white;
        private float _flash;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            CurrentHealth = _maxHealth;
            if (_renderer != null)
                _baseColor = _renderer.color;
        }

        private void Start()
        {
            HealthChanged?.Invoke(CurrentHealth, _maxHealth);
        }

        public void ResetFull()
        {
            CurrentHealth = _maxHealth;
            _flash = 0f;
            HealthChanged?.Invoke(CurrentHealth, _maxHealth);
        }

        public void TakeDamage(int amount)
        {
            if (CurrentHealth <= 0)
                return;

            if (GameManager.Instance != null && GameManager.Instance.State != GameState.Playing)
                return;

            CurrentHealth = Mathf.Max(0, CurrentHealth - amount);
            _flash = _flashDuration;
            HealthChanged?.Invoke(CurrentHealth, _maxHealth);

            if (CurrentHealth <= 0 && GameManager.Instance != null)
                GameManager.Instance.TriggerLose();
        }

        private void Update()
        {
            if (_renderer == null)
                return;

            if (_flash > 0f)
            {
                _flash -= Time.deltaTime;
                float t = Mathf.Clamp01(_flash / _flashDuration);
                _renderer.color = Color.Lerp(_baseColor, Color.white, t);
            }
            else
            {
                _renderer.color = _baseColor;
            }
        }

        private void OnDestroy()
        {
            if (Instance == this)
                Instance = null;
        }
    }
}
