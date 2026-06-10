using UnityEngine;

namespace ChickenHunt
{
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class EscapeZone : MonoBehaviour
    {
        [SerializeField] private ChickensManager _manager;

        private void Awake()
        {
            Collider2D zoneCollider = GetComponent<Collider2D>();
            zoneCollider.isTrigger = true;

            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.gravityScale = 0f;

            if (_manager == null)
                _manager = FindObjectOfType<ChickensManager>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Chicken chicken = other.GetComponentInParent<Chicken>();

            if (chicken == null)
                return;

            _manager.RegisterChickenEscape(chicken);
        }
    }
}