using System;
using UnityEngine;

namespace Projects.OrbitGunner.Scripts
{
    public class CoreHealth : MonoBehaviour
    {
        public static CoreHealth Instance { get; private set; }

        public event Action<int, int> HealthChanged;
        public event Action Died;

        [SerializeField] private SpriteRenderer _renderer;
        [SerializeField] private float _radius = 0.62f;
        [SerializeField] private int _maxHealth = 5;
        [SerializeField] private float _flashDuration = 0.18f;

        public int CurrentHealth { get; private set; }
        public int MaxHealth => _maxHealth;
        public float Radius => _radius;
        public float Normalized => _maxHealth > 0 ? (float)CurrentHealth / _maxHealth : 0f;

        private Color _baseColor = Color.white;
        private float _flashRemaining;

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

        public void TakeDamage(int amount)
        {
            if (CurrentHealth <= 0)
                return;

            if (GameManager.Instance != null && GameManager.Instance.State != GameState.Playing)
                return;

            CurrentHealth = Mathf.Max(0, CurrentHealth - amount);
            HealthChanged?.Invoke(CurrentHealth, _maxHealth);

            _flashRemaining = _flashDuration;

            if (ScoreManager.Instance != null)
                ScoreManager.Instance.NotifyCoreHit();

            if (CameraShake.Instance != null)
                CameraShake.Instance.Shake(0.55f);

            if (CurrentHealth <= 0)
            {
                Died?.Invoke();
                if (GameManager.Instance != null)
                    GameManager.Instance.TriggerGameOver();
            }
        }

        private void Update()
        {
            if (_renderer == null)
                return;

            if (_flashRemaining > 0f)
            {
                _flashRemaining -= Time.deltaTime;
                float t = Mathf.Clamp01(_flashRemaining / _flashDuration);
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
