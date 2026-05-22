using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Manages health points for an object. Can take damage and die.
/// The foundation for any damageable object in the game.
/// </summary>
public class Health : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth;
    
    [Header("Invincibility")]
    [SerializeField] private bool invincibleOnStart = false;
    [SerializeField] private float invincibilityDuration = 0f;
    
    [Header("Events")]
    [SerializeField] private UnityEvent onDamage;
    [SerializeField] private UnityEvent onHeal;
    [SerializeField] private UnityEvent onDeath;
    
    private bool isInvincible;
    private float invincibilityTimer;
    
    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;
    public float HealthPercent => currentHealth / maxHealth;
    public bool IsAlive => currentHealth > 0f;
    public bool IsInvincible => isInvincible;
    
    private void Awake()
    {
        currentHealth = maxHealth;
        isInvincible = invincibleOnStart;
    }
    
    private void Update()
    {
        if (invincibilityTimer > 0f)
        {
            invincibilityTimer -= Time.deltaTime;
            if (invincibilityTimer <= 0f)
            {
                isInvincible = false;
            }
        }
    }
    
    public void TakeDamage(float damage)
    {
        if (!IsAlive || isInvincible) return;
        
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0f);
        
        onDamage?.Invoke();
        
        if (invincibilityDuration > 0f)
        {
            SetInvincible(invincibilityDuration);
        }
        
        if (currentHealth <= 0f)
        {
            Die();
        }
    }
    
    public void Heal(float amount)
    {
        if (!IsAlive) return;
        
        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);
        
        onHeal?.Invoke();
    }
    
    public void SetInvincible(float duration)
    {
        isInvincible = true;
        invincibilityTimer = duration;
    }
    
    public void SetInvincible(bool value)
    {
        isInvincible = value;
        if (!value) invincibilityTimer = 0f;
    }
    
    private void Die()
    {
        onDeath?.Invoke();
    }
    
    public void ResetHealth()
    {
        currentHealth = maxHealth;
    }
}
