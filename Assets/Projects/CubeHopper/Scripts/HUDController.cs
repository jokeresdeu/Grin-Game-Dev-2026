using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Projects.CubeHopper.Scripts
{
    public class HUDController : MonoBehaviour
    {
        [SerializeField] private TMP_Text _scoreLabel;
        [SerializeField] private TMP_Text _bestScoreLabel;
        [SerializeField] private TMP_Text _comboLabel;
        [SerializeField] private Image[] _lifeIcons;
        [SerializeField] private Color _lifeActiveColor = Color.white;
        [SerializeField] private Color _lifeLostColor = new Color(1f, 1f, 1f, 0.2f);
        [SerializeField] private PlayerHealth _health;

        private bool _isScoreSubscribed;
        private bool _isHealthSubscribed;
        private int _lastScore = int.MinValue;
        private int _lastBestScore = int.MinValue;
        private int _lastCombo = int.MinValue;
        private int _lastLives = int.MinValue;

        private void OnEnable()
        {
            TrySubscribe();
            ForcePushAllValues();
        }

        private void Start()
        {
            TrySubscribe();
            ForcePushAllValues();
        }

        private void OnDisable()
        {
            if (_isScoreSubscribed && ScoreManager.Instance != null)
            {
                ScoreManager.Instance.ScoreChanged -= OnScoreChanged;
                ScoreManager.Instance.BestScoreChanged -= OnBestScoreChanged;
                ScoreManager.Instance.ComboChanged -= OnComboChanged;
                _isScoreSubscribed = false;
            }

            if (_isHealthSubscribed && _health != null)
            {
                _health.LivesChanged -= OnLivesChanged;
                _isHealthSubscribed = false;
            }
        }

        private void Update()
        {
            if (!_isScoreSubscribed || !_isHealthSubscribed)
                TrySubscribe();

            PollIfChanged();
        }

        private void TrySubscribe()
        {
            if (!_isScoreSubscribed && ScoreManager.Instance != null)
            {
                ScoreManager.Instance.ScoreChanged += OnScoreChanged;
                ScoreManager.Instance.BestScoreChanged += OnBestScoreChanged;
                ScoreManager.Instance.ComboChanged += OnComboChanged;
                _isScoreSubscribed = true;
            }

            if (!_isHealthSubscribed && _health != null)
            {
                _health.LivesChanged += OnLivesChanged;
                _isHealthSubscribed = true;
            }
        }

        private void ForcePushAllValues()
        {
            if (ScoreManager.Instance != null)
            {
                OnScoreChanged(ScoreManager.Instance.Score);
                OnBestScoreChanged(ScoreManager.Instance.BestScore);
                OnComboChanged(ScoreManager.Instance.ComboMultiplier);
            }

            if (_health != null)
                OnLivesChanged(_health.Lives);
        }

        private void PollIfChanged()
        {
            if (ScoreManager.Instance != null)
            {
                if (ScoreManager.Instance.Score != _lastScore)
                    OnScoreChanged(ScoreManager.Instance.Score);
                if (ScoreManager.Instance.BestScore != _lastBestScore)
                    OnBestScoreChanged(ScoreManager.Instance.BestScore);
                if (ScoreManager.Instance.ComboMultiplier != _lastCombo)
                    OnComboChanged(ScoreManager.Instance.ComboMultiplier);
            }

            if (_health != null && _health.Lives != _lastLives)
                OnLivesChanged(_health.Lives);
        }

        private void OnScoreChanged(int score)
        {
            _lastScore = score;
            if (_scoreLabel != null)
                _scoreLabel.text = score.ToString("D6");
        }

        private void OnBestScoreChanged(int best)
        {
            _lastBestScore = best;
            if (_bestScoreLabel != null)
                _bestScoreLabel.text = $"Best {best:D6}";
        }

        private void OnComboChanged(int combo)
        {
            _lastCombo = combo;
            if (_comboLabel == null)
                return;

            if (combo <= 1)
            {
                _comboLabel.gameObject.SetActive(false);
            }
            else
            {
                _comboLabel.gameObject.SetActive(true);
                _comboLabel.text = $"x{combo}";
            }
        }

        private void OnLivesChanged(int lives)
        {
            _lastLives = lives;
            if (_lifeIcons == null)
                return;

            for (int i = 0; i < _lifeIcons.Length; i++)
            {
                if (_lifeIcons[i] == null)
                    continue;

                _lifeIcons[i].color = i < lives ? _lifeActiveColor : _lifeLostColor;
            }
        }
    }
}
