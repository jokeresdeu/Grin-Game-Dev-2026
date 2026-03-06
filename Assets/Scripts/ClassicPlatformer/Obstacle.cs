using UnityEngine;

namespace ClassicPlatformer
{
    public class Obstacle : MonoBehaviour
    {
        [Header("Damage")]
        [SerializeField] private int _damage = 1;
        [SerializeField] private bool _instantKill = false;

        private void OnTriggerEnter2D(Collider2D other)
        {
            var health = other.GetComponent<Health>();
            if (health == null) return;

            health.TakeDamage(_instantKill ? health.MaxHealth : _damage);
        }
    }
}
