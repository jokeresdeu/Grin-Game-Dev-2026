using UnityEngine;

namespace ClassicPlatformer
{
    public class Lever : BaseInteractable
    {
        [SerializeField] private Doors _doors;

        private bool _isPlayerNear = false;
        private Player _player;

        protected override void OnTriggerEnter2D(Collider2D other)
        {
            _player = other.GetComponent<Player>();
            if (_player != null)
            {
                _isPlayerNear = true;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.GetComponent<Player>() != null)
            {
                _isPlayerNear = false;
                _player = null;
            }
        }

        private void Update()
        {
            if (_isPlayerNear && Input.GetKeyDown(KeyCode.E))
            {
                Interact(_player);
            }
        }

        public override void Interact(Player player)
        {
            if (_doors != null)
            {
                _doors.Open();
                Debug.Log("Важіль натиснуто!");
            }
        }
    }
}