using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace ClassicPlatformer
{
    public class PlayerHealthUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Slider _healthSlider;
        [SerializeField] private TextMeshProUGUI _healthLabel;
        [SerializeField] private Image _fillImage;

        [Header("Colors")]
        [SerializeField] private Color _highHealthColor = Color.green;
        [SerializeField] private Color _midHealthColor = Color.yellow;
        [SerializeField] private Color _lowHealthColor = Color.red;

        private Player _player;

        private void Update()
        {
            if (_player == null)
            {
                _player = FindFirstObjectByType<Player>();
                if (_player == null) return;

                if (_healthSlider != null)
                {
                    _healthSlider.minValue = 0;
                    _healthSlider.maxValue = _player.MaxHealth;
                }
            }

            Refresh();
        }

        private void Refresh()
        {
            int hp = _player.CurrentHealth;
            int maxHp = _player.MaxHealth;

            if (_healthSlider != null)
                _healthSlider.value = hp;

            if (_healthLabel != null)
                _healthLabel.text = $"{hp} / {maxHp}";

            if (_fillImage != null)
            {
                float ratio = (float)hp / maxHp;
                _fillImage.color = ratio > 0.6f ? _highHealthColor
                             : ratio > 0.3f ? _midHealthColor
                             : _lowHealthColor;
            }
        }
    }
}