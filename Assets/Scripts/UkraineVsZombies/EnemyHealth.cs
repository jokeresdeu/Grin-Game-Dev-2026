using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    public int hp = 50;
    public int scoreForKill = 10;

    private EnemyAnimation enemyAnimation;
    private bool isDead = false;

    private void Awake()
    {
        enemyAnimation = GetComponent<EnemyAnimation>();
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        hp -= damage;

        if (enemyAnimation != null)
            enemyAnimation.PlayHurt();

        if (hp <= 0)
        {
            isDead = true;

            if (GameUI.Instance != null)
                GameUI.Instance.AddScore(scoreForKill);

            StartCoroutine(DeathRoutine());
        }
    }

    private IEnumerator DeathRoutine()
    {
        if (enemyAnimation != null)
            enemyAnimation.PlayDeath();

        yield return new WaitForSeconds(0.4f);
        Destroy(gameObject);
    }
}