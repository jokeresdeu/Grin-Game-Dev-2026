using UnityEngine;

/// <summary>
/// Trigger-зона за межами екрану. Коли птах входить у тригер —
/// гравець втрачає одне життя, а птах знищується.
///
/// Це — приклад використання Collider2D (isTrigger = true)
/// та OnTriggerEnter2D для ігрової механіки.
/// </summary>
[RequireComponent(typeof(BoxCollider2D))]
public class BirdEscapeZone : MonoBehaviour
{
    private void Awake()
    {
        BoxCollider2D col = GetComponent<BoxCollider2D>();
        col.isTrigger = true;
    }

    /// <summary>
    /// Trigger interaction: коли птах виходить за межі ігрового поля
    /// та потрапляє в цю зону — він «втік» і гравець втрачає життя.
    /// </summary>
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
