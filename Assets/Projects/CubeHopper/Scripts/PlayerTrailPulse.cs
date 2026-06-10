using UnityEngine;

namespace Projects.CubeHopper.Scripts
{
    public class PlayerTrailPulse : MonoBehaviour
    {
        [SerializeField] private TrailRenderer _trail;
        [SerializeField] private Gradient _baseGradient;
        [SerializeField] private Gradient _comboGradient;
        [SerializeField] private float _baseTime = 0.25f;
        [SerializeField] private float _comboTime = 0.5f;

        private void Awake()
        {
            if (_trail == null)
                _trail = GetComponentInChildren<TrailRenderer>();
        }

        private void OnEnable()
        {
            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.ComboChanged += OnComboChanged;
                OnComboChanged(ScoreManager.Instance.ComboMultiplier);
            }
        }

        private void OnDisable()
        {
            if (ScoreManager.Instance != null)
                ScoreManager.Instance.ComboChanged -= OnComboChanged;
        }

        private void OnComboChanged(int combo)
        {
            if (_trail == null)
                return;

            if (combo > 1 && _comboGradient != null)
            {
                _trail.colorGradient = _comboGradient;
                _trail.time = _comboTime;
            }
            else if (_baseGradient != null)
            {
                _trail.colorGradient = _baseGradient;
                _trail.time = _baseTime;
            }
        }
    }
}
