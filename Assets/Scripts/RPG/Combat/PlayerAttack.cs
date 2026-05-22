using UnityEngine;
using RPG;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float _attackRange = 1.5f;
    [SerializeField] private int _damage = 1;
    [SerializeField] private LayerMask _enemyLayer;

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Attack();
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
            var damageable = hit.GetComponent<IDamageable>();

            if (damageable != null)
            {
                damageable.TakeDamage(_damage);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackRange);
    }
}