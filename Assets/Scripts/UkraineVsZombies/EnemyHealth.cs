using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int hp = 30;
    public int scoreForKill = 10;

    public void TakeDamage(int damage)
    {
        hp -= damage;

        if (hp <= 0)
        {
            if (GameUI.Instance != null)
            {
                GameUI.Instance.AddScore(scoreForKill);
            }

            Destroy(gameObject);
        }
    }
}