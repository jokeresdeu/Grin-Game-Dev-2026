using UnityEngine;

namespace ClassicPlatformer
{
    public class Stairs : BaseInteractable
    {
        private Player _player;

        public override void Interact(Player player)
        {
            _player = player;
            _player.EnableVerticalMovement(true);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.TryGetComponent(out Player player) || player != _player) return;
            _player.EnableVerticalMovement(false);
            _player = null;
        }
    }
}
