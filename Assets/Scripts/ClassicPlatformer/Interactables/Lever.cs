using UnityEngine;
namespace ClassicPlatformer
{
    public class Lever : BaseInteractable
    {
        [SerializeField] private Doors _doors;

        public bool IsActivated { get; private set; }

        public override void Interact(Player player)
        {
            _doors.Open();
            IsActivated = true;
            Debug.Log("Lever activated");
        }
    }
}