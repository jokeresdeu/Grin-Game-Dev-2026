using UnityEngine;

namespace ClassicPlatformer
{
    [RequireComponent(typeof(Animator), typeof(Collider2D))]
    public class Enemy : MonoBehaviour
    {
        [Header("Patrol")]
        [SerializeField] private float _patrolSpeed = 2f;
        [SerializeField] private Transform _leftPoint;
        [SerializeField] private Transform _rightPoint;

        [Header("Damage & Health")]
        [SerializeField] private int _damage = 1;
        [SerializeField] private int _health = 2;
        [SerializeField] private float _deathAnimationTime = 0.5f;

        private int _direction = 1;
        private Animator _animator;
        private Collider2D _collider;
        private bool _isDead = false;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _collider = GetComponent<Collider2D>();
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

            float currentScaleY = transform.localScale.y;
            float currentScaleZ = transform.localScale.z;
            float absoluteScaleX = Mathf.Abs(transform.localScale.x);

            transform.localScale = new Vector3(-_direction * absoluteScaleX, currentScaleY, currentScaleZ);
        }

        private void CheckPatrolBounds()
        {
            if (_leftPoint != null && transform.position.x <= _leftPoint.position.x)
                _direction = 1;
            else if (_rightPoint != null && transform.position.x >= _rightPoint.position.x)
                _direction = -1;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_isDead) return;

            var player = other.GetComponentInParent<Player>();

            if (player != null)
            {
                player.PlayAttackAnimation();

                player.TakeDamage(_damage);
                Debug.Log("Зіткнення! Гравець б'є і отримує шкоду одночасно.");

                TakeDamage();
            }
        }

        private void TakeDamage()
        {
            _health--;
            Debug.Log("Ворога вдарили! У нього залишилось HP: " + _health);

            if (_health <= 0)
            {
                Die();
            }
            else
            {
                if (_animator != null)
                {
                    _animator.SetTrigger("Hit");
                }
            }
        }

        private void Die()
        {
            _isDead = true;
            _collider.enabled = false;

            if (_animator != null)
            {
                _animator.SetTrigger("Die");
            }

            Destroy(gameObject, _deathAnimationTime);
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