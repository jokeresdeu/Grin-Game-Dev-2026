using Platformer;
using UnityEngine;
using Assets.FantasyMonsters.Common.Scripts;

public class EnemyAttack : MonoBehaviour
{
    [Header("Attack")]
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private int damage = 1;
    [SerializeField] private float attackCooldown = 1.5f;
    [SerializeField] private LayerMask playerLayer;

    [Header("Attack Point")]
    [SerializeField] private Transform attackPoint;

    private Monster _monster;
    private float _lastAttackTime;
    private bool _isDead;

    private void Start()
    {
        _monster = GetComponent<Monster>();

        if (attackPoint == null)
            attackPoint = transform;
    }

    private void Update()
    {
        if (_isDead) return;

        Collider2D playerCol = Physics2D.OverlapCircle(
            attackPoint.position,
            attackRange,
            playerLayer
        );

        if (playerCol != null)
        {
            Debug.Log("Enemy sees player: " + playerCol.name);
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

        IDamageable damageable = playerCol.GetComponentInParent<IDamageable>();

        if (damageable != null)
        {
            Debug.Log("Enemy attacks player!");
            damageable.TakeDamage(damage);
        }
        else
        {
            Debug.LogWarning("Enemy found player collider, but no IDamageable on parent!");
        }
    }

    public void DisableAttack()
    {
        _isDead = true;
        enabled = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Vector3 pos = attackPoint != null ? attackPoint.position : transform.position;
        Gizmos.DrawWireSphere(pos, attackRange);
    }
}