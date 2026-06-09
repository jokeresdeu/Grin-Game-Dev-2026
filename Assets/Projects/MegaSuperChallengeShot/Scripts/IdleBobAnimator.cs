using UnityEngine;

namespace Projects.MegaSuperChallengeShot.Scripts
{
    public class IdleBobAnimator : MonoBehaviour
    {
        [SerializeField] private Vector3 _bobAxis = Vector3.up;
        [SerializeField] private float _amplitude = 0.15f;
        [SerializeField] private float _frequency = 1.5f;
        [SerializeField] private float _rotationAmplitude = 0f;
        [SerializeField] private float _phaseOffset = 0f;

        private Vector3 _basePosition;
        private Quaternion _baseRotation;

        private void Awake()
        {
            _basePosition = transform.localPosition;
            _baseRotation = transform.localRotation;
        }

        private void Update()
        {
            float t = (Time.time + _phaseOffset) * _frequency * Mathf.PI * 2f;
            float sine = Mathf.Sin(t);

            transform.localPosition = _basePosition + _bobAxis.normalized * (sine * _amplitude);

            if (_rotationAmplitude > 0f)
            {
                float angle = sine * _rotationAmplitude;
                transform.localRotation = _baseRotation * Quaternion.Euler(0f, 0f, angle);
            }
        }
    }
}
