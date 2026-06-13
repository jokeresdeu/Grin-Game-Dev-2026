using UnityEngine;

namespace RPG
{
    [RequireComponent(typeof(Collider2D))]
    public class Hazard : MonoBehaviour
    {
        [SerializeField] private float _damagePerSecond = 20f;

        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                var player = other.GetComponent<Player>();
                if (player != null)
                {
                    player.TakeDamage(_damagePerSecond * Time.deltaTime);
                }
            }
        }
    }
}

