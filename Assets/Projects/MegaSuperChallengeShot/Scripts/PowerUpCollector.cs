using UnityEngine;

public class PowerUpCollector : MonoBehaviour
{
    [Header("Collection Area (OverlapBox)")]
    [SerializeField] private Vector2 collectionSize = new Vector2(1f, 1f);
    [SerializeField] private LayerMask powerUpLayer;

    private void Update()
    {
        if (GameManager.Instance == null) return;
        if (GameManager.Instance.State != GameState.Playing) return;

        Collider2D[] hits = Physics2D.OverlapBoxAll(
            transform.position, collectionSize, 0f, powerUpLayer);

        foreach (Collider2D hit in hits)
        {
            PowerUp powerUp = hit.GetComponent<PowerUp>();
            if (powerUp != null)
            {
                powerUp.Collect();
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0f, 1f, 0f, 0.3f);
        Gizmos.DrawWireCube(transform.position, collectionSize);
    }
}
