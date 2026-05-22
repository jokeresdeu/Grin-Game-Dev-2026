using UnityEngine;

namespace ClassicPlatformer
{
    public class DeathZone : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out Player player))
            {
                player.TakeDamage(player.MaxHealth);
            }
        }
    }
}