using UnityEngine;

namespace ChickenHunt
{
    public class Chest : MonoBehaviour, IShootable
    {
        [Header("Explosion Settings")]
        [SerializeField] private float _explosionRadius = 3f;

        public void OnShoot()
        {
            Explode();
            Destroy(gameObject);
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