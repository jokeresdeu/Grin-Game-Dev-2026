using UnityEngine;

namespace ClassicPlatformer
{
    public class Lever : BaseInteractable
    {
        [SerializeField] private SimpleDoor _door;

        public override void Interact(Player player)
        {
            if (_door != null)
            {
                _door.Open();
                Debug.Log("Lever pulled, door opened!");
            }
            else
            {
                Debug.LogWarning("No door assigned to Lever!");
            }
        }
    }
}