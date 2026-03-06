using UnityEngine;

namespace FruitSlice
{
    public class FruitHalf : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float _lifetime = 2f;
        [SerializeField] private float _fadeSpeed = 1f;

        private SpriteRenderer _spriteRenderer;
        private float _timer;
        private bool _isFading;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _timer = _lifetime;
        }

        private void Update()
        {
            _timer -= Time.deltaTime;

            if (_timer <= 0f && !_isFading)
                _isFading = true;

            if (_isFading)
                Fade();

            if (transform.position.y < -10f)
                Destroy(gameObject);
        }

        private void Fade()
        {
            if (_spriteRenderer == null)
            {
                Destroy(gameObject);
                return;
            }

            Color color = _spriteRenderer.color;
            color.a -= _fadeSpeed * Time.deltaTime;
            _spriteRenderer.color = color;

            if (color.a <= 0f)
                Destroy(gameObject);
        }
    }
}
