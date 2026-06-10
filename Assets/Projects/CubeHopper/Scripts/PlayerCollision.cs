using UnityEngine;

namespace Projects.CubeHopper.Scripts
{
    public class PlayerCollision : MonoBehaviour
    {
        [SerializeField] private PlayerHealth _health;
        [SerializeField] private string _obstacleTag = "Obstacle";
        [SerializeField] private CameraShake _cameraShake;
        [SerializeField] private float _knockbackImpulse = 6f;
        [SerializeField] private Rigidbody2D _rigidbody;

        private void Awake()
        {
            if (_health == null)
                _health = GetComponent<PlayerHealth>();
            if (_rigidbody == null)
                _rigidbody = GetComponent<Rigidbody2D>();

            if (_health != null)
                _health.Died += OnDied;
        }

        private void OnDestroy()
        {
            if (_health != null)
                _health.Died -= OnDied;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            HandleObstacleHit(other);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            HandleObstacleHit(collision.collider);
        }

        private void HandleObstacleHit(Collider2D other)
        {
            if (_health == null || !other.CompareTag(_obstacleTag))
                return;

            if (_health.IsInvincible || _health.Lives <= 0)
                return;

            if (!_health.TryDamage())
                return;

            ApplyKnockback();

            if (_cameraShake != null)
                _cameraShake.Shake();

            if (ScoreManager.Instance != null)
                ScoreManager.Instance.ResetCombo();

            if (WorldSpeed.Instance != null)
                WorldSpeed.Instance.ApplyHitSlowdown();
        }

        private void ApplyKnockback()
        {
            if (_rigidbody == null)
                return;

            Vector2 velocity = _rigidbody.linearVelocity;
            velocity.y = _knockbackImpulse;
            _rigidbody.linearVelocity = velocity;
        }

        private void OnDied()
        {
            if (ScoreManager.Instance != null)
                ScoreManager.Instance.CommitBestScore();

            if (GameManager.Instance != null)
                GameManager.Instance.TriggerGameOver();
        }
    }
}
