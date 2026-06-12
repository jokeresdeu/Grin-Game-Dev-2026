using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        CheckDeath(collision.gameObject);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        CheckDeath(collider.gameObject);
    }

    private void CheckDeath(GameObject target)
    {
        if (target.CompareTag("Enemy"))
        {
            gameObject.SetActive(false);

            if (GameManager.Instance != null)
            {
                GameManager.Instance.GameOver();
            }
        }
    }
}