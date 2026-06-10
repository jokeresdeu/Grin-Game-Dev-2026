using UnityEngine;

namespace Projects.CubeHopper.Scripts
{
    public class CameraShake : MonoBehaviour
    {
        [SerializeField] private float _duration = 0.25f;
        [SerializeField] private float _magnitude = 0.18f;
        [SerializeField] private float _frequency = 28f;

        private Vector3 _basePosition;
        private float _remaining;
        private float _phaseX;
        private float _phaseY;

        private void Awake()
        {
            _basePosition = transform.localPosition;
            _phaseX = Random.value * 1000f;
            _phaseY = Random.value * 1000f;
        }

        private void LateUpdate()
        {
            if (_remaining <= 0f)
            {
                transform.localPosition = _basePosition;
                return;
            }

            _remaining -= Time.unscaledDeltaTime;
            float strength = Mathf.Clamp01(_remaining / _duration) * _magnitude;
            float t = Time.unscaledTime * _frequency;
            float dx = (Mathf.PerlinNoise(_phaseX, t) - 0.5f) * 2f * strength;
            float dy = (Mathf.PerlinNoise(_phaseY, t) - 0.5f) * 2f * strength;
            transform.localPosition = _basePosition + new Vector3(dx, dy, 0f);
        }

        public void Shake()
        {
            _remaining = _duration;
        }

        public void Shake(float duration, float magnitude)
        {
            _duration = duration;
            _magnitude = magnitude;
            _remaining = duration;
        }
    }
}
