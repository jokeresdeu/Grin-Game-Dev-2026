using System;
using UnityEngine;

namespace Projects.OrbitGunner.Scripts
{

    public class DifficultyDirector : MonoBehaviour
    {
        public static DifficultyDirector Instance { get; private set; }

        public event Action<bool> FrenzyChanged;
        public event Action<int> WaveChanged;

        [Header("Spawn interval (seconds)")]
        [SerializeField] private float _startInterval = 1.3f;
        [SerializeField] private float _minInterval = 0.45f;
        [SerializeField] private float _intervalRampSeconds = 75f;

        [Header("Enemy speed multiplier")]
        [SerializeField] private float _maxSpeedMultiplier = 1.9f;
        [SerializeField] private float _speedRampSeconds = 90f;

        [Header("Waves & frenzy")]
        [SerializeField] private float _secondsPerWave = 15f;
        [SerializeField] private float _frenzyPeriod = 18f;
        [SerializeField] private float _frenzyDuration = 4f;

        public float ElapsedTime { get; private set; }
        public bool IsFrenzy { get; private set; }
        public int Wave { get; private set; } = 1;

        public float CurrentSpawnInterval =>
            Mathf.Lerp(_startInterval, _minInterval, Mathf.Clamp01(ElapsedTime / _intervalRampSeconds));

        public float SpeedMultiplier =>
            Mathf.Lerp(1f, _maxSpeedMultiplier, Mathf.Clamp01(ElapsedTime / _speedRampSeconds));

        private float _frenzyTimer;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            _frenzyTimer = _frenzyPeriod;
        }

        private void Update()
        {
            if (GameManager.Instance != null && GameManager.Instance.State != GameState.Playing)
                return;

            ElapsedTime += Time.deltaTime;

            int wave = 1 + Mathf.FloorToInt(ElapsedTime / _secondsPerWave);
            if (wave != Wave)
            {
                Wave = wave;
                WaveChanged?.Invoke(Wave);
            }

            _frenzyTimer -= Time.deltaTime;

            if (!IsFrenzy && _frenzyTimer <= 0f)
            {
                IsFrenzy = true;
                _frenzyTimer = _frenzyDuration;
                FrenzyChanged?.Invoke(true);
            }
            else if (IsFrenzy && _frenzyTimer <= 0f)
            {
                IsFrenzy = false;
                _frenzyTimer = _frenzyPeriod;
                FrenzyChanged?.Invoke(false);
            }
        }

        private void OnDestroy()
        {
            if (Instance == this)
                Instance = null;
        }
    }
}
