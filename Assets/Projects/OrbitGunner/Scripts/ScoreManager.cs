using System;
using UnityEngine;

namespace Projects.OrbitGunner.Scripts
{

    public class ScoreManager : MonoBehaviour
    {
        private const string BestScoreKey = "OrbitGunner.BestScore";

        public static ScoreManager Instance { get; private set; }

        public event Action<int> ScoreChanged;
        public event Action<int> BestScoreChanged;
        public event Action<int> ComboChanged;

        [SerializeField] private float _comboWindow = 2.5f;
        [SerializeField] private int _killsPerComboStep = 3;
        [SerializeField] private int _maxComboMultiplier = 8;

        public int Score { get; private set; }
        public int BestScore { get; private set; }
        public int ComboMultiplier { get; private set; } = 1;

        public bool NewBestThisRun { get; private set; }

        private int _comboKills;
        private float _comboTimer;

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
            if (_comboTimer <= 0f)
                return;

            if (GameManager.Instance != null && GameManager.Instance.State != GameState.Playing)
                return;

            _comboTimer -= Time.deltaTime;
            if (_comboTimer <= 0f)
                ResetCombo();
        }

        public void RegisterKill(int baseValue)
        {
            Score += baseValue * ComboMultiplier;
            ScoreChanged?.Invoke(Score);

            _comboKills++;
            _comboTimer = _comboWindow;

            int newMultiplier = Mathf.Clamp(1 + _comboKills / Mathf.Max(1, _killsPerComboStep), 1, _maxComboMultiplier);
            if (newMultiplier != ComboMultiplier)
            {
                ComboMultiplier = newMultiplier;
                ComboChanged?.Invoke(ComboMultiplier);
            }
        }

        public void NotifyCoreHit()
        {
            ResetCombo();
        }

        public void ResetCombo()
        {
            _comboKills = 0;
            _comboTimer = 0f;
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
            NewBestThisRun = true;
            PlayerPrefs.SetInt(BestScoreKey, BestScore);
            PlayerPrefs.Save();
            BestScoreChanged?.Invoke(BestScore);
        }

        private void OnDestroy()
        {
            if (Instance == this)
                Instance = null;
        }
    }
}
