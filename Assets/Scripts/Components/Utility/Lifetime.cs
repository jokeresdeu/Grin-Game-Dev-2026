using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Automatically destroys object after a specified time.
/// Use for projectiles, effects, temporary objects.
/// </summary>
public class Lifetime : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float lifetime = 5f;
    [SerializeField] private bool useUnscaledTime = false;
    
    [Header("Events")]
    [SerializeField] private UnityEvent onLifetimeEnd;
    
    private float timer;
    
    public float RemainingTime => lifetime - timer;
    public float LifetimePercent => timer / lifetime;
    
    private void Update()
    {
        timer += useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
        
        if (timer >= lifetime)
        {
            onLifetimeEnd?.Invoke();
            Destroy(gameObject);
        }
    }
    
    public void SetLifetime(float newLifetime)
    {
        lifetime = newLifetime;
    }
    
    public void ResetTimer()
    {
        timer = 0f;
    }
}
