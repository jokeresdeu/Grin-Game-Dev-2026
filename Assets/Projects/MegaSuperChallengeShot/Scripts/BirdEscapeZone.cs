using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class BirdEscapeZone : MonoBehaviour
{
    private void Awake()
    {
        BoxCollider2D col = GetComponent<BoxCollider2D>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Bird bird = other.GetComponent<Bird>();
        if (bird == null) return;

        // Птах втік — гравець втрачає життя
        if (GameManager.Instance != null)
        {
            GameManager.Instance.LoseLife();
        }

        Destroy(other.gameObject);
    }
}
