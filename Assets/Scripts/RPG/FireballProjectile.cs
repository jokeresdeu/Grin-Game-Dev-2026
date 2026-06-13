using UnityEngine;

namespace RPG
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class FireballProjectile : MonoBehaviour
    {
        public float damage = 50f;
        public float lifeTime = 3f;

        private void Start()
        {
            Destroy(gameObject, lifeTime);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log("FIREBALL COLLIDED WITH: " + other.gameObject.name + " Tag: " + other.tag);
            if (other.CompareTag("Player")) return;

            var enemy = other.GetComponent<RPGEnemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                Destroy(gameObject);
                return;
            }

            var chest = other.GetComponent<RPGChest>();
            if (chest != null)
            {
                chest.TakeDamage(damage);
                Destroy(gameObject);
                return;
            }
        }
    }
}

