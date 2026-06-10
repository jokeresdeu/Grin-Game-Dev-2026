using UnityEngine;

namespace ChickenHunt
{
    public class GroundSway : MonoBehaviour
    {
        [Header("Vertical Sway")]
        [SerializeField] private float _verticalAmplitude = 0.03f;
        [SerializeField] private float _verticalSpeed = 0.8f;

        [Header("Rotation Sway")]
        [SerializeField] private float _rotationAmplitude = 0.4f;
        [SerializeField] private float _rotationSpeed = 0.6f;

        private Vector3 _startPosition;
        private Quaternion _startRotation;

        private void Awake()
        {
            _startPosition = transform.position;
            _startRotation = transform.rotation;
        }

        private void Update()
        {
            if (GameManager.Instance != null && !GameManager.Instance.IsPlaying)
                return;

            float yOffset = Mathf.Sin(Time.time * _verticalSpeed) * _verticalAmplitude;
            float zRotation = Mathf.Sin(Time.time * _rotationSpeed) * _rotationAmplitude;

            transform.position = _startPosition + new Vector3(0f, yOffset, 0f);
            transform.rotation = _startRotation * Quaternion.Euler(0f, 0f, zRotation);
        }
    }
}