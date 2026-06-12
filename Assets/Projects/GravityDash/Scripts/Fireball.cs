using UnityEngine;

public class Fireball : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameController gc = FindObjectOfType<GameController>();
            if (gc != null)
                gc.TakeDamage(1);

            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameController gc = FindObjectOfType<GameController>();
            if (gc != null)
                gc.TakeDamage(1);

            Destroy(gameObject);
        }

        // «нищуЇмо при удар≥ об землю
        if (collision.gameObject.CompareTag("Ground"))
            Destroy(gameObject);
    }
}