using UnityEngine;

namespace ChickenHunt
{
    public class Chest : MonoBehaviour, IShootable
    {
        [Header("Explosion Settings")]
        [SerializeField] private float _explosionRadius = 3f;
        [SerializeField] private float _destroyDelay = 0.8f; // Загальний час життя після кліку
        [SerializeField] private float _damageDelay = 0.2f;  // Затримка перед самим "бумом"

        [Header("Visuals")]
        [SerializeField] private Animator _animator;
        [SerializeField] private SpriteRenderer _spriteRenderer; // Перетягни сюди спрайт скрині
        [SerializeField] private Color _explosionColor = Color.red;

        private bool _isExploded = false;

        public void OnShoot()
        {
            Explode();
        }

        private void OnMouseDown()
        {
            Explode();
        }

        public void Explode()
        {
            if (_isExploded) return;
            _isExploded = true;

            // 1. Скриня червоніє
            if (_spriteRenderer != null)
            {
                _spriteRenderer.color = _explosionColor;
            }

            // 2. Запускаємо анімацію
            if (_animator != null)
            {
                _animator.SetTrigger("Explode");
            }

            // 3. Викликаємо вибух із затримкою, щоб він збігався з картинкою
            Invoke(nameof(DamageNearbyChickens), _damageDelay);

            // 4. Знищуємо саму скриню в кінці анімації
            Destroy(gameObject, _destroyDelay);
        }

        private void DamageNearbyChickens()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _explosionRadius);

            foreach (Collider2D coll in colliders)
            {
                if (coll.TryGetComponent(out Chicken chicken))
                {
                    // Тепер курі зникатимуть саме в цей момент
                    chicken.OnShoot();
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _explosionRadius);
        }
    }
}