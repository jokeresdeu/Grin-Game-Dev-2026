using System.Collections;
using UnityEngine;

namespace ClassicPlatformer
{
    [RequireComponent(typeof(Animator))]
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

        [Header("Death")]
        [SerializeField] private float _destroyDelay = 0.5f;

        private int _direction = 1;
        private bool _isDead;
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            if (_isDead) return;
            Patrol();
            CheckPatrolBounds();
        }

        private void Patrol()
        {
            transform.Translate(Vector2.right * _direction * _patrolSpeed * Time.deltaTime);

            bool isWalking = _patrolSpeed > 0;
            _animator.SetBool("isWalking", isWalking);

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
            if (_isDead) return;

            // Підтримка старого Health компонента
            var health = collision.gameObject.GetComponent<Health>();
            if (health != null)
            {
                if (collision.contacts[0].normal.y < -0.5f)
                    Die();
                else
                    health.TakeDamage(_damage);
                return;
            }

            // Підтримка нового PlayerHealth компонента
            var playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                if (collision.contacts[0].normal.y < -0.5f)
                    Die();
                else
                    playerHealth.TakeDamage(_damage);
            }
        }

        // Викликається через Physics2D.OverlapCircle атаки гравця
        public void TakeDamage(int damage = 1)
        {
            if (_isDead) return;
            Die();
        }

        private void Die()
        {
            _isDead = true;
            _animator.SetTrigger("Death");
            _animator.SetBool("isWalking", false);
            StartCoroutine(DestroyAfterDelay());
        }

        private IEnumerator DestroyAfterDelay()
        {
            yield return new WaitForSeconds(_destroyDelay);
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
