using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Destroys object when it becomes invisible to the camera.
/// Great for cleaning up projectiles that leave the screen.
/// </summary>
public class DestroyOnInvisible : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float delay = 0f;
    [SerializeField] private bool onlyAfterVisible = true;
    
    [Header("Events")]
    [SerializeField] private UnityEvent onBecameInvisible;
    
    private bool wasVisible;
    
    private void OnBecameVisible()
    {
        wasVisible = true;
    }
    
    private void OnBecameInvisible()
    {
        if (onlyAfterVisible && !wasVisible) return;
        
        onBecameInvisible?.Invoke();
        
        if (delay > 0f)
        {
            Destroy(gameObject, delay);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
