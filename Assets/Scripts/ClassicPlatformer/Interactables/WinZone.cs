using UnityEngine;
namespace ClassicPlatformer
{
    public class WinZone : MonoBehaviour
    {
        [SerializeField] private Lever _lever;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out Player _))
            {
                if (_lever != null && !_lever.IsActivated)
                    return;

                GameManager.Instance?.SetWin();
            }
        }
    }
}