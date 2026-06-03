using UnityEngine;

namespace ClassicPlatformer
{
    public class Enemy : MonoBehaviour
    {
        [Header("Patrol")]
        [SerializeField] private float _patrolSpeed = 2f;
        [SerializeField] private Transform _leftPoint;
        [SerializeField] private Transform _rightPoint;

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
            var player = other.GetComponentInParent<Player>();

            if (player != null)
            {
                player.TakeDamage(_damage);
                Debug.Log("Слиз вкусив гравця!");
            }
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