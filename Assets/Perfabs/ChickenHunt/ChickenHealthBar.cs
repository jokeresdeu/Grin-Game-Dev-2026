using UnityEngine;

namespace ChickenHunt
{
    public class ChickenHealthBar : MonoBehaviour
    {
        [Header("White overlay that shrinks")]
        [SerializeField] private Transform _fillTransform;

        private Vector3 _startScale;
        private Vector3 _startPosition;

        private void Awake()
        {
            if (_fillTransform == null)
            {
                Transform fill = transform.Find("Fill");

                if (fill != null)
                    _fillTransform = fill;
            }

            if (_fillTransform != null)
            {
                _startScale = _fillTransform.localScale;
                _startPosition = _fillTransform.localPosition;
            }
        }

        public void SetValue(int currentHp, int maxHp)
        {
            if (_fillTransform == null)
            {
                Debug.LogWarning("Fill Transform is not assigned in ChickenHealthBar!", this);
                return;
            }

            if (maxHp <= 0)
                return;

            float hpPercent = Mathf.Clamp01((float)currentHp / maxHp);

            // Уменьшаем белую полоску
            Vector3 newScale = _startScale;
            newScale.x = _startScale.x * hpPercent;
            _fillTransform.localScale = newScale;

            // Смещаем её влево, чтобы она уменьшалась СПРАВА НАЛЕВО
            Vector3 newPosition = _startPosition;
            newPosition.x = _startPosition.x - (_startScale.x - newScale.x) * 0.5f;
            _fillTransform.localPosition = newPosition;
        }
    }
}