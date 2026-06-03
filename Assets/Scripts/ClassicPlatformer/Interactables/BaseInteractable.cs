using UnityEngine;

namespace ClassicPlatformer
{
    [RequireComponent(typeof(Collider2D))]
    public class BaseInteractable : MonoBehaviour
    {
        protected virtual void OnTriggerEnter2D(Collider2D other)
        {
            var player = other.GetComponent<Player>();
            if (player != null)
            {
                Interact(player);
            }
        }

        public virtual void Interact(Player player)
        {
        }
    }
}