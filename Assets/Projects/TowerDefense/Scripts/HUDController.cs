using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Projects.TowerDefense.Scripts
{
    /// <summary>
    /// Polls the gameplay systems each frame and updates the HUD: gold, base HP bar + text,
    /// current level/wave, and the centered level/wave banner. Mirrors OrbitGunner's HUD.
    /// </summary>
    public class HUDController : MonoBehaviour
    {
        [SerializeField] private TMP_Text _goldText;
        [SerializeField] private TMP_Text _hpText;
        [SerializeField] private Image _hpFill;
        [SerializeField] private TMP_Text _levelText;
        [SerializeField] private TMP_Text _bannerText;

        private int _lastGold = int.MinValue;
        private int _lastHp = int.MinValue;
        private int _lastMaxHp = int.MinValue;
        private int _lastLevel = int.MinValue;
        private int _lastWave = int.MinValue;
        private string _lastBanner = null;

        private void Update()
        {
            if (ResourceManager.Instance != null && _goldText != null && ResourceManager.Instance.Gold != _lastGold)
            {
                _lastGold = ResourceManager.Instance.Gold;
                _goldText.text = $"Золото: {_lastGold}";
            }

            if (BaseHealth.Instance != null)
            {
                if (_hpFill != null)
                    _hpFill.fillAmount = BaseHealth.Instance.Normalized;

                if (_hpText != null &&
                    (BaseHealth.Instance.CurrentHealth != _lastHp || BaseHealth.Instance.MaxHealth != _lastMaxHp))
                {
                    _lastHp = BaseHealth.Instance.CurrentHealth;
                    _lastMaxHp = BaseHealth.Instance.MaxHealth;
                    _hpText.text = $"Цитадель: {_lastHp}/{_lastMaxHp}";
                }
            }

            if (LevelManager.Instance != null && _levelText != null)
            {
                int level = LevelManager.Instance.LevelIndex + 1;
                int wave = LevelManager.Instance.WaveIndex + 1;
                if (level != _lastLevel || wave != _lastWave)
                {
                    _lastLevel = level;
                    _lastWave = wave;
                    _levelText.text = $"Рівень {level}/{LevelManager.Instance.LevelCount} · Хвиля {wave}/{LevelManager.Instance.WaveCount}";
                }
            }

            if (LevelManager.Instance != null && _bannerText != null)
            {
                string banner = LevelManager.Instance.BannerText;
                if (banner != _lastBanner)
                {
                    _lastBanner = banner;
                    _bannerText.text = banner;
                }

                if (!string.IsNullOrEmpty(banner))
                {
                    float t = (Mathf.Sin(Time.unscaledTime * 3f) + 1f) * 0.5f;
                    Color c = _bannerText.color;
                    c.a = Mathf.Lerp(0.55f, 1f, t);
                    _bannerText.color = c;
                }
                else if (_bannerText.color.a != 0f)
                {
                    Color c = _bannerText.color;
                    c.a = 0f;
                    _bannerText.color = c;
                }
            }
        }
    }
}
