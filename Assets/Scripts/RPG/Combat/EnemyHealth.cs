using UnityEngine;
using Assets.FantasyMonsters.Common.Scripts;
using RPG;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    [Header("Health")]
    [SerializeField] private int _maxHealth = 3;
    [SerializeField] private Transform _healthFill;

    private int _currentHealth;
    private bool _isDead;

    private Monster _monster;
    private Collider2D _collider;

    private void Awake()
    {
        _currentHealth = _maxHealth;
        _monster = GetComponent<Monster>();
        _collider = GetComponent<Collider2D>();

        UpdateBar();
    }

    public void TakeDamage(int damage)
    {
        if (_isDead) return;

        _currentHealth -= damage;

        if (_monster != null)
            _monster.Spring();

        UpdateBar();

        if (_currentHealth <= 0)
            Die();
    }

    private void UpdateBar()
    {
        if (_healthFill == null) return;

        float percent = Mathf.Clamp01((float)_currentHealth / _maxHealth);

        _healthFill.localScale = new Vector3(percent * 12.5f, 3f, 0f);
    }

    private void Die()
    {
        if (_isDead) return;

        _isDead = true;

        if (_collider != null)
            _collider.enabled = false;

        GameManager.Instance?.AddKill();

        if (_monster != null)
            _monster.Die();
        GetComponent<EnemyAttack>().enabled = false;
        Destroy(gameObject, 1.5f);
    }
}