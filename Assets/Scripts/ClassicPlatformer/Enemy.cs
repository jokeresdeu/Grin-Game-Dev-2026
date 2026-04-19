using UnityEngine;

namespace ClassicPlatformer
{
    public class Enemy : MonoBehaviour
    {
        [Header("Patrol")]
        [SerializeField] private float _patrolSpeed = 2f;
        [SerializeField] private Transform _leftPoint;
        [SerializeField] private Transform _rightPoint;

        [Header("Visual")]
        [SerializeField] private SpriteRenderer _spriteRenderer;

        [Header("Damage")]
        [SerializeField] private int _damage = 1;

        private int _direction = 1;

        private void Update()
        {
            Patrol();
            CheckPatrolBounds();
        }

        private void Patrol()
        {
            transform.Translate(Vector2.right * _direction * _patrolSpeed * Time.deltaTime);

            if (_spriteRenderer != null)
                _spriteRenderer.flipX = _direction < 0;
        }
        
        private void CheckPatrolBounds()
        {
            if (_leftPoint != null && transform.position.x <= _leftPoint.position.x)
                _direction = 1;
            else if (_rightPoint != null && transform.position.x >= _rightPoint.position.x)
                _direction = -1;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            // Підтримка старого Health компонента
            var health = collision.gameObject.GetComponent<Health>();
            if (health != null)
            {
                if (collision.contacts[0].normal.y < -0.5f)
                    Destroy(gameObject);
                else
                    health.TakeDamage(_damage);
                return;
            }

            // Підтримка нового PlayerHealth компонента
            var playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                if (collision.contacts[0].normal.y < -0.5f)
                    Destroy(gameObject);
                else
                    playerHealth.TakeDamage(_damage);
            }
        }

        // Викликається через Physics2D.OverlapCircle атаки гравця
        public void TakeDamage(int damage = 1)
        {
            Destroy(gameObject);
        }

        private void OnDrawGizmosSelected()
        {
            if (_leftPoint != null && _rightPoint != null)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(_leftPoint.position, _rightPoint.position);
            }
        }
    }
}
