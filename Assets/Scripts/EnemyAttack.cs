using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public GameObject player;
    public GameObject enemyAttackZone;
    public GameObject healthBar;

    public float damage = 5f;
    public float attackCooldown = 1f;

    public Player playerInfo;

    private float _maxHealthBarWidth;
    private float _lastAttackTime;

    void Start()
    {
        // Запам'ятовуємо початкову ширину healthBar для розрахунку пропорцій
        _maxHealthBarWidth = healthBar.transform.localScale.x;
    }

    void Update()
    {
        if (player == null || healthBar == null) return;

        var zoneBounds = enemyAttackZone.GetComponent<Collider2D>().bounds;
        var playerPos = player.transform.position;

        bool playerInZone = zoneBounds.Contains(playerPos);
        bool hpLeft = healthBar.transform.localScale.x > 0;
        bool cooldownPassed = Time.time >= _lastAttackTime + attackCooldown;

        if (playerInZone && hpLeft && cooldownPassed)
        {
            // Розраховуємо скільки зняти з шкали здоров'я пропорційно
            float damageRatio = (_maxHealthBarWidth * damage) / playerInfo.health;
            
            healthBar.transform.localScale = new Vector3(
                Mathf.Max(0, healthBar.transform.localScale.x - damageRatio),
                healthBar.transform.localScale.y,
                healthBar.transform.localScale.z
            );

            _lastAttackTime = Time.time;
        }
    }
}
