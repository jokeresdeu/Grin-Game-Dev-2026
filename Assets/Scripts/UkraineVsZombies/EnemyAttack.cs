using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public int damage = 10;
    public float attackCooldown = 1f;

    private float timer = 0f;

    private void Update()
    {
        timer += Time.deltaTime;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        DefenderHealth defender = other.GetComponent<DefenderHealth>();

        if (defender != null && timer >= attackCooldown)
        {
            defender.TakeDamage(damage);
            timer = 0f;
        }
    }
}
