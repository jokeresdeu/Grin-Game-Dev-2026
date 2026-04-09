using UnityEngine;

namespace ClassicPlatformer
{
    public class Lever : BaseInteractable
    {
        [SerializeField] private Doors _doors;
        [SerializeField] private bool _singleUse = true;

        private bool _used;

        public override void Interact(Player player)
        {
            if (_doors == null)
            {
                Debug.LogWarning("Lever has no door assigned.");
                return;
            }

            if (_singleUse && _used)
                return;

            _doors.Open();
            _used = true;

            Debug.Log("Lever activated");
        }
    }
}