using TMPro;
using UnityEngine;

namespace ClassicPlatformer
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private Player _player;
        [SerializeField] private TextMeshProUGUI _hpText;
        [SerializeField] private TextMeshProUGUI _timerText;

        private float _elapsed;
        private bool _running;

        private void Start()
        {
            if (_player != null)
            {
                _player.OnHealthChanged += UpdateHP;
                _player.OnDeath += StopTimer;
                UpdateHP(_player.CurrentHealth, _player.MaxHealth);
            }
        }

        private void OnDestroy()
        {
            if (_player != null)
            {
                _player.OnHealthChanged -= UpdateHP;
                _player.OnDeath -= StopTimer;
            }
        }

        public void StartTimer() => _running = true;

        private void StopTimer() => _running = false;

        private void Update()
        {
            if (!_running) return;
            _elapsed += Time.deltaTime;
            int m = (int)(_elapsed / 60);
            int s = (int)(_elapsed % 60);
            if (_timerText != null)
                _timerText.text = $"{m:00}:{s:00}";
        }

        private void UpdateHP(int current, int max)
        {
            if (_hpText != null)
                _hpText.text = $"HP: {current}/{max}";
        }
    }
}
