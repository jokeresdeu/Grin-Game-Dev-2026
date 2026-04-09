using UnityEngine;

namespace ClassicPlatformer
{
    public abstract class BaseInteractable : MonoBehaviour
    {
        protected bool _playerInside;
        protected Player _currentPlayer;

        protected virtual void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out Player player))
            {
                _playerInside = true;
                _currentPlayer = player;
            }
        }

        protected virtual void OnTriggerExit2D(Collider2D other)
        {
            if (other.TryGetComponent(out Player player) && player == _currentPlayer)
            {
                _playerInside = false;
                _currentPlayer = null;
            }
        }

        private void Update()
        {
            if (_playerInside && _currentPlayer != null && Input.GetKeyDown(KeyCode.E))
            {
                Interact(_currentPlayer);
            }
        }

        public abstract void Interact(Player player);
    }
}