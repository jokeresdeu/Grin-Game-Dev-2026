using UnityEngine;
using RPG;
using Assets.FantasyMonsters.Common.Scripts;

public class AttackEnemy : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int _maxHealth = 10;
    [SerializeField] private int _currentHealth = 10;
    [SerializeField] private Transform _healthFill;
    [SerializeField] private float _deathDelay = 1.2f;

    [Header("Attack")]
    [SerializeField] private float _range = 1.5f;
    [SerializeField] private int _damageAmount = 1;
    [SerializeField] private float _cooldown = 1.5f;
    [SerializeField] private LayerMask _targetMask;

    [Header("Movement")]
    [SerializeField] private float _moveSpeed = 2f;
    [SerializeField] private float _followRange = 10f;

    private Monster _monster;
    private Player _player;
    private float _nextAttackTime;
    private Vector3 _healthFillStartScale;
    private bool _isDead;

    private void Awake()
    {
        _monster = GetComponent<Monster>();
        _player = FindAnyObjectByType<Player>();

        if (_currentHealth > _maxHealth)
            _currentHealth = _maxHealth;

        if (_healthFill != null)
            _healthFillStartScale = _healthFill.localScale;

        UpdateHealthBar();
    }

    private void Update()
    {
        if (_isDead)
            return;

        FollowPlayer();
        TryAttack();
    }

    public void TakeDamage(int amount)
    {
        if (_isDead || amount <= 0)
            return;

        _currentHealth -= amount;

        if (_currentHealth <= 0)
        {
            Die();
            return;
        }

        UpdateHealthBar();
    }

    private void TryAttack()
    {
        if (_player == null)
            return;

        if (Time.time < _nextAttackTime)
            return;

        Collider2D target = Physics2D.OverlapCircle(transform.position, _range, _targetMask);

        if (target == null)
            return;

        Player player = target.GetComponent<Player>();

        if (player == null)
            return;

        _nextAttackTime = Time.time + _cooldown;

        if (_monster != null)
            _monster.Attack();

        player.TakeDamage(_damageAmount);
    }

    private void FollowPlayer()
    {
        if (_player == null)
        {
            if (_monster != null)
                _monster.SetState(MonsterState.Idle);

            return;
        }

        Vector2 direction = _player.transform.position - transform.position;
        float distance = direction.magnitude;

        if (distance <= _range || distance > _followRange)
        {
            if (_monster != null)
                _monster.SetState(MonsterState.Idle);

            return;
        }

        Vector2 moveDirection = direction.normalized;
        transform.position += (Vector3)(moveDirection * _moveSpeed * Time.deltaTime);

        if (_monster != null)
            _monster.SetState(MonsterState.Walk);

        if (moveDirection.x > 0.01f)
            transform.localScale = new Vector3(-1f, 1f, 1f);
        else if (moveDirection.x < -0.01f)
            transform.localScale = new Vector3(1f, 1f, 1f);
    }

    private void Die()
    {
        if (_isDead)
            return;

        _isDead = true;

        _player?.GetComponentInChildren<PlayerAnimationController>()?.PlayVictory();

        FindAnyObjectByType<GameUIManager>()?.ShowEnemyDeath();

        if (_monster != null)
            _monster.Die();

        StartCoroutine(DestroyAfterDeath());
    }

    private System.Collections.IEnumerator DestroyAfterDeath()
    {
        yield return new WaitForSeconds(_deathDelay);
        Destroy(gameObject);
    }

    private void UpdateHealthBar()
    {
        if (_healthFill == null)
            return;

        float percent = _maxHealth > 0 ? (float)_currentHealth / _maxHealth : 0f;
        percent = Mathf.Clamp01(percent);

        Vector3 newScale = _healthFillStartScale;
        newScale.x *= percent;
        _healthFill.localScale = newScale;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _range);
    }
}
