using UnityEngine;

namespace Projects.OrbitGunner.Scripts
{

    public class CameraShake : MonoBehaviour
    {
        public static CameraShake Instance { get; private set; }

        [SerializeField] private float _traumaFalloff = 2.2f;
        [SerializeField] private float _maxOffset = 0.6f;
        [SerializeField] private float _maxAngle = 4f;
        [SerializeField] private float _frequency = 24f;

        private Vector3 _basePosition;
        private float _trauma;
        private float _seed;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                return;
            }

            Instance = this;
            _basePosition = transform.localPosition;
            _seed = 12.34f;
        }

        private void OnDestroy()
        {
            if (Instance == this)
                Instance = null;
        }

        public void Shake(float amount)
        {
            _trauma = Mathf.Clamp01(_trauma + amount);
        }

        private void LateUpdate()
        {
            if (_trauma <= 0f)
            {
                transform.localPosition = _basePosition;
                return;
            }

            float shake = _trauma * _trauma;
            float t = Time.unscaledTime * _frequency;

            float offsetX = (Mathf.PerlinNoise(_seed, t) - 0.5f) * 2f;
            float offsetY = (Mathf.PerlinNoise(_seed + 1f, t) - 0.5f) * 2f;
            float angle = (Mathf.PerlinNoise(_seed + 2f, t) - 0.5f) * 2f;

            transform.localPosition = _basePosition + new Vector3(offsetX, offsetY, 0f) * (_maxOffset * shake);
            transform.localRotation = Quaternion.Euler(0f, 0f, angle * _maxAngle * shake);

            _trauma = Mathf.Max(0f, _trauma - _traumaFalloff * Time.unscaledDeltaTime);

            if (_trauma <= 0f)
            {
                transform.localPosition = _basePosition;
                transform.localRotation = Quaternion.identity;
            }
        }
    }
}
