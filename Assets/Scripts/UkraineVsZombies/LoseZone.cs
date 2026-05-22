using UnityEngine;

public class LoseZone : MonoBehaviour
{
    public int damageToBase = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        EnemyHealth enemy = other.GetComponent<EnemyHealth>();

        if (enemy != null)
        {
            if (GameUI.Instance != null)
            {
                GameUI.Instance.LoseHP(damageToBase);
            }

            Destroy(other.gameObject);
        }
    }
}