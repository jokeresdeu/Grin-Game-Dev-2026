using UnityEngine;
using RPG;
using System.Collections;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float _attackRange = 1.5f;
    [SerializeField] private int _damage = 1;
    [SerializeField] private LayerMask _enemyLayer;
    [SerializeField] private float _attackDuration = 0.5f;

    [Header("Animation")]
    [SerializeField] private PlayerAnimationController _animationController;

    private bool _isAttacking;

    private void Awake()
    {
        if (_animationController == null)
            _animationController = GetComponentInChildren<PlayerAnimationController>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1) && !_isAttacking)
        {
            StartCoroutine(AttackRoutine());
        }
    }

    private IEnumerator AttackRoutine()
    {
        _isAttacking = true;

        if (_animationController != null)
            _animationController.PlayAttack();

        Attack();

        yield return new WaitForSeconds(_attackDuration);

        if (_animationController != null)
            _animationController.EndAttack();

        _isAttacking = false;
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