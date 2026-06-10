using UnityEngine;

namespace Projects.CubeHopper.Scripts
{
    public class WorldSpeed : MonoBehaviour
    {
        public static WorldSpeed Instance { get; private set; }

        [SerializeField] private float _baseSpeed = 6f;
        [SerializeField] private float _maxSpeed = 18f;
        [SerializeField] private float _accelerationPerSecond = 0.15f;
        [SerializeField] private float _hitSlowdownFactor = 0.5f;
        [SerializeField] private float _hitSlowdownDuration = 0.6f;

        public float Current { get; private set; }
        public float Base => _baseSpeed;
        public float Max => _maxSpeed;
        public float NormalizedSpeed => Mathf.InverseLerp(_baseSpeed, _maxSpeed, Current);

        private float _accumulatedSpeed;
        private float _slowdownRemaining;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            ResetSpeed();
        }

        private void Update()
        {
            if (GameManager.Instance != null && GameManager.Instance.State != GameState.Playing)
                return;

            _accumulatedSpeed = Mathf.Min(_accumulatedSpeed + _accelerationPerSecond * Time.deltaTime, _maxSpeed - _baseSpeed);

            float target = _baseSpeed + _accumulatedSpeed;

            if (_slowdownRemaining > 0f)
            {
                _slowdownRemaining -= Time.deltaTime;
                target *= _hitSlowdownFactor;
            }

            Current = target;
        }

        public void ResetSpeed()
        {
            _accumulatedSpeed = 0f;
            _slowdownRemaining = 0f;
            Current = _baseSpeed;
        }

        public void ApplyHitSlowdown()
        {
            _slowdownRemaining = _hitSlowdownDuration;
        }

        private void OnDestroy()
        {
            if (Instance == this)
                Instance = null;
        }
    }
}
