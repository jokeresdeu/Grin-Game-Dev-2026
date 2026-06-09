using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Projects.MegaSuperChallengeShot.Scripts
{
    public class PlayerHealth : MonoBehaviour
    {
        public static PlayerHealth Instance { get; private set; }

        [SerializeField] private int _maxHp = 5;
        [SerializeField] private Slider _hpSlider;
        [SerializeField] private TMP_Text _hpText;
        [SerializeField] private PlayerCoopAnimator _coopAnimator;

        private int _currentHp;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        private void Start()
        {
            _currentHp = _maxHp;
            UpdateUI();
        }

        public void TakeDamage(int amount)
        {
            if (GameManager.Instance != null && GameManager.Instance.State != GameState.Playing)
                return;

            _currentHp = Mathf.Max(0, _currentHp - amount);
            UpdateUI();

            if (_currentHp <= 0)
            {
                if (_coopAnimator != null)
                    _coopAnimator.PlayDeath();

                if (GameManager.Instance != null)
                    GameManager.Instance.TriggerGameOver();
            }
            else if (_coopAnimator != null)
            {
                _coopAnimator.PlayDamage();
            }
        }

        private void UpdateUI()
        {
            if (_hpSlider != null)
            {
                _hpSlider.maxValue = _maxHp;
                _hpSlider.value = _currentHp;
            }

            if (_hpText != null)
                _hpText.text = $"HP: {_currentHp}/{_maxHp}";
        }

        private void OnDestroy()
        {
            if (Instance == this)
                Instance = null;
        }
    }
}
