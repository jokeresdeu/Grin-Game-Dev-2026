using UnityEngine;

namespace ChickenHunt
{
    public class EscapeZone : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out Chicken escapedChicken))
            {
                ChickensManager.Instance.TakeDamage(1);
                Destroy(escapedChicken.gameObject);
            }
        }

        private void OnDrawGizmos()
        {
            BoxCollider2D box = GetComponent<BoxCollider2D>();
            if (box != null)
            {
                Gizmos.color = new Color(1f, 0f, 0f, 0.3f);
                Gizmos.DrawCube(transform.position + (Vector3)box.offset, box.size);
            }
        }
    }
}