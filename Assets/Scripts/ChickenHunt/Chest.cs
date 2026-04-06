using UnityEngine;

namespace ChickenHunt
{
    public class Chest : MonoBehaviour, IShootable
    {
        [Header("Explosion Settings")]
        [SerializeField] private float _explosionRadius = 3f;

        private Animator _animator;
        private bool _isDead = false;

        private void Start()
        {
            _animator = GetComponent<Animator>();
        }

        public void OnShoot()
        {
            if (_isDead) return;
            _isDead = true;

            Explode();

            if (_animator != null)
            {
                _animator.SetTrigger("DoExplode");
            }

            Destroy(gameObject, 0.3f);
        }

        private void Explode()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _explosionRadius);

            foreach (Collider2D coll in colliders)
            {
                if (coll.TryGetComponent(out Chicken chicken))
                {
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