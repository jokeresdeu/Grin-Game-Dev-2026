using UnityEngine;

namespace Projects.MegaSuperChallengeShot.Scripts
{
    [RequireComponent(typeof(Collider2D))]
    public class BirdEscapeTrigger : MonoBehaviour
    {
        [SerializeField] private int _damagePerBird = 1;
        [SerializeField] private LayerMask _birdLayer;

        private void Reset()
        {
            Collider2D col = GetComponent<Collider2D>();
            col.isTrigger = true;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (((1 << other.gameObject.layer) & _birdLayer.value) == 0)
                return;

            if (PlayerHealth.Instance != null)
                PlayerHealth.Instance.TakeDamage(_damagePerBird);

            Destroy(other.gameObject);
        }
    }
}
