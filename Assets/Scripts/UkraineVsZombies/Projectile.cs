using System.Collections;
using UnityEngine;

namespace UkraineVsZombies
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float _speed = 10f;
        [SerializeField] private float _lifetime = 3f;
        [SerializeField] private float _flyPulseAmount = 0.12f;
        [SerializeField] private float _flyPulseSpeed = 14f;
        [SerializeField] private float _hitAnimationTime = 0.12f;

        private Enemy _target;
        private float _damage;
        private float _timer;
        private Vector3 _startScale;
        private bool _isHit;

        private void Awake()
        {
            _startScale = transform.localScale;
        }

        public void Initialize(Enemy target, float damage)
        {
            _target = target;
            _damage = damage;
            _timer = _lifetime;
        }

        private void Update()
        {
            if (_isHit) return;

            _timer -= Time.deltaTime;
            if (_timer <= 0f)
            {
                Destroy(gameObject);
                return;
            }

            if (_target == null || !_target.IsAlive)
            {
                Destroy(gameObject);
                return;
            }

            Vector3 direction = (_target.transform.position - transform.position).normalized;
            transform.position += direction * _speed * Time.deltaTime;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);

            float pulse = 1f + Mathf.Sin(Time.time * _flyPulseSpeed) * _flyPulseAmount;
            transform.localScale = _startScale * pulse;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            var enemy = other.GetComponent<Enemy>();
            if (enemy != null && enemy == _target && !_isHit)
            {
                enemy.TakeDamage(_damage);
                StartCoroutine(PlayHitAnimation());
            }
        }

        private IEnumerator PlayHitAnimation()
        {
            _isHit = true;

            Collider2D projectileCollider = GetComponent<Collider2D>();
            if (projectileCollider != null)
                projectileCollider.enabled = false;

            float timer = 0f;
            Vector3 fromScale = transform.localScale;
            Vector3 toScale = fromScale * 1.8f;

            while (timer < _hitAnimationTime)
            {
                timer += Time.deltaTime;
                float t = timer / _hitAnimationTime;
                transform.localScale = Vector3.Lerp(fromScale, toScale, t);
                yield return null;
            }

            Destroy(gameObject);
        }
    }
}
