using UnityEngine;
using RPG;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float _attackRange = 1.5f;
    [SerializeField] private int _damage = 1;
    [SerializeField] private LayerMask _enemyLayer;
    [SerializeField] private float _cooldown = 1f;

    private float _nextAttackTime;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.time >= _nextAttackTime)
        {
            Attack();
            _nextAttackTime = Time.time + _cooldown;
        }
    }

    private void Attack()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(
            transform.position,
            _attackRange,
            _enemyLayer
        );

        foreach (var hit in hits)
        {
            if (hit.gameObject == gameObject)
                continue;

            hit.SendMessage("TakeDamage", _damage, SendMessageOptions.DontRequireReceiver);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackRange);
    }
}