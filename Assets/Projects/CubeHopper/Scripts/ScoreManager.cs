using System;
using UnityEngine;

namespace Projects.CubeHopper.Scripts
{
    public class ScoreManager : MonoBehaviour
    {
        private const string BestScoreKey = "CubeHopper.BestScore";

        public static ScoreManager Instance { get; private set; }

        public event Action<int> ScoreChanged;
        public event Action<int> BestScoreChanged;
        public event Action<int> ComboChanged;

        [SerializeField] private float _pointsPerSecond = 10f;
        [SerializeField] private float _comboBoostPerObstacle = 0.5f;
        [SerializeField] private int _maxComboMultiplier = 5;

        public int Score { get; private set; }
        public int BestScore { get; private set; }
        public int ComboMultiplier { get; private set; } = 1;

        private float _scoreAccumulator;
        private float _comboProgress;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            BestScore = PlayerPrefs.GetInt(BestScoreKey, 0);
        }

        private void Start()
        {
            ScoreChanged?.Invoke(Score);
            BestScoreChanged?.Invoke(BestScore);
            ComboChanged?.Invoke(ComboMultiplier);
        }

        private void Update()
        {
            if (GameManager.Instance != null && GameManager.Instance.State != GameState.Playing)
                return;

            _scoreAccumulator += _pointsPerSecond * ComboMultiplier * Time.deltaTime;
            int whole = Mathf.FloorToInt(_scoreAccumulator);
            if (whole > 0)
            {
                _scoreAccumulator -= whole;
                AddScore(whole);
            }
        }

        public void RegisterObstacleCleared()
        {
            _comboProgress += _comboBoostPerObstacle;
            int newMultiplier = Mathf.Clamp(1 + Mathf.FloorToInt(_comboProgress), 1, _maxComboMultiplier);
            if (newMultiplier != ComboMultiplier)
            {
                ComboMultiplier = newMultiplier;
                ComboChanged?.Invoke(ComboMultiplier);
            }
        }

        public void ResetCombo()
        {
            _comboProgress = 0f;
            if (ComboMultiplier != 1)
            {
                ComboMultiplier = 1;
                ComboChanged?.Invoke(ComboMultiplier);
            }
        }

        public void CommitBestScore()
        {
            if (Score <= BestScore)
                return;

            BestScore = Score;
            PlayerPrefs.SetInt(BestScoreKey, BestScore);
            PlayerPrefs.Save();
            BestScoreChanged?.Invoke(BestScore);
        }

        private void AddScore(int amount)
        {
            Score += amount;
            ScoreChanged?.Invoke(Score);
        }

        private void OnDestroy()
        {
            if (Instance == this)
                Instance = null;
        }
    }
}
