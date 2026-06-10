using UnityEngine;

namespace ChickenHunt
{
    public class CloudWaveMover : MonoBehaviour
    {
        [Header("Horizontal Movement")]
        [SerializeField] private float _moveSpeed = 0.25f;
        [SerializeField] private float _moveDistance = 1.5f;

        [Header("Vertical Movement")]
        [SerializeField] private float _verticalSpeed = 0.6f;
        [SerializeField] private float _verticalDistance = 0.15f;

        private Vector3 _startPosition;

        private void Awake()
        {
            _startPosition = transform.position;
        }

        private void Update()
        {
            if (GameManager.Instance != null && !GameManager.Instance.IsPlaying)
                return;

            float x = Mathf.Sin(Time.time * _moveSpeed) * _moveDistance;
            float y = Mathf.Sin(Time.time * _verticalSpeed) * _verticalDistance;

            transform.position = _startPosition + new Vector3(x, y, 0f);
        }
    }
}