using UnityEngine;

namespace ClassicPlatformer
{
    public class Lever : BaseInteractable
    {
        [SerializeField] private Doors _doors;

        private bool _playerInRange;
        private bool _activated;

        private void Update()
        {
            if (_activated || !_playerInRange) return;
            if (Input.GetKeyDown(KeyCode.E))
                _doors.Open();
        }

        protected override void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out Player _))
                _playerInRange = true;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.TryGetComponent(out Player _))
                _playerInRange = false;
        }

        public override void Interact(Player player) { }
    }
}
