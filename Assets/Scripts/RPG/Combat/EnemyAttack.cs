using UnityEngine;
using RPG;
using Assets.FantasyMonsters.Common.Scripts;

public class EnemyAttack : MonoBehaviour
{
    [Header("Attack")]
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private int damage = 1;
    [SerializeField] private float attackCooldown = 1.5f;
    [SerializeField] private LayerMask playerLayer;

    private Monster _monster;
    private float _lastAttackTime;

    private void Start()
    {
        _monster = GetComponent<Monster>();
    }

    private void Update()
    {
        // шукаЇмо гравц€ в рад≥ус≥
        Collider2D playerCol = Physics2D.OverlapCircle(transform.position, attackRange, playerLayer);

        if (playerCol != null)
        {
            TryAttack(playerCol);
        }
    }

    private void TryAttack(Collider2D playerCol)
    {
        if (Time.time < _lastAttackTime + attackCooldown)
            return;

        _lastAttackTime = Time.time;

        if (_monster != null)
            _monster.Attack();

        var damageable = playerCol.GetComponent<IDamageable>();

        if (damageable != null)
            damageable.TakeDamage(damage);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}