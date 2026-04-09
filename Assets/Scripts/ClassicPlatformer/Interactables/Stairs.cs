using UnityEngine;

namespace ClassicPlatformer
{
    public class Stairs : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out Player player))
            {
                player.EnableVerticalMovement(true);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.TryGetComponent(out Player player))
            {
                player.EnableVerticalMovement(false);
            }
        }
    }
}