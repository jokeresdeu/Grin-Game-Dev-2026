using System.Collections;
using UnityEngine;

namespace ChickenHunt
{
    public class HealthKit : MonoBehaviour, IShootable
    {
        [SerializeField] private float _speed = 4f;
        [SerializeField] private Vector2 _direction = Vector2.left;
        [SerializeField] private float _amplitude = 0.5f;
        [SerializeField] private float _frequency = 2f;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Animator _animator;

        private float _startY;
        private bool _isDead = false;

        public void Initialize(Vector2 direction)
        {
            _direction = direction;
        }

        void Start()
        {
            _startY = transform.position.y;

            if (_spriteRenderer == null)
            {
                _spriteRenderer = GetComponent<SpriteRenderer>();
            }

            if (_animator == null)
            {
                _animator = GetComponent<Animator>();
            }
        }

        void Update()
        {
            if (_isDead) return;

            Vector3 horizontalMove = (Vector3)_direction * _speed * Time.deltaTime;
            transform.position += horizontalMove;

            float newY = _startY + Mathf.Sin(Time.time * _frequency) * _amplitude;
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);

            if (_spriteRenderer != null)
            {
                _spriteRenderer.flipX = (_direction.x > 0);
            }
        }

        public void OnShoot()
        {
            if (_isDead) return;
            _isDead = true;

            if (_animator != null)
            {
                _animator.enabled = false;
            }

            if (ChickensManager.Instance != null)
            {
                ChickensManager.Instance.AddLife();
            }

            StartCoroutine(DieRoutine());
        }

        private IEnumerator DieRoutine()
        {
            float duration = 0.35f;
            float elapsed = 0f;
            Vector3 startScale = transform.localScale;
            Vector3 targetScale = startScale * 2.5f;
            Color startColor = _spriteRenderer != null ? _spriteRenderer.color : Color.white;
            Color flashColor = new Color(0f, 1f, 0.2f, 0f);

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / duration;

                transform.localScale = Vector3.Lerp(startScale, targetScale, t);

                if (_spriteRenderer != null)
                {
                    _spriteRenderer.color = Color.Lerp(startColor, flashColor, t);
                }

                yield return null;
            }

            Destroy(gameObject);
        }
    }
}