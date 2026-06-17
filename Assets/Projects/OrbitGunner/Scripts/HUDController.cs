using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Projects.OrbitGunner.Scripts
{
    public class HUDController : MonoBehaviour
    {
        [SerializeField] private TMP_Text _scoreText;
        [SerializeField] private TMP_Text _bestText;
        [SerializeField] private TMP_Text _comboText;
        [SerializeField] private TMP_Text _waveText;
        [SerializeField] private Image _hpFill;
        [SerializeField] private Image _overdriveFill;

        private int _lastScore = int.MinValue;
        private int _lastBest = int.MinValue;
        private int _lastCombo = int.MinValue;
        private int _lastWave = int.MinValue;
        private int _lastSeconds = int.MinValue;

        private void Update()
        {
            if (ScoreManager.Instance != null)
            {
                if (ScoreManager.Instance.Score != _lastScore)
                {
                    _lastScore = ScoreManager.Instance.Score;
                    if (_scoreText != null)
                        _scoreText.text = _lastScore.ToString("N0");
                }

                if (ScoreManager.Instance.BestScore != _lastBest)
                {
                    _lastBest = ScoreManager.Instance.BestScore;
                    if (_bestText != null)
                        _bestText.text = $"Рекорд {_lastBest:N0}";
                }

                if (ScoreManager.Instance.ComboMultiplier != _lastCombo)
                {
                    _lastCombo = ScoreManager.Instance.ComboMultiplier;
                    if (_comboText != null)
                        _comboText.text = _lastCombo > 1 ? $"x{_lastCombo} КОМБО" : string.Empty;
                }
            }

            if (CoreHealth.Instance != null && _hpFill != null)
                _hpFill.fillAmount = CoreHealth.Instance.Normalized;

            if (OverdriveMeter.Instance != null && _overdriveFill != null)
                _overdriveFill.fillAmount = OverdriveMeter.Instance.Normalized;

            if (DifficultyDirector.Instance != null && _waveText != null)
            {
                int wave = DifficultyDirector.Instance.Wave;
                int seconds = Mathf.FloorToInt(DifficultyDirector.Instance.ElapsedTime);
                if (wave != _lastWave || seconds != _lastSeconds)
                {
                    _lastWave = wave;
                    _lastSeconds = seconds;
                    _waveText.text = $"Хвиля {wave} · {seconds}с";
                }
            }
        }
    }
}
